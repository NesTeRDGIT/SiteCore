function SEND_SIGN(VALUE) {

    const dict = new Object();
    var index = 0;
    for (key in VALUE) {
        const value = VALUE[key];
        dict[`[${index}].Key`] = key;
        dict[`[${index}].Value`] = value;
        index++;
    }
  
    $.ajax({
        type: "POST",
        cache: false,
        traditional: true,
        url: "/MedpomReestr/SignData",
    
        data: dict,
        success: function (response) {
            debugger;
            alert("Успешно");
            $("#certProgressText").html("");
            $("#certProgress").hide();
            $("#container").html(response);
        },
        beforeSend: function () {
            $("#certProgressText").html("Ожидание ответа сервера...");
        },
        error: function (jqXHR, exception) {
          
            var msg = "";
            if (jqXHR.status === 0) {
                msg = "Not connect.\n Verify Network.";
            } else if (jqXHR.status == 404) {
                msg = "Requested page not found. [404]";
            } else if (jqXHR.status == 500) {
                msg = "Internal Server Error [500].";
            } else if (exception === "parsererror") {
                msg = "Requested JSON parse failed.";
            } else if (exception === "timeout") {
                msg = "Time out error.";
            } else if (exception === "abort") {
                msg = "Ajax request aborted.";
            } else {
                msg = "Uncaught Error.\n" + jqXHR.responseText;
            }
            $("#certProgress").hide();
            alert(msg);
        }
    });
}
var canPromise = !!window.Promise;
if (canPromise) {

    cadesplugin.then(function () {
            const t = Common_CheckForPlugIn();

        },
        function (error) {
            document.getElementById("PlugInEnabledTxt").innerHTML = error;
        }
    );

} else {
    window.addEventListener("message", function (event) {
        if (event.data == "cadesplugin_loaded") {
            CheckForPlugIn_NPAPI("isPlugInEnabled");

        } else if (event.data === "cadesplugin_load_error") {
            document.getElementById("PluginEnabledImg").setAttribute("src", "/Lib/Cades/Img/red_dot.png");
            document.getElementById("PlugInEnabledTxt").innerHTML = "Плагин не загружен";
        }
    },


        false);
    window.postMessage("cadesplugin_echo_request", "*");
}

function change(object) {

    const strUser = object.options[object.selectedIndex];
    const valueINFO = strUser.getAttribute("ID_SERT_INFO");
    const hint = document.getElementById("ID_SERT_INFO_" + valueINFO);

    const els = document.getElementsByClassName("VisibleToolTip"); // Creates an HTMLObjectList not an array.

    Array.prototype.forEach.call(els, function (el) {
        el.className = "HiddenToolTip";
    });

    if (hint) {
        hint.className = "VisibleToolTip";
    }

}