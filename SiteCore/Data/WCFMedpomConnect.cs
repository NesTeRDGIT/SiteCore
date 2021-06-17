using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ServiceLoaderMedpomData;
using System.ServiceModel;
using Microsoft.AspNetCore.SignalR;
using SiteCore.Hubs;


namespace SiteCore.Data
{
    public  class WCFConnect
    {

        private  IWcfInterface _MyWcfConnection;
        private  volatile bool InConnect;
        private string HOST { get; }
        private string UserName { get; }
        private string Password { get;}
        private ILogger logger { get; }
        private IHubContext<NotificationHub> NotificationHubContext { get; }

        public WCFConnect(string HOST,string UserName,string Password, ILogger logger, IHubContext<NotificationHub> NotificationHubContext)
        {
            this.HOST = HOST;
            this.UserName = UserName;
            this.Password = Password;
            this.logger = logger;
            this.NotificationHubContext = NotificationHubContext;
        }

        private  IWcfInterface MyWcfConnection
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

          void Connect()
        {
            InConnect = true;
            try
            {
                
                var addr = $@"net.tcp://{HOST}:12344/TFOMSMEDPOM.svc"; // Адрес сервиса
                var tcpUri = new Uri(addr);
                var address = new EndpointAddress(tcpUri, new DnsEndpointIdentity("MSERVICE"));

                var netTcpBinding = new NetTcpBinding(SecurityMode.None)
                {
                    ReaderQuotas = {MaxArrayLength = int.MaxValue, MaxBytesPerRead = int.MaxValue, MaxStringContentLength = int.MaxValue},
                    MaxBufferPoolSize = 105000000,
                    MaxReceivedMessageSize = 105000000,
                    SendTimeout = new TimeSpan(24, 0, 0),
                    ReceiveTimeout = new TimeSpan(24, 0, 0),
                    Security = {Mode = SecurityMode.TransportWithMessageCredential, Message = {ClientCredentialType = MessageCredentialType.UserName}, Transport = {ClientCredentialType = TcpClientCredentialType.None}}
                };

                var callback = new MyServiceCallback();
                var instanceContext = new InstanceContext(callback);

                var factory = new DuplexChannelFactory<IWcfInterface>(instanceContext, netTcpBinding, address);

                factory.Credentials.UserName.UserName = UserName;
                factory.Credentials.UserName.Password = Password;

                factory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;


                _MyWcfConnection = factory.CreateChannel(); // Создаём само подключение      
                _MyWcfConnection.Connect();
                callback.OnNewPackState += OnNewPackState;
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка при подключении к WCF: {ex.Message}", LogType.Error);
            }
            finally
            {
                InConnect = false;
            }
        }

        public  void OnNewPackState(string CODE_MO)
        {
            // Получаем контекст хаба
            NotificationHubContext.Clients.Groups(CODE_MO).SendAsync("NewPackState");
        }

     
        public  bool Ping()
        {
            try
            {
                return MyWcfConnection?.Ping() == true;
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка PING: {ex.FullError()}", LogType.Error);
                return false;
            }
        }

        public  StatusPriem StatusInvite()
        {
            try
            {
                return MyWcfConnection?.GetStatusInvite();
            }
            catch(Exception)
            {
                return null;
            }
        }

        public  bool ReestrEnabled()
        {
            try
            {
                var StatusInvite = this.StatusInvite();
                return StatusInvite is {AutoPriem: true, FLKInviterStatus: true};
            }
            catch (Exception)
            {
                return false;
            }
        }

        public  bool GetTypePriem()
        {
            try
            {
                var StatusInvite = this.StatusInvite();
                return StatusInvite is {TypePriem: true};
            }
            catch (Exception)
            {
                return false;
            }
        }

        public  FilePacketAndOrder GetPackForMO(string CODE_MO)
        {
            return MyWcfConnection.GetPackForMO(CODE_MO);
        }

        public  void AddFilePacketForMO(FilePacket fp)
        {
            try
            {
                MyWcfConnection.AddFilePacketForMO(fp);
            }
            catch (Exception ex)
            {
                throw new Exception("AddFilePacketForMO:" + ex.Message, ex);
            }


        }

        public  byte[] GetProtocol(string CODE_MO)
        {
            var ms = new MemoryStream();
            byte[] buff;
            var length = 0;
            while ((buff = MyWcfConnection.GetFile(CODE_MO, 0, TypeDOWLOAD.ZIP_ARCHIVE, length)).Length != 0)
            {
                ms.Write(buff, 0, buff.Length);
                length += buff.Length;
            }

            return ms.ToArray();

        }
    }

    [CallbackBehavior( UseSynchronizationContext = false)]
    public class MyServiceCallback : IWcfInterfaceCallback
    {

        public delegate void newNotifi();
        public newNotifi OnProgress = null;
        public void NewNotifi()
        {
            OnProgress?.Invoke();
        }
        public delegate void newPackState(string CODE_MO);
        public newPackState OnNewPackState = null;
        public void NewPackState(string CODE_MO)
        {
            OnNewPackState?.Invoke(CODE_MO);
        }

        public void NewFileManager()
        {

        }

        public void PING()
        {
            return;
        }
    }
}
