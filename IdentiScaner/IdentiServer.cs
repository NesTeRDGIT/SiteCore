using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentiModel;
using System.Configuration;
using System.IO;
using CRZ_IDENTITIFI;
using System.ServiceModel;

namespace IdentiScaner
{
    public partial class IdentiServer : ServiceBase
    {
        public IdentiServer()
        {
            InitializeComponent();
        }

        private string PathIn = ConfigurationManager.AppSettings["PathIn"];
        private string PathOut = ConfigurationManager.AppSettings["PathOut"];
        private int TimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOut"]) * 1000;

        protected override void OnStart(string[] args)
        {
            AddLog("Запуск службы", EventLogEntryType.Information);
            if (StartServer())
            {
                Start();
            }

            else
            {
                AddLog("Остановка службы", EventLogEntryType.Information);
                this.Stop();
            }
        }



        protected override void OnStop()
        {
            CTS.Cancel();
        }


        private Thread InTh;
        private Thread OutTh;
        private Thread NotifyTh;
        CancellationTokenSource CTS = new CancellationTokenSource();

        private void Start()
        {
            
            InTh = new Thread(InThread) {IsBackground = true};
            OutTh = new Thread(OutThread) {IsBackground = true};
            NotifyTh = new Thread(NotifyThread) { IsBackground = true };
            
            InTh.Start();
            OutTh.Start();
            NotifyTh.Start();
        }


        WCFService wi;
        public ServiceHost WcfConection { set; get; }
        private bool StartServer()
        {
            try
            {
                AddLog("Запуск WCF", EventLogEntryType.Information);
                const string uri = @"net.tcp://localhost:44447/IdentiServer.svc"; // Адрес, который будет прослушивать сервер
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
                    SendTimeout = new TimeSpan(24, 0, 0)
                };
                wi = new WCFService();

                WcfConection = new ServiceHost(wi, new Uri(uri)); // Запускаем прослушивание
                //var myEndpointAdd = new EndpointAddress(new Uri(uri), EndpointIdentity.CreateDnsIdentity("NESTER"));
                var myEndpointAdd = new EndpointAddress(new Uri(uri));

                netTcpBinding.ReceiveTimeout = new TimeSpan(24, 0, 0);
              
                var ep = WcfConection.AddServiceEndpoint(typeof(IWCFIdentiScaner), netTcpBinding, "");
                ep.Address = myEndpointAdd;
                WcfConection.OpenTimeout = new TimeSpan(24, 00, 0);
                WcfConection.CloseTimeout = new TimeSpan(24, 00, 0);
                WcfConection.Credentials.UserNameAuthentication.CachedLogonTokenLifetime = new TimeSpan(24, 0, 0);

               
            

                WcfConection.Open();
                AddLog("WCF запущен", EventLogEntryType.Information);
                return true;
            }
            catch (Exception ex)
            {
                AddLog("Ошибка при запуске WCF: " + ex.Message, EventLogEntryType.Error);
                return false;
            }
        }

        private string FileNamePrefix = "SITE";

   
        private bool CheckFileAv(string path, int NOT_FOUND_COUNT = 1)
        {
            var NOT_FOUND = 0;
            PrichinAv? pr;
            while (!Ext.CheckFileAv(path, out pr))
            {
                if (pr == PrichinAv.NOT_FOUND)
                {
                    AddLog("Не удалось найти файл " + path, EventLogEntryType.Error);
                }
                NOT_FOUND++;
                if (NOT_FOUND >= NOT_FOUND_COUNT)
                {
                    return false;
                }
                Thread.Sleep(1500);
            }
            return true;

        }

        private void InThread()
        {
            while (!CTS.IsCancellationRequested)
            {
                try
                {
                    var Pattern = $"76000-{FileNamePrefix}*";
                    var files = Directory.GetFiles(PathIn, Pattern).OrderBy(x => x.ToUpper());
                    foreach (var f in files)
                    {
                        if (!CheckFileAv(f, 3)) continue;
                        switch (Path.GetExtension(f).ToUpper())
                        {
                            case ".UPRAK1":
                                ProccesUprak1(f);
                                break;
                            case ".UPRAK2":
                                ProccesUprak2(f);
                                break;
                        }
                        File.Delete(f);
                    }

                }
                catch (Exception e)
                {
                    AddLog($"{e.FullError()}({e.StackTrace})", EventLogEntryType.Error);
                }

                Thread.Sleep(TimeOut);

            }
        }


