import React, { Component, useRef, useEffect } from "react"

import Dialog from "@material-ui/core/Dialog";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import Spoller from "./Spoller.jsx"


const styles = {
    imgCenter: { border: 'ridge', display: 'block', margin: 'auto' },
    textCenter: { display: 'block', textAlign: 'auto' },
    imgContainer: { display: 'flex'},
    imgVerticalAlignCenter: { alignSelf: 'center' }
}



export default function InstructionDialog(props) {

    const { isOpen, onClose } = props;
  

    const handleClose = () => {
        onClose();
    };

  
    return (
        <div>
            <Dialog open={isOpen} onClose={handleClose} aria-labelledby="form-dialog-title" fullWidth={true} maxWidth={"lg"}>
                <DialogContent>
                    <DialogContentText>
                        Инструкция по использованию модуля
                    </DialogContentText>
                    <p className="RedText">Обращаем Ваше внимание, что сервис вступит в работу, только после заключения соглашение между Медицинскими организациями и ТФОМС Забайкальского края. Документ находится в стадии разработки</p>
                    <p className="GreenText">Однако если у вас установлен КриптоПро CSP Вы можете попробовать использовать ресурс(в рамках тестирования). Для этого свяжитесь с ТФОМС Забайкальского края: 21-26-69</p>
                    <p>Описание: Модуль предназначен для подписи документам между медицинскими организациями и ТФОМС Забайкальского края</p>
                    <p>Основные возможности:</p>
                    <ul>
                        <li>Скачивание файлов</li>
                        <li>Скачивание файлов совместно с подписями</li>
                        <li>Подпись файлов в браузере</li>
                        <li>Подпись файлов через загрузку файла открепленной подписи</li>
                    </ul>
                    <p className="TextCursiv">Для использование функции «Подпись файлов в браузере» Вам необходимо подготовить рабочее место. Для подготовки рабочего места желательно обратитесь к IT специалистам</p>

                    <Spoller caption="Подготовка рабочего места">
                        <ul>
                            <li>Установить <span className="BoldText">КриптоПро CSP</span>(не рассматривается в данной инструкции)</li>
                            <li>Установить <span className="BoldText">КриптоПро ЭЦП Browser plug-in</span></li>
                            <li>Установить <span className="BoldText">расширение КриптоПро ЭЦП Browser plug-in</span> для веб-браузере</li>
                            <li>Установить сертификат пользователя которым планируется подписывать документы</li>
                        </ul>
                        <Spoller caption="Установка КриптоПро ЭЦП Browser plug-in:">
                            <p>
                                При наличии интернета перейдите на сайт <a href="https://www.cryptopro.ru/products/cades/plugin" target="_blank">www.cryptopro.ru</a> скачайте продукт.<br/>
                                При отсутствии интернета скачать плагин можно c данного сайта: <a href="../SignInstunction/Soft/cadesplugin.exe" target="_blank">КриптоПро ЭЦП Browser plug-in </a><br/>
                                После загрузки запустите файл и следуйте инструкции установщика.<br/>
                                После установки в Вашем веб-браузере должно появится расширение: <img src="../SignInstunction/Image/PluginICO.png"/>
                            </p>
                            <p>Если при установке <span className="BoldText">КриптоПро ЭЦП Browser plug-in</span> по какой то причине не установилось расширение в веб-браузере, то можно установить их вручную</p>
                            <Spoller caption="Для Chrome:">
                                <p>Скачиваем файл расширения по следующей ссылке: <a href="../SignInstunction/Soft/Расширение CryptoPRO.crx" target="_blank">Расширение для Chrome</a><br/><br/>
                                    В Chrome Меню&#10140;Дополнительные инструменты&#10140;Расширения&#10140;Активируем режим разработчика&#10140;Переносим загруженный файл в рабочую область<br/><br/>
                                </p>
                                <p className="BoldText" align="center">Анимированная инструкция</p>
                                <img src="../SignInstunction/Image/Установка в Chrome.gif" alt="Анимированная инструкция" style={styles.imgCenter}/><br/><br/>
                            </Spoller>
                            <Spoller caption="Для Firefox:">
                                <p>
                                    Скачиваем файл расширения по следующей ссылке: <a href="../SignInstunction/Soft/firefox_cryptopro_extension_latest.xpi" target="_blank">Расширение для Firefox</a><br /><br />
                                    Меню&#10140;Дополнения и темы&#10140;Раздел «Расширения»&#10140;Инструменты для всех дополнений(значок шестеренки)&#10140;Установить дополнение из файла
                                </p>
                                <p className="BoldText" align="center">Анимированная инструкция</p>
                                <img src="../SignInstunction/Image/Установка в Firefox.gif" alt="Анимированная инструкция" style={styles.imgCenter}/><br/><br/>
                            </Spoller>
                        </Spoller>

                        <Spoller caption="Установка сертификата:">
                            <ul>
                                <li>Устанавливаем токен электронной цифровой подписи в USB порт компьютера</li>
                                <li>Открываем КриптоПро CSP&#10140;Cервис&#10140;Посмотреть сертификаты в контейнере</li>
                                <li>Обзор</li>
                                <li>Находим свой контейнер и жмем OK</li>
                                <li>Нажимаем далее и проверяем данные сертификата(ФИО итд)</li>
                                <li>Если в сертификате все верно, жмем установить сертификат</li>
                            </ul>
                            <p className="BoldText" align="center">Анимированная инструкция</p>
                            <img src="../SignInstunction/Image/Установка сертификата.gif" alt="Анимированная инструкция" style={styles.imgCenter}/><br/><br/>
                            <p>
                                Сертификат будет добавлен в личное хранилище текущего пользователя<br/>
                                Необходимо убедится, что все цепочка сертификатов валидна на текущем рабочем месте:
                            </p>
                            <ul>
                                <li>Открываем менеджер управления сертификатами(WIN+R или Пуск выполнить и вводим команду <span className="BoldText">certmgr.msc</span>)</li>
                                <li>Находим свои сертификат в разделе Личные&#10140;Сертификаты</li>
                                <li>Открываем его и проверяем, чтоб в разделе путь сертификации все сертификаты были действительными</li>
                            </ul>
                            <p className="BoldText" align="center"> Пример не действительной цепочки</p>

                            <img src="../SignInstunction/Image/Ошибка цепочки сертификатов.png" alt="Не действительной цепочка" style={styles.imgCenter}/><br/><br/>
                            <p>
                                Чтобы исправить ошибку как на изображении необходимо добавить сертификат "Минкомсвязи России" в <span className="BoldText">доверенные корневые центры сертификации</span>
                            </p>
                            <p className="BoldText" align="center">Анимированная инструкция</p>
                            <img src="../SignInstunction/Image/Установка издателей.gif" alt="Анимированная инструкция" style={styles.imgCenter}/><br/>
                            <p>
                                Если на Вашем рабочем месте отсутствует интернет, то Вам необходимо регулярно устанавливать списки отзывов центра сертификации который выдал Вам ЭЦП. Это необходимо делать для всех корневых и промежуточных центров сертификации<br/>
                                Информацию о точке распространения списков отзывов можно найти в сертификате
                            </p>
                            <p className="BoldText" align="center"> Информацию о точке распространения списков отзывов</p>
                            <img src="../SignInstunction/Image/Точка распространения.png" style={styles.imgCenter} />
                        </Spoller>
                        <Spoller caption="Обновление списков отзывов при отсутсвии сети Интернет">
                            <p>
                                ТФОМС Забайкальского края копирует списки отзывов(CRL), используемых центров сертификации на данный ресурс
                            </p>
                            <p>
                                Например, адрес точки распространния CRL для центра сертификации ФФОМС: http://<span className="BoldText">ucfoms.ffoms.ru</span>/cdp/630a8f435386a2e3f3e340c9adb5640bd9640dce.crl, данный файл вы можете найти на http://<span className="BoldText">mis.zabtfoms.ru</span>/cdp/630a8f435386a2e3f3e340c9adb5640bd9640dce.crl
                            </p>
                            <p>
                                Значит, Вы можете использовать 1 из 3 вариантов:
                                <ul>
                                    <li>Перед началом подписи документов устанавливать CRL с <a target="_blank" href="http://mis.zabtfoms.ru/cdp/630a8f435386a2e3f3e340c9adb5640bd9640dce.crl">http://mis.zabtfoms.ru/cdp/630a8f435386a2e3f3e340c9adb5640bd9640dce.crl</a> </li>
                                    <li>Поменять настройки локального DNS сервера указать ему что доменное имя <span className="BoldText">ucfoms.ffoms.ru</span>, находится на IP-адресе <span className="BoldText">mis.zabtfoms.ru</span></li>
                                    <li>Внести изменение в файл ..Windows\System32\drivers\etc\host на рабочем месте, указать в нем что доменное имя <span className="BoldText">ucfoms.ffoms.ru</span>, находится на IP-адресе <span className="BoldText">mis.zabtfoms.ru</span></li>
                                </ul>

                            </p>
                        </Spoller>
                    </Spoller>
                    <Spoller caption="Использование функций модуля">
                        <Spoller caption="Главное окно модуля">
                            <img src="../SignInstunction/Image/Главное окно.png" alt="Главное окно" style={styles.imgCenter} />
                            <p>Главное окно модуля состоит из:</p>
                            <ul>
                                <li>
                                    <div>
                                        Раздела «Список тем» - группа файлов с определенной темой(например Акты МЭК за январь 2021 года)<br />
                                        <span className="TextCursiv">Навигация по темам осуществляется с помощью щелчка левой кнопкой мыши на соответствующую тему</span>
                                        <span className="TextCursiv">Для повторного запроса с сервера списка тем используйте контекстное меню(щелчок правой кнопкой мыши на области тем)</span>
                                    </div>
                                </li>
                                <li>Раздела файлов на подпись – список файлов на подпись для выбранной темы</li>
                            </ul>
                            <p>Таблица списка файлов содержит следующие столбцы:</p>
                            <ul>
                                <li><span className="BoldText">Медицинская организация</span> – наименование Вашей медицинской организации</li>
                                <li><span className="BoldText">Имя файла</span> – наименование файла для подписи</li>
                                <li><span className="BoldText">Дата</span> – дата загрузки документа в информационную систему</li>
                                <li><span className="BoldText">Подписано</span> – Список лиц подпись которых ожидается. Если подписант подписал письмо, то значение будет <span className="GreenText">зеленого цвета </span>, если подписант не подписал документ значение будет <span className="RedText">красного цвета </span></li>
                                <li><span className="BoldText">Действия</span> – действия над файлами</li>
                            </ul>
                        </Spoller>
                        <Spoller caption="Действия над файлами">
                            <p>Всего в модуле существует 4 действия:</p>
                            <ul>
                                <li>
                                    <div style={styles.imgContainer}>
                                        <img src="../SignInstunction/Image/DownloadICO.png" alt="Скачать файл" style={styles.imgVerticalAlignCenter} />
                                        <span style={styles.imgVerticalAlignCenter}><span className="BoldText" >Скачать файл</span> – загрузить файл с сервера</span>
                                    </div>
                                </li>
                                <li>
                                    <div style={styles.imgContainer}>
                                        <img src="../SignInstunction/Image/DownloadArchiveICO.png" alt="Скачать файл с подписями" style={styles.imgVerticalAlignCenter} />
                                        <span style={styles.imgVerticalAlignCenter}><span className="BoldText">Скачать файл с подписями</span> – Загрузить файл и все его подписи</span>
                                    </div>
                                </li>

                                <li>
                                    <div style={styles.imgContainer}>
                                        <img src="../SignInstunction/Image/SignICO.png" alt="Подписать" style={styles.imgVerticalAlignCenter} />
                                        <span style={styles.imgVerticalAlignCenter}><span className="BoldText">Подписать</span> – подписать файл в браузере(возможна массовая подпись)</span></div>
                                </li>
                                <li>
                                    <div style={styles.imgContainer}>
                                        <img src="../SignInstunction/Image/SigFileICO.png" alt="Загрузить открепленную подпись" style={styles.imgVerticalAlignCenter} />
                                        <span style={styles.imgVerticalAlignCenter}><span className="BoldText">Загрузить открепленную подпись</span> – загрузить файл открепленной подписи к файлу</span>
                                    </div>
                                </li>
                            </ul>

                            <p>  Описание работы действий:</p>
                            <p>При активации "Скачать файл" - браузер загрузит файл на Ваш компьютер</p>
                            <p>При активации "Скачать файл с подписями" - браузер загрузит ZIP архив с файлом и всеми подписями на Ваш компьютер</p>

                            <Spoller caption="Подпись документа">
                                <p>Для подписи документа нажмите на кнопку «Подписать», после чего откроется следующие окно:</p>
                                <img src="../SignInstunction/Image/Окно подписи.png" alt="Окно подписи" style={styles.imgCenter} />
                                <p className="TextCursiv">Если плагин не доступен значит Ваше рабочее место не подготовлено к использованию ЭЦП</p>

                                <p className="TextCursiv">В данном окне необходимо выбрать сертификат пользователя которым вы хотите подпись файл и нажать на кнопку подписать</p>
                                <p className="TextCursiv">Браузер загрузит файл, подпишет его и отправит на сервер для проверки</p>
                                <p>В результате документ подпишется, если подпись валидна или не подпишется, и браузер отобразит ошибку</p>
                                <p>Возможные ошибки сервера, если подпись действительна:</p>
                                <ul>
                                    <li><span className="RedText">Файл не найден на сервере</span> – не удалось найти файла на сервере(возможно он был удален пока Вы готовились к подписи)</li>
                                    <li><span className="RedText">Не найдена дата подписи</span> – подпись не содержит дату подписания</li>
                                    <li><span className="RedText">Не найден владелец подписи</span> – не удалось найти подписанта в списке лиц допущенных к подписи документов</li>
                                    <li><span className="RedText">Данная подпись не ожидается для данного документа</span> – подписант найден в списке доступных, однако для данного документа не требуется его подпись</li>
                                    <li><span className="RedText">Документ уже подписан данной подписью</span> – файл уже подписан данным лицом</li>
                                </ul>
                            </Spoller>

                            <Spoller caption="Подпись документа путем загрузки открепленной подписи">
                                <p>Открепленная подпись - это файл содержащий в себе электронно-цифровую подпись в кодировке BASE64. Обычно такие файлы имеют расширение *.SIG</p>
                                <p>Для подпись документа путем загрузки открепленной подписи нажмите на кнопку «Загрузить открепленную подпись», после чего откроется следующие окно:</p>
                                <img src="../SignInstunction/Image/Окно открепленной подписи.png" alt="Окно открепленной подписи" style={styles.imgCenter} />
                              
                                <p className="TextCursiv">В данном окне необходимо выбрать файл для загрузки и нажать на кнопку "Отправить"</p>
                                <p className="TextCursiv">Браузер отправит файла на сервер для проверки</p>
                                <p>В результате документ подпишется, если подпись валидна или не подпишется, и браузер отобразит ошибку</p>
                                <p>Возможные ошибки сервера, если подпись действительна:</p>
                                <ul>
                                    <li><span className="RedText">Файл не найден на сервере</span> – не удалось найти файла на сервере(возможно он был удален пока Вы готовились к подписи)</li>
                                    <li><span className="RedText">Не найдена дата подписи</span> – подпись не содержит дату подписания</li>
                                    <li><span className="RedText">Не найден владелец подписи</span> – не удалось найти подписанта в списке лиц допущенных к подписи документов</li>
                                    <li><span className="RedText">Данная подпись не ожидается для данного документа</span> – подписант найден в списке доступных, однако для данного документа не требуется его подпись</li>
                                    <li><span className="RedText">Документ уже подписан данной подписью</span> – файл уже подписан данным лицом</li>
                                </ul>
                            </Spoller>
                        </Spoller>
                    </Spoller>








                    

                </DialogContent>
            </Dialog>
        </div>
    );
}