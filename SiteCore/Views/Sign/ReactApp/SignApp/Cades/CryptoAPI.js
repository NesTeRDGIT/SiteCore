var CAPICOM_CURRENT_USER_STORE = 2;
var CAPICOM_MY_STORE = "My";
var CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED = 2;
var CAPICOM_CERTIFICATE_FIND_SUBJECT_NAME = 1;
var CADESCOM_HASH_ALGORITHM_CP_GOST_3411_2012_256 = 101;
var CADESCOM_CADES_BES = 1;
var CADESCOM_BASE64_TO_BINARY = 1;

var ProviderSupport = [
    { FriendlyName: "ГОСТ Р 34.10-2012 256 бит", Value: "1.2.643.7.1.1.1.1", algorithm: cadesplugin.CADESCOM_HASH_ALGORITHM_CP_GOST_3411_2012_256 },
    { FriendlyName: "ГОСТ Р 34.10-2012 512 бит", Value: "1.2.643.7.1.1.1.2", algorithm: cadesplugin.CADESCOM_HASH_ALGORITHM_CP_GOST_3411_2012_512 }];


export async function activatePluginAsync() {
    try {
        if (cadesplugin) {
          
            await cadesplugin;
         
            const infoPlugin = await GetInfoPlugin();
          
            const certs = await LoadCert();
           
            return { infoPlugin, certs }
        } else {
            throw new Error("Объект плагина не найден");
        }
    } catch (err) {
        return { infoPlugin: { version: "", cspName: "", versionCSP: "", isEnabled: false, Error: err }, certs: [] }
    }
}




function SupportAlgorithm(algValue) {
    return ProviderSupport.find((value) => { return value.Value === algValue });
}


async function GetInfoPlugin() {
    try {
        const oAbout = await cadesplugin.CreateObjectAsync("CAdESCOM.About");
        const version = await (await oAbout.PluginVersion).toString();
        const cspName = await oAbout.CSPName(75);
        const ver = await oAbout.CSPVersion("", 75);
        const versionCSP = (await ver.MajorVersion) + "." + (await ver.MinorVersion) + "." + (await ver.BuildVersion);
        return { version, cspName, versionCSP, isEnabled: true };
    } catch (err) {
        return { version, cspName, versionCSP, isEnabled: false, Error: err };
    }
}

async function LoadCert() {

    const oStore = await cadesplugin.CreateObjectAsync("CAdESCOM.Store");
    if (!oStore) {
        alert("Не удалось открыть хранилище сертификатов");
        return null;
    }
    await oStore.Open(CAPICOM_CURRENT_USER_STORE,
        CAPICOM_MY_STORE,
        CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);

    let certList = [];
    const certs = await oStore.Certificates;
    const certCnt = await certs.Count;
    try {
        for (let i = 1; i <= certCnt; i++) {


            const cert = await certs.Item(i);
            const isPK = await cert.HasPrivateKey();
            let ProviderName = 'Нет КЛЮЧА';
            if (isPK == true) {
                const PrivateKey = await cert.PrivateKey;
                ProviderName = await PrivateKey.ProviderName;
            }

            const PublicKey = await cert.PublicKey();


            const Algorithm = await PublicKey.Algorithm;
            const SerialNumber = await cert.SerialNumber;
            const SubjectName = await cert.SubjectName;
            const algValue = await Algorithm.Value;

            const isSupportAlg = SupportAlgorithm(algValue) != null;
            const IssuerName = await cert.IssuerName;
            const FriendlyName = await Algorithm.FriendlyName;
            const ValidFromDate = new Date(await cert.ValidFromDate);
            const ValidToDate = new Date(await cert.ValidToDate);


            const CertName = GetCertName(SubjectName);
            const Issuer = GetIssuer(IssuerName);
            const FIO = GetCertFIO(SubjectName);
            if (isSupportAlg) {
                const certObj = {
                    SerialNumber,
                    SubjectName,
                    algValue,
                    isSupportAlg,
                    IssuerName,
                    FriendlyName,
                    ValidFromDate,
                    ValidToDate,
                    ProviderName,
                    CertName,
                    Issuer,
                    FIO
                };
                certList.push(certObj);
            }
        }
    } catch (ex) {
        alert(`Ошибка при перечислении сертификатов: ${cadesplugin.getLastError(ex)}`);
        certList = null;
    } finally {
        oStore.Close();
    }
    return certList;
}


function extract (from, what) {
    let certName = "";
    const begin = from.indexOf(what);

    if (begin >= 0) {
        const end = from.indexOf(', ', begin);
        certName = (end < 0) ? from.substr(begin) : from.substr(begin, end - begin);
    }
    return certName;
}

function GetCertName (certSubjectName) {
    return extract(certSubjectName, 'CN=');
}

function GetIssuer (certIssuerName) {
    return extract(certIssuerName, 'CN=');
}

function GetCertFIO(certSubjectName) {
    return `${extract(certSubjectName, 'SN=')} ${extract(certSubjectName, 'G=')}`;
}



export async function SignFile(dataInBase64, snCert) {
   
    const result = { Result: false, Error: "", SIGN: "" };

    try {
        const oStore = await cadesplugin.CreateObjectAsync("CAdESCOM.Store");
        if (!oStore) {
            throw new Error("Не удалось открыть хранилище сертификатов");
        }
        await oStore.Open(CAPICOM_CURRENT_USER_STORE, CAPICOM_MY_STORE, CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);


        const cert = await findCert(await oStore.Certificates, snCert);


        if (cert === null) {
            throw new Error("Не удалось найти сертификат");
        }

        const PublicKey = await cert.PublicKey();
        const Algorithm = await PublicKey.Algorithm;


        const algSing = SupportAlgorithm(await Algorithm.Value);
        if (algSing == undefined) {
            throw new Error('Алгоритм подписи не поддерживается');
        }
        const oSigner = await cadesplugin.CreateObjectAsync("CAdESCOM.CPSigner");
        await oSigner.propset_Certificate(cert);
        await oSigner.propset_CheckCertificate(true);
       
        const oSignedData = await cadesplugin.CreateObjectAsync("CAdESCOM.CadesSignedData");
        const oHashedData = await cadesplugin.CreateObjectAsync("CAdESCOM.HashedData");

        await oHashedData.propset_DataEncoding(cadesplugin.CADESCOM_BASE64_TO_BINARY);
        await oHashedData.propset_Algorithm(algSing.algorithm);
        
        
        await oHashedData.Hash(dataInBase64);
       
        const sSignedMessage = await oSignedData.SignHash(oHashedData, oSigner, cadesplugin.CADESCOM_CADES_BES);
     
        result.SIGN = sSignedMessage;
        result.Result = true;
        oStore.Close();
        return result;
    } catch (err) {
        result.Result = false;
        const e = cadesplugin.getLastError(err);
        if (e)
            result.Error = e;
        else
            result.Error = err.message;
        return result;
    }

}

async function findCert(oCertificates,snSert) {
    const count = await  oCertificates.Count;
    if (count > 1) {
        for (let i = 1; i <= count; i++) {
            const item = await oCertificates.Item(i);
            const ssn = await item.SerialNumber;
            if (ssn === snSert)
                return item;
        }
    }
    return null;
}



