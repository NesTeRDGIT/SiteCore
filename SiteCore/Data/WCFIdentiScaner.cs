using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using IdentiModel;
using Microsoft.AspNetCore.SignalR;
using ServiceLoaderMedpomData;
using SiteCore.Hubs;



namespace SiteCore.Data
{
    public  class WCFIdentiScaner
    {
        private  IWCFIdentiScaner _MyWcfConnection;
        private  volatile bool InConnect = false;
        private string HOST { get; }
        private string UserName { get; }
        private string Password { get; }
        private ILogger logger { get; }
        private IHubContext<NotificationHub> NotificationHubContext { get; }
        private CSOracleSet csOracleSet { get; }

        public WCFIdentiScaner(string HOST, string UserName, string Password, ILogger logger, IHubContext<NotificationHub> NotificationHubContext, CSOracleSet csOracleSet)
        {
            this.HOST = HOST;
            this.UserName = UserName;
            this.Password = Password;
            this.logger = logger;
            this.NotificationHubContext = NotificationHubContext;
            this.csOracleSet = csOracleSet;
        }
        private  IWCFIdentiScaner MyWcfConnection
        {
            get
            {
                waitConnect();
                if (_MyWcfConnection == null)
                {
                    Connect();
                    return _MyWcfConnection;
                }
                if (((ICommunicationObject)_MyWcfConnection).State == CommunicationState.Faulted)
                {
                    Connect();
                    return _MyWcfConnection;
                }

                return _MyWcfConnection;
            }
        }



        private async void waitConnect()
        {
            while (InConnect)
            {
                await Task.Delay(300);
            }
        }


        private  void Connect()
        {
            InConnect = true;
            try
            {
                var addr = $@"net.tcp://{HOST}:44447/IdentiServer.svc"; // Адрес сервиса
                var tcpUri = new Uri(addr);
                var address = new EndpointAddress(tcpUri);
                //  BasicHttpBinding basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None); //HTTP!
                var netTcpBinding = new NetTcpBinding(SecurityMode.None)
                {
                    ReaderQuotas = {MaxArrayLength = int.MaxValue, MaxBytesPerRead = int.MaxValue, MaxStringContentLength = int.MaxValue},
                    MaxBufferPoolSize = 105000000,
                    MaxReceivedMessageSize = 105000000,
                    SendTimeout = new TimeSpan(24, 0, 0),
                    ReceiveTimeout = new TimeSpan(24, 0, 0)
                };


                // Ниже строки для того, чтоб пролазили таблицы развером побольше


                var callback = new IdentiScanerCallback();
                var instanceContext = new InstanceContext(callback);

                var factory = new DuplexChannelFactory<IWCFIdentiScaner>(instanceContext, netTcpBinding, address);

                _MyWcfConnection = factory.CreateChannel(); // Создаём само подключение      
                _MyWcfConnection.Connect();

                callback.onNewListState += Callback_onNewListState;


            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка при подключении к WCF_IdentiServer: {ex.Message}", LogType.Error);
            }
            finally
            {
                InConnect = false;
            }
        }

        private void Callback_onNewListState(List<int> ID)
        {
            var list = csOracleSet.CS_LIST.Where(x => ID.Contains(x.CS_LIST_ID)).GroupBy(x => x.CODE_MO);

            foreach (var item in list)
            {
                var CODE_MO = item.Key; 
                NotificationHubContext.Clients.Groups(CODE_MO).SendAsync("NewListState", ID);
            }
            NotificationHubContext.Clients.Group("Admin").SendAsync("NewListState", ID);
        }
        public bool IsEnabled
        {
            get
            {
                try
                {
                    return MyWcfConnection!=null && MyWcfConnection.Ping();
                }
                catch (Exception ex)
                {
                    logger?.AddLog($"Ошибка WCF_IdentiServer(Ping): {ex.Message}", LogType.Error);
                    return false;
                }
            }
        }

        public IdentiModel.EntriesMy[] GetLog()
        {
            return MyWcfConnection!=null ? MyWcfConnection.GetEventLogEntry() : new []{ new IdentiModel.EntriesMy { Message = "Нет связи с сервисом" } };
        }
    }


    [CallbackBehavior(UseSynchronizationContext = false)]
    public class IdentiScanerCallback : IWCFIdentiScanerCallback
    {

        public delegate void dNewListState(List<int> ID);

        public event dNewListState onNewListState;
        public void NewListState(List<int> ID)
        {
            onNewListState?.Invoke(ID);
        }

        public void PING()
        {

        }
    }
}
