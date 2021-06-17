using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;

namespace SiteCore.Class
{
    public interface IHasher
    {
        string GetHash(string sourceFile);
        SING_Result Viryfi(byte[] SIGN, byte[] srcData);
    }
    public class SING_Result
    {
        public bool Valid { get; set; }
        public string Comment { get; set; }
        public byte[] PublicKey { get; set; }
        public string SN { get; set; }
    }

    public class GostHasher : IHasher
    {
        public string GetHash(string sourceFile)
        {
            return "";
        }


        private byte[] ReadFile(string sourceFile)
        {
            using var inStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
            using var ms = new MemoryStream();
            inStream.CopyTo(ms);
            return ms.ToArray();
        }


        private  string HexToString(IEnumerable<byte> hashValue)
        {
            return hashValue.Aggregate("", (current, t) => current + $"{t:X2}");
        }


        public SING_Result Viryfi(byte[] SIGN, byte[] srcData)
        {
            try
            {
                var contentInfo = new ContentInfo(srcData);

                var signedCms = new SignedCms(contentInfo, true);
                signedCms.Decode(SIGN);
                if (signedCms.SignerInfos.Count == 0)
                {
                    return new SING_Result{ Valid = false, Comment = "Документ не подписан." };
                }
                var result = new SING_Result();
                var enumerator = signedCms.SignerInfos.GetEnumerator();
                var owner_SN = signedCms.Certificates[0].SerialNumber.ToUpper();

                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    try
                    {
                        // проверка только подписи
                        current.CheckSignature(false);
                        result.SN = current.Certificate.SerialNumber.ToUpper();
                        result.PublicKey = current.Certificate.GetPublicKey();
                        result.Valid = true;
                    }
                    catch (CryptographicException e)
                    {
                        result.Valid = false;
                        result.Comment = e.Message;
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при проверке подписи:{ex.Message}", ex);
            }
        }
    }
}
