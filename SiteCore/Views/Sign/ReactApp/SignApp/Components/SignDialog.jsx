import React, { Component, useRef, useEffect } from "react"
import ReactDOM from "react-dom"

import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";

import { makeStyles } from '@material-ui/core/styles';

import Grid from '@material-ui/core/Grid';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import { format } from "date-fns";

import { activatePluginAsync, SignFile } from '../Cades/CryptoAPI.js';

import Button from "@material-ui/core/Button";
import CircularProgress from '@material-ui/core/CircularProgress';

export default function SingDialog(props) {
    const { onClose, isOpen, onSave, files } = props;
    const [pluginInfo, setPluginInfo] = React.useState(null);
    const [signList, setSignList] = React.useState([]);

    const [certs, setCerts] = React.useState([]);
    const [currentCert, setCurrentCert] = React.useState([]);

    const [signProcess, setSignProcess] = React.useState(false);

    const handleClose = () => {
        onClose();
    };
    const [checkingPlugin, setCheckingPlugin] = React.useState(false);
    useEffect(async () => {
        if (isOpen) {
            try {
                setSignErr("");
                setCheckingPlugin(true);
                const data = await activatePluginAsync();
                setPluginInfo(data.infoPlugin);
                setCerts(data.certs);
                setSignList(files.map((value) => {
                    return { isSing: 0, Text: "", FILENAME: value.FILENAME, DOC_FOR_SIGN_ID: value.DOC_FOR_SIGN_ID }
                }));
            } catch (err) {
                alert(err.message);
                handleClose();
            } finally {
                setCheckingPlugin(false);
            }
        }
    }, [isOpen]);

    const downloadFile = async (docForSignId) => {
        const result = { Valid: false, Error: "", File: null };
        try {
            const requestOptions = {
                method: "GET",
                cache: 'no-cache',
                headers: { 'Content-Type': "application/json" }
            };
          
            const response = await window.fetch(`DownloadFileForSign?DOC_FOR_SIGN_ID=${docForSignId}`, requestOptions);
            const data = await response.json();
            if (data.Result === false) {
                result.Error = data.Value;
            } else {
                result.Valid = true;
                result.File = data.Value;
            }
            return result;

        } catch (error) {
            result.Error = error;
            return result;
        }
    }


    const sendSignFile = async (sign, docForSignId) => {
        const result = { Valid: false, Error: "" };
        try {
           
            const formData = new FormData();
            formData.append("SIGN", sign);
            formData.append("DOC_FOR_SIGN_ID", docForSignId);
            
            const requestOptions = {
                method: "POST",
                body: formData
            };

            const response = await window.fetch(`SendSign`, requestOptions);
            const data = await response.json();

           
            if (data.Result === false) {
                result.Error = data.Value;
            } else {
                result.Valid = true;
            }
            return result;
        } catch (error) {
            result.Error = error;
            return result;
        }
    }

    const updateItemState = async (signItem, isSing, text) => {
        setSignList(signList.map((value) => {
            if (value === signItem) {
                value.isSing = isSing;
                value.Text = text;
            }
            return value;
        }));
    }
    const [singErr, setSignErr] = React.useState("");
    const signFiles = async () => {
        let countOk = 0;
        try {
            setSignProcess(true);
            for (const signItem of signList) {
                const docForSignId = signItem.DOC_FOR_SIGN_ID;
                updateItemState(signItem, 1, "Загрузка файла с сервера");
                const file = await downloadFile(docForSignId);
                if (file.Valid) {
                    updateItemState(signItem, 1, "Подпись файла");
                    const sign = await SignFile(file.File.FileContents, currentCert.SerialNumber);
                    if (sign.Result) {
                        updateItemState(signItem, 1, "Отправка подписи");
                        const res = await sendSignFile(sign.SIGN, docForSignId);
                        if (res.Valid) {
                            updateItemState(signItem, 2, "Файл подписан");
                            countOk++;
                        } else {
                            updateItemState(signItem, 3, `Ошибка сервера: ${res.Error}`);
                        }
                    } else {
                        updateItemState(signItem, 3, `Ошибка клиента: ${sign.Error}`);
                    }
                } else {
                    updateItemState(signItem, 3, `Ошибка клиента: ${file.Error}`);
                }
            }
         
        }
        catch (err) {
            alert();
        }
        finally {
            setSignProcess(false);
            if (countOk === signList.length) {
                onSave();
            }
            else {
                setSignErr(`Не удалось подписать: ${signList.length - countOk} файлов`);
            }
        }
        
    }


    const onSelected = (index) => {
        setCurrentCert(certs[index]);
    }

    const switchClassNameStatusText = (value) => {
        switch (value) {
        case 2:
                return "GreenText";
        case 3:
                return "RedText";
            default:
                return "";
        }
    }


    const switchIconStatus = (value) => {
        switch (value) {
        case 1:
                return <CircularProgress size="25px" />;
        case 2:
                return <img width="25px" src="../Image/IconOK.png"/>;
        case 3:
                return <img width="25px" src="../Image/IconERROR.png"/>;
        default:
            return "";
        }
    }
    return (
        <div >
            <Dialog open={isOpen} onClose={handleClose}  aria-labelledby="form-dialog-title" fullWidth={true} maxWidth={"lg"}>
                <DialogTitle id="form-dialog-title">Подписать файл</DialogTitle>
                <DialogContent>
                    <div> {signList != null ?
                        <table className="table_report">
                            <tbody>
                            <tr><th>Имя файла</th><th>Статус</th><th>Действие</th></tr>
                            {signList.map((value, index) =>
                                    <tr key={index}>
                                        <td>{value.FILENAME}</td>
                                        <td>{switchIconStatus(value.isSing)}</td>
                                        <td className={switchClassNameStatusText(value.isSing)}>{value.Text}</td></tr>)}
                            </tbody>
                        </table>
                        : null}
                    </div>
                    {checkingPlugin ?
                        <div>
                            <div><CircularProgress size="25px"/></div>
                            <div>Опрос плагина</div>
                        </div>
                        : null
                    }
                    <div>
                        {pluginInfo != null ? <PluginInfo pluginInfo={pluginInfo}/> : null}
                    </div>
                    <div>
                        {certs != null ? <CertList items={certs} onSelected={onSelected}/>: null}
                    </div>
                    <div className="RedText">{singErr}</div>
                    <br/>
                    <Button variant="contained" color="primary" onClick={signFiles} disabled={signProcess}>Подписать</Button>
                </DialogContent>
            </Dialog>
        </div>
    );
}