        private List<int> UpdateStateList = new List<int>();

        private void AddUpdateStateList(int ID)
        {
            Monitor.Enter(UpdateStateList);
            UpdateStateList.Add(ID);
            Monitor.Exit(UpdateStateList);
        }

        private void NotifyThread()
        {
            while (!CTS.IsCancellationRequested)
            {
                try
                {
                    Monitor.Enter(UpdateStateList);
                    if (UpdateStateList.Count != 0)
                    {
                        Notify(UpdateStateList);
                        UpdateStateList.Clear();
                    }
                }
                catch (Exception e)
                {
                    AddLog(e.Message, EventLogEntryType.Error);
                }
                finally
                {
                    Monitor.Exit(UpdateStateList);
                }
                Thread.Sleep(2000);

            }
        }

        private void Notify(List<int> ID)
        {
            var DEL = new List<int>();
            foreach (var cu in BankAcc.Cards_user)
            {
                if (cu.Value.IsOpen)
                {
                    cu.Value.Context.GetCallbackChannel<IWCFIdentiScanerCallback>().NewListState(ID);
                }
                else
                {
                    DEL.Add(cu.Key);
                }
            }


            foreach (var d in DEL)
            {
                BankAcc.Cards_user.Remove(d);
            }
        }

        private int GetID(string val)
        {
            try
            {
                return Convert.ToInt32(val.Replace($"{FileNamePrefix}-", "").Replace("*76000-", "").Replace(".uprmes",""));
            }
            catch (Exception e)
            {
               
                throw new Exception($"Ошибка в GetID, val={val}, FileNamePrefix={FileNamePrefix}: {e.Message}");
            }
           
        }
        private void ProccesUprak1(string path)
        {
            using (var db = new CSOracleSet("DefaultConnection"))
            {
                var FLK = IDENTITIFI_PACK.ReadFLK(path);
                var ID = GetID(FLK.ID);
                var list = db.CS_LIST.FirstOrDefault(x => x.CS_LIST_ID == ID);
                if (list == null)
                {
                    AddLog($"Пакет с ID = [{FLK.ID}] не найден", EventLogEntryType.Error);
                    return;
                }
                if (FLK.FLK.Count != 0)
                {
                    var FatalError = new List<ErrorFLK>();
                    foreach (var flk in FLK.FLK)
                    {
                        int in_id = 0;
                        if (int.TryParse(flk.ID, out in_id))
                        {
                            var list_in = list.CS_LIST_IN.FirstOrDefault(x => x.CS_LIST_IN_ID == in_id);
                            if (list_in != null)
                            {
                                list_in.COMM = flk.Message;
                                list_in.STATUS = false;
                                continue;
                            }
                        }
                        FatalError.Add(flk);
                    }
                    list.COMM = string.Join(";", FatalError.Select(x => $"{x.ID}-{x.Message}"));
                }

                list.STATUS = FLK.IsFatalFLK ? StatusCS_LIST.Error : StatusCS_LIST.FLK;
                db.SaveChanges();
                AddUpdateStateList(list.CS_LIST_ID);
            }
        }

