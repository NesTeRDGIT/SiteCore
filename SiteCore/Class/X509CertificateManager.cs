using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiteCore.Class;
using SiteCore.Data;

namespace SiteCore.Class
{

    public class CertAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class CertData
    {
        public List<string> Error { get; set; } = new();

        public string CAPTION => !string.IsNullOrEmpty(NAME) ? NAME : ORG;
        public string PublicKey { get; set; }

        public bool VALID => Error.Count == 0;
     
        /// <summary>
        /// Общее имя(CN)
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// Организация(O)
        /// </summary>
        public string ORG { get; set; }
        /// <summary>
        /// Подразделение(OU)
        /// </summary>
        public string PODR { get; set; }
        /// <summary>
        /// Должность(T)
        /// </summary>
        public string DOLG { get; set; }
        /// <summary>
        /// Адрес(ул., дом)(Street)
        /// </summary>
        public string Adres { get; set; }

        /// <summary>
        /// Населенный пункт(L)
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Регион(S)
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// ИНН(INN)
        /// </summary>
        public string INN { get; set; }

        /// <summary>
        /// ОГРН(OGRN)
        /// </summary>
        public string OGRN { get; set; }

        /// <summary>
        /// Страна(C)
        /// </summary>
        public string Country { get; set; }


        /// <summary>
        /// Фамилия(SN)
        /// </summary>
        public string FAM { get; set; }

        /// <summary>
        ///  Имя Отчество(GN)
        /// </summary>
        public string IM_OT { get; set; }

        /// <summary>
        /// Email(E)
        /// </summary>
        public string EMAIL { get; set; }

        /// <summary>
        /// СНИЛС(SNILS)
        /// </summary>
        public string SNILS { get; set; }
        public DateTime DATE_B { get; set; }
        public DateTime DATE_E { get; set; }

        public List<CertAttribute> OtherAttribute { get; set; } = new List<CertAttribute>();

        private static List<string> SplitData(string Data)
        {
            var result = new List<string>();
            var value = "";
            char[] separator = { ',' };
            char[] elementchar = { '"' };
            bool isValue = false;
            foreach (var c in Data)
            {
                if (elementchar.Contains(c))
                {
                    isValue = !isValue;
                    continue;
                }
                if (separator.Contains(c) && !isValue)
                {
                    result.Add(value.Trim());
                    value = "";
                }
                else
                {
                    value += c;
                }
            }

            return result;
        }
        public static CertData GetCertData(string data)
        {
            var attributes = SplitData(data).Select(x => x.Trim()).Select(x=>
            {
                var at = x.Split('=');
                if (at.Length != 2)
                    throw new Exception($"Ошибка парсинга атрибута={x}");
                return new CertAttribute { Name = at[0].Trim(), Value = at[1].Trim() };
            }).ToList();
            var result =new CertData();
            result.OtherAttribute.AddRange(attributes);
            result.ReadAttribute();
            return result;
        }
        public void ReadAttribute()
        {
            var removeAttribute = new List<CertAttribute>();
            foreach (var certAttribute in OtherAttribute)
            {
                var isremove = true;
                switch (certAttribute.Name)
                {
                    case "CN": NAME = certAttribute.Value; break;
                    case "O": ORG = certAttribute.Value;break;
                    case "OU":
                        PODR = certAttribute.Value; break;
                    case "T":
                        DOLG = certAttribute.Value; break;
                    case "STREET": Adres = certAttribute.Value; break;
                    case "L": City = certAttribute.Value; break;
                    case "S": Region = certAttribute.Value; break;
                    case "INN":
                    case "ИНН": INN = certAttribute.Value; break;
                    case "OGRN":
                    case "ОГРН": OGRN = certAttribute.Value; break;
                    case "C": Country = certAttribute.Value; break;
                    case "SN": FAM = certAttribute.Value; break;
                    case "G":
                    case "GN": IM_OT = certAttribute.Value; break;
                    case "E": EMAIL = certAttribute.Value; break;
                    case "SNILS":
                    case "СНИЛС": SNILS = certAttribute.Value; break;
                    default: isremove = false; break;
                }
                if(isremove)
                    removeAttribute.Add(certAttribute);
            }

            foreach (var certAttribute in removeAttribute)
            {
                OtherAttribute.Remove(certAttribute);
            }
        }
    }






    public interface IX509CertificateManager
    {
        public CertificateInfo GetInfo(byte[] file);
        public Task<bool> CheckIssuerAsync(CertData issuer);
    }


    public class CertificateInfo
    {
        public bool Valid => Data.Min(x => x.VALID);
        public List<CertData> Data { get; set; } = new();

        public List<string> Errors => Data.SelectMany(x => x.Error).ToList();
    }


    public class X509CertificateManager : IX509CertificateManager
    {
        private MyOracleSet myOracleSet;

        public X509CertificateManager(MyOracleSet myOracleSet)
        {
            this.myOracleSet = myOracleSet;
        }
        public CertificateInfo GetInfo(byte[] file)
        {
            var result = new CertificateInfo();
            var certificateChain = new X509Chain();
            certificateChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            var isCertificateChainValid = certificateChain.Build(new X509Certificate2(file));
            foreach (var chainElement in certificateChain.ChainElements)
            {
                var data = CertData.GetCertData(chainElement.Certificate.Subject);
                data.PublicKey = chainElement.Certificate.GetPublicKeyString();
                data.DATE_B = Convert.ToDateTime(chainElement.Certificate.GetEffectiveDateString());
                data.DATE_E = Convert.ToDateTime(chainElement.Certificate.GetExpirationDateString());
                data.Error.AddRange(chainElement.ChainElementStatus.Select(x=>x.StatusInformation));
                result.Data.Add(data);
            }
            return result;
        }

        public async Task<bool> CheckIssuerAsync(CertData issuer)
        {
            var item = await myOracleSet.SING_ISSUER.FirstOrDefaultAsync(x => x.PUBLICKEY == issuer.PublicKey && DateTime.Now.Date>= x.DATE_B && DateTime.Now.Date <= (x.DATE_E ?? DateTime.Now));
            return item != null;

        }
    }

}
