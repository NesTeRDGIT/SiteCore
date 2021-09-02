var isPluginEnabled = false;
var fileContent; // Переменная для хранения информации из файла, значение присваивается в cades_bes_file.html

var CAPICOM_CURRENT_USER_STORE = 2;
var CAPICOM_MY_STORE = "My";
var CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED = 2;
var CAPICOM_CERTIFICATE_FIND_SUBJECT_NAME = 1;
var CADESCOM_HASH_ALGORITHM_CP_GOST_3411 = 100;
var CADESCOM_HASH_ALGORITHM_CP_GOST_3411_2012_256 = 101;
var CADESCOM_CADES_BES = 1;

var ProviderSupport = [{ FriendlyName: "ГОСТ Р 34.10-2012 256 бит", Value: "1.2.643.7.1.1.1.1", algorithm: CADESCOM_HASH_ALGORITHM_CP_GOST_3411_2012_256 }];


function FindAlgorithm(algValue) {

    for (var i = 0; i < ProviderSupport.length; i++) {
        if (ProviderSupport[i].Value === algValue) {
            return ProviderSupport[i].algorithm;
        }
    }
}


function getXmlHttp() {
    var xmlhttp;
    try {
        xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
    } catch (e) {
        try {
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        } catch (E) {
            xmlhttp = false;
        }
    }
    if (!xmlhttp && typeof XMLHttpRequest != 'undefined') {
        xmlhttp = new XMLHttpRequest();
    }
    return xmlhttp;
}


//Отправка подписей


function GetCert_NPAPI() {

    var certAr = {};
    var certIndex = 0;
    try {

        var oStore = cadesplugin.CreateObject("CAdESCOM.Store");
        oStore.Open();
    } catch (e) {
        alert("Certificate not found");
        return certAr;
    }

    var dateObj = new Date();
    var certCnt = oStore.Certificates.Count;


    try {
        for (var i = 1; i <= certCnt; i++) {
            var cert = oStore.Certificates.Item(i);
            if (dateObj < cert.ValidToDate && cert.HasPrivateKey() && cert.IsValid().Result) {
                certAr[certIndex] = cert;
                certIndex++;
            }
        }
    } catch (ex) {
        alert("Ошибка при перечислении сертификатов: " + cadesplugin.getLastError(ex));
        return certAr;
    }
    return certAr;
}

function printSert(cert) {

    var CertInfo = document.getElementById("CertInfo")
    var SelectCert = document.getElementById("SelectCert");
    var sertInfoData = document.getElementById("sertInfoData");
    for (var i = 0; i < sertInfoData.children.length; i++) {
        sertInfoData.removeChild(sertInfoData.children[i]);
    }


    SelectCert.innerHTML = '';
    var i = 1;
    for (certindex in cert) {
        var certItem = cert[certindex];
        var div = document.createElement("DIV");

        var vl = document.createElement("p");
        vl.innerHTML = "Владелец: <b>" + certItem.GetCertName() + "<b>";
        var FIO = document.createElement("p");
        FIO.innerHTML = "Имя владельца: <b>" + certItem.GetFIO() + "<b>";

        var is = document.createElement("p");
        is.innerHTML = "Издатель: <b>" + certItem.GetIssuer() + "<b>";
        var dv = document.createElement("p");
        dv.innerHTML = "Выдан: <b>" + certItem.GetCertFromDate() + "<b>";
        var dd = document.createElement("p");
        dd.innerHTML = "Действителен до: <b>" + certItem.GetCertTillDate() + "<b>";
        var cr = document.createElement("p");
        cr.innerHTML = "Криптопровайдер: <b>" + certItem.GetPrivateKeyProviderName() + "<b>";
        var alg = document.createElement("p");
        alg.innerHTML = "Алгоритм ключа: <b>" + certItem.GetPubKeyAlgorithm() + "<b>";
        div.appendChild(vl);
        div.appendChild(FIO);
        div.appendChild(is);
        div.appendChild(dv);
        div.appendChild(dd);
        div.appendChild(cr);
        div.appendChild(alg);
        div.id = "ID_SERT_INFO_" + i;
        div.className = "HiddenToolTip";
        sertInfoData.appendChild(div);
        var op = document.createElement("option");
        op.text = certItem.GetCertName();
        if (certItem.GetFIO() != "")
            op.text = certItem.GetCertName() + "(" + certItem.GetFIO() + ")";
        op.setAttribute("ID_SERT_INFO", i);
        op.setAttribute("SN", certItem.SN);
        op.setAttribute("CertName", certItem.GetCertName());
        SelectCert.appendChild(op);
        i++;
    }
}

