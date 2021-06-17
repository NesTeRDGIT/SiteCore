$(function () {
    // Программное открытие окна выбора файла по щелчку
    $('figure').on('click', function () {
        $(':file').trigger('click');
    });
    // При перетаскивании файлов в форму, подсветить
    $('section').on('dragover', function (e) {
        $(this).addClass('dd');
        e.preventDefault();
        e.stopPropagation();
    });
    // Предотвратить действие по умолчанию для события dragenter
    $('section').on('dragenter', function (e) {
        e.preventDefault();
        e.stopPropagation();
    });
    $('section').on('dragleave', function (e) {
        $(this).removeClass('dd');
    });
    $('section').on('drop', function (e) {
        if (e.originalEvent.dataTransfer) {
            if (e.originalEvent.dataTransfer.files.length) {
                e.preventDefault();
                e.stopPropagation();

                // Вызвать функцию загрузки. Перетаскиваемые файлы содержатся
                // в свойстве e.originalEvent.dataTransfer.files
                upload(e.originalEvent.dataTransfer.files);
            }
        }
    });

    // Загрузка файлов классическим образом - через модальное окно
    $('#fileControl').on('change', function () {
        if ($(this).prop('files').length !== 0) {
            upload($(this).prop('files'));
            $("#fileControl").val('');
        }
    });
});

// Функция загрузки файлов
function upload(files) {
    // Пройти в цикле по всем файлам
    const formData = new FormData();
    let count = 0;
    for (let i = 0; i < files.length; i++) {
        let ext = files[i].name.substr(files[i].name.lastIndexOf('.') + 1);
        ext = ext.toUpperCase();
        if (ext !== "ZIP" && ext !== "XML") {
            alert(`Файл ${files[i].name} имеет не допустимый формат`);
            continue;
        }
        formData.append(`file_${i}`, files[i]);
        count++;
    }
    if (count!==0)
        SEND(formData);
}
function SEND(formData) {
    
    // Ajax-запрос на сервер
    $.ajax({
        type: 'POST',
        url: 'Upload', // URL на метод действия Upload контроллера MedpomReestrController
        data: formData,
        processData: false,
        contentType: false,
        beforeSend: ShowProgressBar(),
        error: ErrorPOST,
        success: successhandler,

        xhr: function () {
            const myXhr = $.ajaxSettings.xhr();
            if (myXhr.upload) {
                // For handling the progress of the upload
                myXhr.upload.addEventListener('progress', function (e) {
                    if (e.lengthComputable) {
                        const p = Math.round(e.loaded / e.total * 100);
                        $("#progress-bar-text").text(p + "%");
                        $("#progress-bar").attr("style", "width:" + p + "%");
                        if (p === 100) {
                            $("#progress-bar-text").text(p + " Обработка файлов...");
                        }
                    }
                }, false);

            }
            return myXhr;
        }
    });




};

function ErrorPOST(xhr, status, p3) {
    alert(xhr.statusText);
    HideProgressBar();
}

function HideProgressBar() {
    $('#progress-bar').attr('style', 'width:0%');
    $('#progress-bar-text').text('0%');
    $('#div-progress-bar').attr('hidden', 'hidden');
}

function ShowProgressBar(parameters) {
    $("#div-progress-bar").removeAttr('hidden');
    $("#progress-bar").attr('style', 'width:0%');
    $("#progress-bar-text").text('0%');
}
function successhandler(response) {
    HideProgressBar();
    $('#container').html(response);
}



