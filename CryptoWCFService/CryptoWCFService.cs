using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using CryptoWCFContract;

namespace CryptoWCFService
{
    public partial class CryptoWCFService : ServiceBase
    {
        private ILogger Logger = new LoggerEventLog("CryptoWCFServiceLog");
        public CryptoWCFService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (!StartServer())
            {
                this.Stop();
            }
        }

        protected override void OnStop()
        {
            Logger.AddLog("Остановка WCF сервера", LogType.Information);
            WcfConection?.Abort();
            Logger.AddLog("Служба остановлена", LogType.Information);
        }
        public static ServiceHost WcfConection { set; get; }
        private bool StartServer()
        {
            try
            {
                const string uri = @"net.tcp://localhost:15447/CryptoWCFService.svc"; // Адрес, который будет прослушивать сервер
                const string mex = @"http://localhost:8080/CryptoWCFService.svc";

                var netTcpBinding = new NetTcpBinding(SecurityMode.None)
                {
                    ReaderQuotas =
                    {
                        MaxArrayLength = int.MaxValue,
                        MaxBytesPerRead = int.MaxValue,
                        MaxStringContentLength = int.MaxValue
                    },
                    MaxBufferPoolSize = long.MaxValue,
                    MaxReceivedMessageSize = int.MaxValue,
                    MaxBufferSize = int.MaxValue,
                    OpenTimeout = new TimeSpan(24, 0, 0),
                    ReceiveTimeout = new TimeSpan(24, 0, 0),
                    SendTimeout = new TimeSpan(24, 0, 0),
                    PortSharingEnabled = true,
                    Security = { Mode = SecurityMode.None, Message = { ClientCredentialType = MessageCredentialType.None }, Transport = { ClientCredentialType = TcpClientCredentialType.None } }
                };

             
                WcfConection = new ServiceHost(typeof(WCF), new Uri(uri), new Uri(mex));
             
                var myEndpointAdd = new EndpointAddress(new Uri(uri));
                var ep = WcfConection.AddServiceEndpoint(typeof(ICryptoWCFContract), netTcpBinding, "");
                ep.Address = myEndpointAdd;

                WcfConection.OpenTimeout = new TimeSpan(24, 0, 0);
                WcfConection.CloseTimeout = new TimeSpan(24, 0, 0);

              
                #region МЕТАДАННЫЕ
                var smb = WcfConection.Description.Behaviors.Find<ServiceMetadataBehavior>() ?? new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                WcfConection.Description.Behaviors.Add(smb);
                WcfConection.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), $"mex");
                #endregion

                WcfConection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Logger.AddLog($"Ошибка при запуске WCF: {ex.Message}", LogType.Error);
                return false;
            }
        }

    }
}