function GetSertificat() {
    cadesplugin.set_log_level(cadesplugin.LOG_LEVEL_DEBUG);
    var canAsync = !!cadesplugin.CreateObjectAsync;

    if (canAsync) {
        include_async_code().then(function() {
            var t = GetSert_Async();
            t.then(
                function(result) {
                    printSert(result);
                },
                function(result) {
                    alert(result);
                }
            );
        });


    } else {
        try {
            printSert(GetSert());
        } catch (ex) {
            alert(ex);
        }
    }

}


function GetSert() {


    try {

        var oStore = cadesplugin.CreateObject("CAdESCOM.Store");
        if (!oStore) {
            alert("store failed");
            return;
        }

        oStore.Open(CAPICOM_CURRENT_USER_STORE,
            CAPICOM_MY_STORE,
            CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);
    } catch (ex) {
        alert("Certificate not found");
        return;
    }


    var certCnt;
    var certs;

    try {
        certs =  oStore.Certificates;
        certCnt =  certs.Count;
    }
    catch (ex) {
        alert(ex.message);
    }

      
    var certindex = 0;
    var certest  = {};
    for (var i = 1; i <= certCnt; i++) {
        var cert;
        try {
           
          
            cert = certs.Item(i);
            var ProviderName = 'Нет КЛЮЧА';
            if (cert.HasPrivateKey()) {
                var PrivateKey = cert.PrivateKey;
                 ProviderName = PrivateKey.ProviderName;
            }
            var PublicKey = cert.PublicKey();
            var Algorithm = PublicKey.Algorithm;

            if (FindAlgorithm(Algorithm.Value)) {
                var certObj = new CertificateObj(cert.SubjectName, cert.IssuerName, ProviderName, Algorithm.FriendlyName,
                    cert.ValidFromDate, cert.ValidToDate, cert.SerialNumber);
                certest[certindex] = certObj;
                certindex++;
            }
          
        }
        catch (ex) {
            alert("Ошибка при перечислении сертификатов: " + cadesplugin.getLastError(ex));
            return;
        }
    }
           
                
     oStore.Close();

    return certest;
}


var async_code_included = 0;
var async_Promise;
var async_resolve;
function include_async_code()
{
    if(async_code_included)
    {
        return async_Promise;
    }
    var fileref = document.createElement('script');
    fileref.setAttribute("type", "text/javascript");
    fileref.setAttribute("src", "../Lib/Cades/async_code.js?1034");
    document.getElementsByTagName("head")[0].appendChild(fileref);
    async_Promise = new Promise(function(resolve, reject){
        async_resolve = resolve;
    });
    async_code_included = 1;
    return async_Promise;
}


function SignNPAP(certSubjectName, SN, sHashValue) {

    var oStore = cadesplugin.CreateObject("CAdESCOM.Store");
    oStore.Open(CAPICOM_CURRENT_USER_STORE, CAPICOM_MY_STORE, CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);

    var oCertificate;
    var oCertificates = oStore.Certificates.Find(CAPICOM_CERTIFICATE_FIND_SUBJECT_NAME, certSubjectName);
    if (oCertificates.Count > 1) {
        for (var i = 1; i <= oCertificates.Count; i++) {
            if (oCertificates.Item(i).SerialNumber == SN)
                oCertificate = oCertificates.Item(i);
        }
        if (!oCertificate)
            throw ("Найдено более одного сертификата " + certSubjectName + ", но нет с SN = ${SN}");
    }
    if (oCertificates.Count === 0) {
        throw ("Сертификат " + certSubjectName + " не найден");
    }
    if (!oCertificate)
        oCertificate = oCertificates.Item(1);
 
    var PublicKey = oCertificate.PublicKey();
    var Algorithm = PublicKey.Algorithm;

    var algSIGN;

    algSIGN = FindAlgorithm(Algorithm.Value);
    if (algSIGN == undefined) {
        alert('Алгоритм подписи не поддерживается!');
        return;
    }

    var oHashedData = cadesplugin.CreateObject("CAdESCOM.HashedData");

    // Инициализируем объект заранее вычисленным хэш-значением
    // Алгоритм хэширования нужно указать до того, как будет передано хэш-значение
    oHashedData.Algorithm = algSIGN;
    oHashedData.SetHashValue(sHashValue);


    // Создаем объект CAdESCOM.CPSigner
    var oSigner = cadesplugin.CreateObject("CAdESCOM.CPSigner");
    oSigner.Certificate = oCertificate;

    // Создаем объект CAdESCOM.CadesSignedData
    var oSignedData = cadesplugin.CreateObject("CAdESCOM.CadesSignedData");

    return oSignedData.SignHash(oHashedData, oSigner, CADESCOM_CADES_BES);
}


