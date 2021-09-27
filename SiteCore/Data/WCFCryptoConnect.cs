using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using CryptoWCFContract;
using IdentiModel;
using ServiceLoaderMedpomData;

namespace SiteCore.Data
{
    public class WCFCryptoConnect
    {
        private ICryptoWCFContract _MyWcfConnection;
        private volatile bool InConnect = false;
        private string HOST { get; }
        private ILogger logger { get; }
       
        public WCFCryptoConnect(string HOST, ILogger logger)
        {
            this.HOST = HOST;
            this.logger = logger;
        }

        private ICryptoWCFContract MyWcfConnection
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


        private void Connect()
        {
            InConnect = true;
            try
            {
                var addr = $@"net.tcp://{HOST}:15447/CryptoWCFService.svc"; // Адрес сервиса
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


                // Ниже строки для того, чтоб пролазили таблицы развером побольше
                var factory = new ChannelFactory<ICryptoWCFContract>(netTcpBinding, address);

                _MyWcfConnection = factory.CreateChannel(); // Создаём само подключение      
                _MyWcfConnection.Connect();
            }
            catch (Exception ex)
            {
                logger?.AddLog($"Ошибка при подключении к WCFCryptoConnect: {ex.Message}", LogType.Error);
            }
            finally
            {
                InConnect = false;
            }
        }

        public Task<CheckSignatureResult> CheckSignatureBase64Async(byte[] file, string sign)
        {
            return Task.Run(() => MyWcfConnection?.CheckSignatureBase64(file, sign));
        }




    }
}
