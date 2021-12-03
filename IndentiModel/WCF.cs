using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace IdentiModel
{
    [ServiceContract(CallbackContract = typeof(IWCFIdentiScanerCallback),SessionMode = SessionMode.Required)]
    public interface IWCFIdentiScaner
    {
        [OperationContract]
        bool Connect();

        [OperationContract]
        bool Ping();

        [OperationContract]
        EntriesMy[] GetEventLogEntry();
    }


    /// <summary>
    /// Тип сообщения
    /// </summary>
    public enum TypeEntries
    {
        message = 0,
        error = 1,
        warning = 2
    }
    /// <summary>
    /// Мой Entries упрощеный
    /// </summary>
    public class EntriesMy
    {
        public DateTime TimeGenerated;
        public string Message;
        public TypeEntries Type;
    }

    [ServiceContract]
    public interface IWCFIdentiScanerCallback
    {
        [OperationContract(IsOneWay = true)]
        void NewListState(List<int> ID);

        [OperationContract(IsOneWay = true)]
        void PING();
    }

}