function Common_CheckForPlugIn() {
    cadesplugin.set_log_level(cadesplugin.LOG_LEVEL_DEBUG);
    var canAsync = !!cadesplugin.CreateObjectAsync;
  
    if(canAsync)
    {
      include_async_code().then(function () {
          CheckForPlugIn_Async();
          var t = GetSert_Async();
          t.then(
                       function (result) {
                           printSert(result);
                       },
                       function(result)
                       {
                           alert(result);
                       }
                       );
      });
    }
    else
    {
        try
        {
            CheckForPlugIn_NPAPI();
            printSert(GetSert());
        }
        catch(ex)
        {
            alert(ex);
        }
    }
    
}

var isSigned = false;

function SignFiles() {
    try {
        if (isSigned) return;
        isSigned = true;
        var Progress = document.getElementById("certProgress");
        Progress.style.display = "block";
        $('#certProgressText').html("Подпись файлов...");
        var canAsync = !!cadesplugin.CreateObjectAsync;
        var HASHTABLE = GetListHashFiles();
        var e = document.getElementById("SelectCert");
        var SN = "";
        var SERTSUBJ;
        if (e.selectedIndex != -1) {
            SERTSUBJ = e.options[e.selectedIndex].getAttribute("CertName").replace('CN=', '');
            SN = e.options[e.selectedIndex].getAttribute("SN");
        } else {
            throw new Error("Не выбран сертификат");
        }
    } catch (error) {
        alert(error);
        Progress.style.display = "none";
        $('#certProgressText').html("");
        isSigned = false;
        return;
    }
 
    if (canAsync) {
        var promise = Sign_Async(SERTSUBJ, SN, HASHTABLE);
        promise.then(function(result) {
                $('#certProgressText').html("Отправка...");
                SEND_SIGN(result);
                isSigned = false;
            },
            function(result) {
                alert(result);
                $('#certProgressText').html("");
                Progress.style.display = "none";
                isSigned = false;
            });
    } else {
        try {
            var res = {};
            for (key in HASHTABLE) {
                var value = HASHTABLE[key];
                var t = SignNPAP(SERTSUBJ, SN, value);
                res[key] = t;
            }
            $('#certProgressText').html("Отправка...");
            SEND_SIGN(res);
            Progress.style.display = "none";
            $('#certProgressText').html("");
            isSigned = false;
        } catch (error) {
            alert(error);
            Progress.style.display = "none";
            isSigned = false;
            return;
        }
    }
}





function GetListHashFiles() {
    var HASHTABLE = {};
    //gets table
    var oTable = document.getElementById('fileList');
    //gets rows of table
    var rowLength = oTable.rows.length;
    //loops through rows
    for (i = 1; i < rowLength; i++) {
        //gets cells of current row
        var oCells = oTable.rows.item(i).cells;
        //gets amount of cells of current row
        var cellLength = oCells.length;
        var FILENAME = '';
        var HASH = '';
        //loops through each cell in current row
        for (var j = 0; j < cellLength; j++) {
            if (oCells.item(j).hasAttribute("hash"))
                HASH = oCells.item(j).innerHTML;
            if (oCells.item(j).hasAttribute("filename"))
                FILENAME = oCells.item(j).innerHTML;
            if (FILENAME != '' && HASH != '') {
                HASHTABLE[FILENAME] = HASH;
                FILENAME = "";
                HASH = "";
            }
        }
        
    }
    return HASHTABLE;
}



