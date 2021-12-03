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
        private string HOST { get; }    
        private ILogger logger { get; }
        private IHubContext<NotificationHub, IHubClient> NotificationHubContext { get; }
        private CSOracleSet csOracleSet { get; }

        public WCFIdentiScaner(string HOST, ILogger logger, IHubContext<NotificationHub, IHubClient> NotificationHubContext, CSOracleSet csOracleSet)
        {
            this.HOST = HOST;            
            this.logger = logger;
            this.NotificationHubContext = NotificationHubContext;
            this.csOracleSet = csOracleSet;
        }

        private ICommunicationObject CommunicationObject => _MyWcfConnection as ICommunicationObject;


        private  IWCFIdentiScaner MyWcfConnection
        {
            get
            {
                if (_MyWcfConnection == null || !CommunicationObject.State.In(CommunicationState.Opened, CommunicationState.Opening))
                {
                    _MyWcfConnection = null;
                    _MyWcfConnection = Connect();
                }
                return _MyWcfConnection;
            }
        }

        private object InConnect = new ();

        private IWCFIdentiScaner Connect()
        {
            lock (InConnect)
            {
                try
                {
                    //Если 1 поток ждал лока то вернуть результат
                    if (_MyWcfConnection != null)
                        return _MyWcfConnection;

                    var addr = $@"net.tcp://{HOST}:44447/IdentiServer.svc"; // Адрес сервиса
                    var tcpUri = new Uri(addr);
                    var address = new EndpointAddress(tcpUri);
                    //  BasicHttpBinding basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None); //HTTP!
                    var netTcpBinding = new NetTcpBinding(SecurityMode.None)
                    {
                        ReaderQuotas = { MaxArrayLength = int.MaxValue, MaxBytesPerRead = int.MaxValue, MaxStringContentLength = int.MaxValue },
                        MaxBufferPoolSize = 105000000,
                        MaxReceivedMessageSize = 105000000,
                        SendTimeout = new TimeSpan(24, 0, 0),
                        ReceiveTimeout = new TimeSpan(24, 0, 0)
                    };

                    var callback = new IdentiScanerCallback();
                    var instanceContext = new InstanceContext(callback);

                    var factory = new DuplexChannelFactory<IWCFIdentiScaner>(instanceContext, netTcpBinding, address);

                    _MyWcfConnection = factory.CreateChannel(); 
                    _MyWcfConnection.Connect();
                    callback.onNewListState += Callback_onNewListState;
                    return _MyWcfConnection;
                }
                catch (Exception ex)
                {
                    logger?.AddLog($"Ошибка при подключении к WCF_IdentiServer: {ex.Message}", LogType.Error);
                    return null;
                }
            }
        }

        private void Callback_onNewListState(List<int> ID)
        {
            var list = csOracleSet.CS_LIST_IN.Where(x => ID.Contains(x.CS_LIST_IN_ID)).Select(x => x.CODE_MO).Distinct().ToArray();
            NotificationHubContext.Clients.Groups(NotificationHub.GetGroupNamesNewCSListState(list,true)).NewCSListState(ID);
        }

        public bool IsEnabled()
        {
            try
            {
                var conn = MyWcfConnection;
                return conn != null && conn.Ping();
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка WCF_IdentiServer(Ping): {ex.Message}", LogType.Error);
                return false;
            }
        }

        public Task<bool> IsEnabledAsync()
        {
            return Task.Run(IsEnabled);
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


    public static class CommunicationStateEx
    {
        public static bool In(this CommunicationState value,params CommunicationState[] items)
        {
            return items.Contains(value);
        }
    }
}
