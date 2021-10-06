using System;
using System.Linq;
using System.Security.Cryptography.Pkcs;

namespace CryptoWCFService
{
    public static class Crypto
    {
       
        public static SIGN_Result CheckSignature(byte[] file, byte[] signature)
        {
            try
            {
                // Создаем объект ContentInfo по сообщению.
                // Это необходимо для создания объекта SignedCms.
                var contentInfo = new ContentInfo(file);

                // Объект, в котором будут происходить декодирование и проверка.
                // Свойство Detached устанавливаем явно в true, таким 
                // образом сообщение будет отделено от подписи.
                var signedCms = new SignedCms(contentInfo, true);
                
                // Декодируем сообщение.
                signedCms.Decode(signature);

                //  Проверяем число основных и дополнительных подписей.

                if (signedCms.SignerInfos.Count == 0)
                {
                    return SIGN_Result.CreateError("Документ не подписан");
                }

                var enumerator = signedCms.SignerInfos.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    try
                    {
                        // проверка только подписи
                        current.CheckSignature(false);
                        return SIGN_Result.CreateResult(current.Certificate.GetPublicKey().HexToString(), signedCms.GetSignTime());
                    }
                    catch (System.Security.Cryptography.CryptographicException e)
                    {
                        return SIGN_Result.CreateError(e.Message);
                    }
                }
                return SIGN_Result.CreateError("Документ не подписан");
            }
            catch (Exception ex)
            {
                return SIGN_Result.CreateError($"Ошибка при проверке подписи:{ex.Message}");
            }
        }

        public static SIGN_Result CheckSignature(string fileBase64, string signatureBase64)
        {
            return CheckSignature(Convert.FromBase64String(fileBase64), Convert.FromBase64String(signatureBase64));
        }
        public static SIGN_Result CheckSignature(byte[] file, string signatureBase64)
        {
            return CheckSignature(file, Convert.FromBase64String(signatureBase64));
        }
    }

    public class SIGN_Result
    {
        public bool Result { get; set; }
        public string Exception { get; set; }
        public string PublicKey { get; set; }
        public DateTime? DateSign { get; set; }

        public static SIGN_Result CreateError(string exception)
        {
            return new SIGN_Result { Exception = exception };
        }
        public static SIGN_Result CreateResult(string publicKey, DateTime? dateSign)
        {
            return new SIGN_Result { Result = true, PublicKey = publicKey, DateSign = dateSign};
        }
    }

    public static class Ext
    {
        public static string HexToString(this byte[] hashValue)
        {
            return hashValue.Aggregate("", (current, t) => current + $"{t:X2}");
        }

        public static DateTime? GetSignTime(this SignedCms cms)
        {
            foreach (var attribute in cms.SignerInfos[0].SignedAttributes)
            {
                if (attribute.Oid.Value == "1.2.840.113549.1.9.5")
                {
                    var pkcs9_time = new Pkcs9SigningTime(attribute.Values[0].RawData);
                    return pkcs9_time.SigningTime.ToLocalTime();
                }
            }

            return null;

        }
    }
}