function FillCertInfo_NPAPI(certificate, certBoxID)
{
    var BoxID;
    var field_prefix;
    if(typeof(certBoxID) == 'undefined')
    {
        BoxID = 'cert_info';
        field_prefix = '';
    }else {
        BoxID = certBoxID;
        field_prefix = certBoxID;
    }

    var certObj = new CertificateObj(certificate);
    document.getElementById(BoxID).style.display = '';
    document.getElementById(field_prefix + "subject").innerHTML = "Владелец: <b>" + certObj.GetCertName() + "<b>";
    document.getElementById(field_prefix + "issuer").innerHTML = "Издатель: <b>" + certObj.GetIssuer() + "<b>";
    document.getElementById(field_prefix + "from").innerHTML = "Выдан: <b>" + certObj.GetCertFromDate() + "<b>";
    document.getElementById(field_prefix + "till").innerHTML = "Действителен до: <b>" + certObj.GetCertTillDate() + "<b>";
    document.getElementById(field_prefix + "provname").innerHTML = "Криптопровайдер: <b>" + certObj.GetPrivateKeyProviderName() + "<b>";
    document.getElementById(field_prefix + "algorithm").innerHTML = "Алгоритм ключа: <b>" + certObj.GetPubKeyAlgorithm() + "<b>";
}




function MakeVersionString(oVer)
{
    var strVer;
    if(typeof(oVer)=="string")
        return oVer;
    else
        return oVer.MajorVersion + "." + oVer.MinorVersion + "." + oVer.BuildVersion;
}

function CheckForPlugIn_NPAPI() {
    function VersionCompare_NPAPI(StringVersion, ObjectVersion)
    {
        if(typeof(ObjectVersion) == "string")
            return -1;
        var arr = StringVersion.split('.');

        if(ObjectVersion.MajorVersion == parseInt(arr[0]))
        {
            if(ObjectVersion.MinorVersion == parseInt(arr[1]))
            {
                if(ObjectVersion.BuildVersion == parseInt(arr[2]))
                {
                    return 0;
                }
                else if(ObjectVersion.BuildVersion < parseInt(arr[2]))
                {
                    return -1;
                }
            }else if(ObjectVersion.MinorVersion < parseInt(arr[1]))
            {
                return -1;
            }
        }else if(ObjectVersion.MajorVersion < parseInt(arr[0]))
        {
            return -1;
        }

        return 1;
    }

    function GetCSPVersion_NPAPI() {
        try {
           var oAbout = cadesplugin.CreateObject("CAdESCOM.About");
        } catch (err) {
            alert('Failed to create CAdESCOM.About: ' + cadesplugin.getLastError(err));
            return;
        }
        var ver = oAbout.CSPVersion("", 75);
        return ver.MajorVersion + "." + ver.MinorVersion + "." + ver.BuildVersion;
    }

    function GetCSPName_NPAPI() {
        var sCSPName = "";
        try {
            var oAbout = cadesplugin.CreateObject("CAdESCOM.About");
            sCSPName = oAbout.CSPName(75);

        } catch (err) {
        }
        return sCSPName;
    }

    function ShowCSPVersion_NPAPI(CurrentPluginVersion)
    {
        if(typeof(CurrentPluginVersion) != "string")
        {
            document.getElementById('CSPVersionTxt').innerHTML = "Версия криптопровайдера: " + GetCSPVersion_NPAPI();
        }
        var sCSPName = GetCSPName_NPAPI();
        if(sCSPName!="")
        {
            document.getElementById('CSPNameTxt').innerHTML = "Криптопровайдер: " + sCSPName;
        }
    }
    function GetLatestVersion_NPAPI(CurrentPluginVersion) {
             var xmlhttp = getXmlHttp();
        xmlhttp.open("GET", "../Lib/Cades/latest_2_0.txt", true);
        xmlhttp.onreadystatechange = function() {
            var PluginBaseVersion;
            if (xmlhttp.readyState == 4) {
                if(xmlhttp.status == 200) {
                    PluginBaseVersion = xmlhttp.responseText;
                    if (isPluginWorked) { // плагин работает, объекты создаются
                        if (VersionCompare_NPAPI(PluginBaseVersion, CurrentPluginVersion)<0) {
                            document.getElementById('PluginEnabledImg').setAttribute("src", "../Lib/Cades/Img/yellow_dot.png");
                            document.getElementById('PlugInEnabledTxt').innerHTML = "Плагин загружен, но есть более свежая версия.";
                        }
                    }
                    else { // плагин не работает, объекты не создаются
                        if (isPluginLoaded) { // плагин загружен
                            if (!isPluginEnabled) { // плагин загружен, но отключен
                                document.getElementById('PluginEnabledImg').setAttribute("src", "../Lib/Cades/Img/red_dot.png");
                                document.getElementById('PlugInEnabledTxt').innerHTML = "Плагин загружен, но отключен в настройках браузера.";
                            }
                            else { // плагин загружен и включен, но объекты не создаются
                                document.getElementById('PluginEnabledImg').setAttribute("src", "../Lib/Cades/Img/red_dot.png");
                                document.getElementById('PlugInEnabledTxt').innerHTML = "Плагин загружен, но не удается создать объекты. Проверьте настройки браузера.";
                            }
                        }
                        else { // плагин не загружен
                            document.getElementById('PluginEnabledImg').setAttribute("src", "../Lib/Cades/Img/red_dot.png");
                            document.getElementById('PlugInEnabledTxt').innerHTML = "Плагин не загружен.";
                        }
                    }
                }
            }
        }
        xmlhttp.send(null);
    }

    var isPluginLoaded = false;
    var isPluginWorked = false;
    var isActualVersion = false;
    try {
        var oAbout = cadesplugin.CreateObject("CAdESCOM.About");
        isPluginLoaded = true;
        isPluginEnabled = true;
        isPluginWorked = true;

        // Это значение будет проверяться сервером при загрузке демо-страницы
        var CurrentPluginVersion = oAbout.PluginVersion;
        if( typeof(CurrentPluginVersion) == "undefined")
            CurrentPluginVersion = oAbout.Version;
        
        document.getElementById('PluginEnabledImg').setAttribute("src", "../Lib/Cades/Img/green_dot.png");
        document.getElementById('PlugInEnabledTxt').innerHTML = "Плагин загружен.";
        document.getElementById('PlugInVersionTxt').innerHTML = "Версия плагина: " + MakeVersionString(CurrentPluginVersion);
        ShowCSPVersion_NPAPI(CurrentPluginVersion);
    }
    catch (err) {
        // Объект создать не удалось, проверим, установлен ли
        // вообще плагин. Такая возможность есть не во всех браузерах
        var mimetype = navigator.mimeTypes["application/x-cades"];
        if (mimetype) {
            isPluginLoaded = true;
            var plugin = mimetype.enabledPlugin;
            if (plugin) {
                isPluginEnabled = true;
            }
        }
    }
    GetLatestVersion_NPAPI(CurrentPluginVersion);
  
    
}