        private void ProccesUprak2(string path)
        {
            using (var db = new CSOracleSet("DefaultConnection"))
            {
                var RSLT = IDENTITIFI_PACK.ReadRSLT(path);
                var ID = Convert.ToInt32(GetID(RSLT.ID));
                var list = db.CS_LIST.FirstOrDefault(x => x.CS_LIST_ID == ID);
                if (list == null)
                {
                    AddLog($"Пакет с ID = [{RSLT.ID}] не найден", EventLogEntryType.Error);
                    return;
                }

                foreach (var rslt in RSLT.RESULT)
                {
                    var CS_LIST_IN = list.CS_LIST_IN.FirstOrDefault(x => x.CS_LIST_IN_ID == Convert.ToInt32(rslt.ID));

                    if (CS_LIST_IN != null)
                    {
                        var pii = rslt as PersIdentInfo;
                        db.CS_LIST_IN_RESULT_SMO.RemoveRange(CS_LIST_IN.CS_LIST_IN_RESULT.SelectMany(x => x.CS_LIST_IN_RESULT_SMO));
                        db.CS_LIST_IN_RESULT.RemoveRange(CS_LIST_IN.CS_LIST_IN_RESULT);

                        if (pii != null)
                        {
                            foreach (var info in pii.Info)
                            {
                                var result = new CS_LIST_IN_RESULT
                                {
                                    DDEATH = info.Ddeath,
                                    DR = info.DR,
                                    ENP = info.MAINENP,
                                    LVL_D = info.LVL_D,
                                    LVL_D_KOD = string.Join(";", info.LVL_D_kod)
                                };
                               
                                CS_LIST_IN.CS_LIST_IN_RESULT.Add(result);

                                foreach (var p in info.RSLT)
                                {
                                    var smo = new CS_LIST_IN_RESULT_SMO
                                    {
                                        DATE_B = p.DateStartSMO,
                                        DATE_E = p.DateEndSMO,
                                        ENP = p.ENP,
                                        SMO = p.SMO,
                                        TYPE_SMO = (IdentiModel.TypeSMO)Convert.ToInt32(p.TYPE_SMO),
                                        SMO_OK = p.KOD_terr,
                                        TF_OKATO = p.TFOMS,
                                        VPOLIS = p.TypePol.ToVPOLIS_VALUES()
                                    };


                                    string sp;
                                    string np;
                                    ConvertOldPolis(p.SNPol, out sp, out np);
                                    smo.SPOLIS = sp;
                                    smo.NPOLIS = np;
                                    result.CS_LIST_IN_RESULT_SMO.Add(smo);
                                }
                                CS_LIST_IN.STATUS = true;
                            }
                        }

                        if (rslt is PersIdentFail)
                        {
                            CS_LIST_IN.STATUS = false;
                        }
                    }

                }
               
                list.STATUS = StatusCS_LIST.Answer;
                db.SaveChanges();
                AddUpdateStateList(list.CS_LIST_ID);
            }
        }

        private void ConvertOldPolis(string snpolis, out string spolis, out string npolis)
        {
            snpolis = snpolis.Trim();
            var split = snpolis.Split('№');

            if (split.Length == 2)
            {
                spolis = split[0].Trim();
                npolis = split[1].Trim();
            }
            else
            {
                spolis = "";
                npolis = snpolis;
            }
        }

        private void CreateUprmes(CS_LIST list)
        {
            var List = new List<PersItem>();
            foreach (var item in list.CS_LIST_IN.ToList())
            {

                var pp = new PersItem
                {
                    ID_PACIENT = item.CS_LIST_IN_ID,
                    ID_PERS = item.CS_LIST_IN_ID,
                    FAM = item.FAM?.ToUpper().Trim(),
                    IM = item.IM?.ToUpper().Trim(),
                    OT = item.OT?.ToUpper().Trim(),
                    DR = item.DR,
                    W = (Pol) item.W

                };
                pp.DateSluch = null;

                if (!string.IsNullOrEmpty(item.DOC_TYPE))
                    pp.DOC_TYPE = Convert.ToInt32(item.DOC_TYPE);


                pp.VPOLIS = item.VPOLIS.ToTypeDPFS();
                pp.DOC_NUM = item.DOC_NUM?.ToUpper().Trim();
                pp.DOC_SER = item.DOC_SER?.ToUpper().Trim();
                if (pp.VPOLIS == TypeDPFS.bOMS || pp.VPOLIS == TypeDPFS.eOMS || pp.VPOLIS == TypeDPFS.uOMS)
                    pp.ENP = item.NPOLIS?.ToUpper().Trim();

                pp.NPOLIS = item.NPOLIS?.ToUpper().Trim();
                pp.SPOLIS = item.SPOLIS?.ToUpper().Trim();
                pp.SNILS = item.SNILS?.ToUpper().Trim();
                List.Add(pp);
            }

            var PACK = CRZ_IDENTITIFI.IDENTITIFI_PACK.GetListXMLNEW(List,
                $"{FileNamePrefix}-{list.CS_LIST_ID.ToString()}");
            using (var st = File.Create(Path.Combine(PathOut, $"76000-{FileNamePrefix}-{list.CS_LIST_ID}.uprmes")))
            {
                UPRMessageBatch.SaveToFile(st, PACK);
            }
        }

