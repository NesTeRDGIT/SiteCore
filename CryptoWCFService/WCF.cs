using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CryptoWCFContract;

namespace CryptoWCFService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, IncludeExceptionDetailInFaults = true, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class WCF: ICryptoWCFContract
    {
        public CheckSignatureResult CheckSignatureBase64(byte[] file, string sign)
        {
           return Crypto.CheckSignature(file, sign).ConvertToCheckSignatureResult();
        }

        public CheckSignatureResult CheckSignature(byte[] file, byte[] sign)
        {
            return Crypto.CheckSignature(file, sign).ConvertToCheckSignatureResult();
        }

        public bool Connect()
        {
            return true;
        }
    }

    public static class WCFExt
    {
        public static CheckSignatureResult ConvertToCheckSignatureResult(this SIGN_Result value)
        {
            var item = new CheckSignatureResult()
            {
                DateSign = value.DateSign,
                IsValidate = value.Result,
                PublicKey = value.PublicKey,
                ErrorList = new List<string>()
            };
            if (!string.IsNullOrEmpty(value.Exception))
                item.ErrorList.Add(value.Exception);
            return item;
        }
    }
}