function CertificateObj(SubjectName, IssuerName, ProviderName, Algorithm, ValidFromDate, ValidToDate,SerialNumber)
{
    this.SubjectName = SubjectName;
    this.IssuerName = IssuerName;
    this.ProviderName = ProviderName;
    this.Algorithm = Algorithm;
    this.certFromDate = new Date(ValidFromDate);
    this.certTillDate = new Date(ValidToDate);
    this.SN = SerialNumber;
}


CertificateObj.prototype.check = function(digit)
{
    return (digit<10) ? "0"+digit : digit;
}

CertificateObj.prototype.extract = function(from, what)
{
    certName = "";
    var begin = from.indexOf(what);

    if(begin>=0)
    {
        var end = from.indexOf(', ', begin);
        certName = (end<0) ? from.substr(begin) : from.substr(begin, end - begin);
    }

    return certName;
}

CertificateObj.prototype.DateTimePutTogether = function(certDate)
{
    return this.check(certDate.getUTCDate())+"."+this.check(certDate.getMonth()+1)+"."+certDate.getFullYear() + " " +
                 this.check(certDate.getUTCHours()) + ":" + this.check(certDate.getUTCMinutes()) + ":" + this.check(certDate.getUTCSeconds());
}

CertificateObj.prototype.GetCertString = function()
{
    return this.extract(this.cert.SubjectName,'CN=') + "; Выдан: " + this.GetCertFromDate();
}

CertificateObj.prototype.GetCertFromDate = function()
{
    return this.DateTimePutTogether(this.certFromDate);
}

CertificateObj.prototype.GetCertTillDate = function()
{
    return this.DateTimePutTogether(this.certTillDate);
}

CertificateObj.prototype.GetPubKeyAlgorithm = function()
{
    return this.Algorithm;
}

CertificateObj.prototype.GetCertName = function()
{
    return this.extract(this.SubjectName, 'CN=');
}


CertificateObj.prototype.GetFIO = function()
{
    return (this.extract(this.SubjectName, 'SN=').replace('SN=', '') +" "+ this.extract(this.SubjectName, 'G=').replace('G=', '')).trim();
}



CertificateObj.prototype.GetIssuer = function()
{
    return this.extract(this.IssuerName, 'CN=');
}

CertificateObj.prototype.GetPrivateKeyProviderName = function()
{
    return this.ProviderName;
}

