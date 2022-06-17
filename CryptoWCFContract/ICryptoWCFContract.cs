using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace CryptoWCFContract
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface ICryptoWCFContract
    {
        [OperationContract]
        CheckSignatureResult CheckSignatureBase64(byte[] file, string sign);

        [OperationContract]
        CheckSignatureResult CheckSignature(byte[] file, byte[] sign);

        [OperationContract]
        bool Connect();

    }

  
    [DataContract]
    public class CheckSignatureResult
    {
        [DataMember]
        public bool IsValidate { get; set; }
        [DataMember]
        public string PublicKey { get; set; }
        [DataMember]
        public DateTime? DateSign { get; set; }
        [DataMember]
        public List<string> ErrorList { get; set; } = new List<string>();
    }
}