        private void OutThread()
        {

            while (!CTS.IsCancellationRequested)
            {
                using (var db = new CSOracleSet("DefaultConnection"))
                {
                    try
                    {
                        var list = db.CS_LIST.Where(x => x.STATUS == StatusCS_LIST.OnSend).ToList();
                        foreach (var l in list)
                        {
                            CreateUprmes(l);
                            l.STATUS = StatusCS_LIST.Send;
                            db.SaveChanges();
                            AddUpdateStateList(l.CS_LIST_ID);
                        }

                    }
                    catch (Exception e)
                    {
                        AddLog(e.Message, EventLogEntryType.Error);
                    }

                    Thread.Sleep(TimeOut);
                }
            }
        }



        public static void AddLog(string log, EventLogEntryType type)
        {
            try
            {
                var el = new EventLog();
                if (!EventLog.SourceExists("IdentiServer"))
                {
                    EventLog.CreateEventSource("IdentiServer", "IdentiServer");
                }

                el.Source = "IdentiServer";
                el.WriteEntry(log, type);
            }

            catch
            {
                // ignored
            }
        }

    }

    public  static class Ext
    {
        public static TypeDPFS? ToTypeDPFS(this VPOLIS_VALUES? val)
        {
            if (val.HasValue)
            {
                switch (val.Value)
                {
                    case VPOLIS_VALUES.OLD:return  TypeDPFS.OLD;
                    case VPOLIS_VALUES.TEMP_B:return TypeDPFS.TEMP_B;
                    case VPOLIS_VALUES.bOMS:return TypeDPFS.bOMS;
                    case VPOLIS_VALUES.eOMS:return TypeDPFS.eOMS;
                    case VPOLIS_VALUES.uOMS:return TypeDPFS.uOMS;
                    case VPOLIS_VALUES.NOT:return TypeDPFS.NOT;
                }
            }

            return null;
        }


        public static VPOLIS_VALUES? ToVPOLIS_VALUES(this TypeDPFS? val)
        {
            if (val.HasValue)
            {
                switch (val.Value)
                {
                    case TypeDPFS.OLD: return VPOLIS_VALUES.OLD;
                    case TypeDPFS.TEMP_B: return VPOLIS_VALUES.TEMP_B;
                    case TypeDPFS.bOMS: return VPOLIS_VALUES.bOMS;
                    case TypeDPFS.eOMS: return VPOLIS_VALUES.eOMS;
                    case TypeDPFS.uOMS: return VPOLIS_VALUES.uOMS;
                    case TypeDPFS.NOT: return VPOLIS_VALUES.NOT;
                }
            }

            return null;
        }


       
        public static bool CheckFileAv(string path, out PrichinAv? Pr)
        {
            Stream stream = null;
            try
            {
                stream = File.Open(path, FileMode.Open, FileAccess.Read);
                stream.Close();
                stream.Dispose();
                Pr = null;
                return true;
            }
            catch (FileNotFoundException)
            {
                Pr = PrichinAv.NOT_FOUND;
                stream?.Dispose();
                return false;
            }
            catch (Exception)
            {
                Pr = PrichinAv.EXEPT;
                stream?.Dispose();
                return false;
            }
        }


        public static string FullError(this Exception ex)
        {
            var rzlt = ex.Message;
            if (ex.InnerException != null)
                rzlt += "||" + ex.InnerException.FullError();
            return rzlt;
        }


    }
    public enum PrichinAv
    {
        EXEPT,
        NOT_FOUND
    }


    public class BankAcc
    {

        public static Dictionary<int, USER> Cards_user = new Dictionary<int, USER>();
        public static void AddAcc(OperationContext oc)
        {
            var id = 0;
            if (Cards_user.Count != 0)
            {
                id = Cards_user.Max(x => x.Key)+1;
            }

            Cards_user.Add(id, new USER() {Context = oc, ID = id});
        }
    }


    public class USER
    {
        public int ID { get; set; }
       
        public OperationContext Context { get; set; }

        public bool IsOpen
        {
            get
            {
                if (Context == null) return false;
                if (Context.Channel.State != CommunicationState.Opened)
                    return false;
                try
                {
                    Context.GetCallbackChannel<IWCFIdentiScanerCallback>().PING();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public void Abort()
        {
            Context?.Channel.Abort();
        }
    }


}