function PluginInfo(props) {
    const { pluginInfo } = props;
    return (
        <div>
            {pluginInfo.isEnabled ? <div className="GreenText">Плагин доступен</div> : <div className="RedText">Плагин не доступен</div>}
            <div>Версия плагина: {pluginInfo.version}</div>
            <div>Криптопровайдер: {pluginInfo.cspName}</div>
            <div>Версия криптопровайдера: {pluginInfo.versionCSP}</div>
        </div>
    );
}

const ListItemStyles = makeStyles((theme) => ({
    red: {
        color: 'red !important'
    },
    green: {
        color: 'green !important'
    }
}));


 function CertList(props) {
    const { items, onSelected } = props;

    const classes = ListItemStyles();

     const [selectedIndex, setselectedIndex] = React.useState(0);
     React.useEffect(() => {
         if (items.length != 0) {
             handleListItemClick(null, 0);
         }
     }, [items] )
    const handleListItemClick = (event, index) => {
        setselectedIndex(index);
        onSelected(index);
    };
    return (
        <div>
            <Grid container spacing={1}>
                <Grid item={true} xs>
                    <div className="BoldText">Список сертификатов</div>
                    <List aria-label="secondary mailbox folder">
                        {items.map((value, index) => <ListItem key={index} className={value.isSupportAlg ? classes.green : classes.red} selected={selectedIndex === index} button onClick={(event) => handleListItemClick(event, index)}><ListItemText primary={`Сертификат:${value.CertName}`} /></ListItem>)}
                    </List>
                </Grid>
                <Grid item={true} xs>
                    <InfoCert cert={items[selectedIndex]} />
                </Grid>
            </Grid>
        </div>
    );
}

function InfoCert(props) {
    const { cert } = props;
    return (
        cert != null ?
            <div>
                <div className="BoldText">Информация о сертификате</div>
                {!cert.isSupportAlg?<div className="RedText">Не поддерживаемый алгоритм ключа</div>:null} 
                <table className="table_report">
                    <tbody>
                    <tr><th>Параметр</th><th>Значение</th></tr>
                    {cert.CertName != null ? <tr><td>Владелец</td><td>{cert.CertName}</td></tr> : null}
                    {cert.FIO != null ? <tr><td>ФИО владельца</td><td>{cert.FIO}</td></tr> : null}
                    {cert.ValidFromDate != null ? <tr><td>Выдан</td><td>{format(cert.ValidFromDate, "dd-MM-yyyy")}</td></tr> : null}
                    {cert.ValidToDate != null ? <tr><td>Действителен до:</td><td>{format(cert.ValidToDate, "dd-MM-yyyy")}</td></tr> : null}
                    {cert.ProviderName != null ? <tr><td>Криптопровайдер</td><td>{cert.ProviderName}</td></tr> : null}
                    {cert.FriendlyName != null ? <tr><td>Алгоритм ключа</td><td>{cert.FriendlyName}</td></tr> : null}
                    </tbody>
                </table>
            </div>
            : null
    );
}
