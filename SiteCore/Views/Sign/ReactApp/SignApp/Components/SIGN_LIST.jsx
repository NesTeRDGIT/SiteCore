import React, { Component, useRef, useEffect } from "react"


import MaterialTable from "material-table"
import Stepper from '@material-ui/core/Stepper';
import Step from '@material-ui/core/Step';
import StepLabel from '@material-ui/core/StepLabel';
import StepContent from '@material-ui/core/StepContent';

import DateFnsUtils from '@date-io/date-fns'; // choose your lib
import ruLocale from "date-fns/locale/ru";
import { KeyboardDatePicker, MuiPickersUtilsProvider, } from '@material-ui/pickers';

import "core-js/stable";
import "regenerator-runtime/runtime";

import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import { makeStyles, createMuiTheme, MuiThemeProvider } from '@material-ui/core/styles';

import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import Typography from '@material-ui/core/Typography';
import Autocomplete from '@material-ui/lab/Autocomplete';
import CertList from "./CertList.jsx";
import ShowCertDialog from "./ShowCertDialog.jsx";
import { saveFile, downloadBase64File } from "../API/FileAPI.js";


const theme = createMuiTheme({
    palette: {
        primary: {
            main: '#4CAF50'
        },
        secondary: {
            main: '#4CAF50'
        },
        error: {
            main: '#c01304'
        }
    },
});




export default  function SIGN_LIST() {
    const tableRef = React.useRef(null);
    const [signList, setSignList] = React.useState([]);

    const getSignList = async () => {
        try {
            const response = await window.fetch("GetSignList", { credentials: "same-origin"});
            const result = await response.json();
            setSignList(result.Value);
        } catch (err) {
            alert(err.toString());
        }
    }

    React.useEffect(() => {
        getSignList();
    }, []);

    function refresh() { getSignList(); }

    const [isOpenSignDialog, setIsOpenSignDialog] = React.useState(false);
    const closeSignDialog = () => {
        setIsOpenSignDialog(false);
    };
    const saveSignDialog = () => {
      
        closeSignDialog();
        refresh();
    };


    const [isOpenShowSignDialog, setIsOpenShowSignDialog] = React.useState(false);
    const [showCertInfo, setshowCertInfo] = React.useState(null);

    const closeShowDialog = () => {
        setIsOpenShowSignDialog(false);
        setshowCertInfo(null);
    };


    const showDetail = async (event, rowData) => {
        try {
            const requestOptions = {
                method: "GET",
                headers: { 'Content-Type': "application/json" }
            };
            const response = await window.fetch(`GetSignCertInfo?ID=${rowData.ID}`, requestOptions);
            const data = await response.json();
            if (data.Result === false) {
                alert(data.Value);
            } else {
                setshowCertInfo(data.Value);
                setIsOpenShowSignDialog(true);
            }
        } catch (error) {

            alert(error.toString());
        }
    }
    const removeSign = async (event, rowData) => {
        try {
            if (confirm(`Вы уверены что хотите удалить подпись: ${rowData.ID}?`)) {
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': "application/json" },
                    body: JSON.stringify(rowData.ID)
                };
                const response = await window.fetch("RemoveSign", requestOptions);
                const data = await response.json();
                if (data.Result === false) {
                    alert(data.Value);
                } else {
                    refresh();
                }
            }
        } catch (error) {
            alert(error.toString());
        }
    }

    const downloadFile = async (event, rowData) => {
        const requestOptions = {
            method: "GET",
            headers: { 'Content-Type': "application/json" }
        };
        
        const response = await window.fetch(`DownloadCert?id=${rowData.ID}`, requestOptions);
        const data = await response.json();
        if (data.Result === true) {
            downloadBase64File(data.Value.FileContents, data.Value.ContentType, data.Value.FileDownloadName);
        } else {
            alert(data.Value);
        }
    }
  

    return (
        <div style={{ maxWidth: "100%" }}>
            <MuiThemeProvider theme={theme}>
            <MaterialTable tableRef={tableRef}
                columns={[
                    { title: "Код МО", field: "CODE_MO" },
                    { title: "Наименование", field: "MO_NAME" },
                    { title: "Роль", field: "ROLE" },
                    { title: "Дата начала", field: "DATE_B", type: "date" },
                    { title: "Дата окончания", field: "DATE_E", type: "date" }
                ]}
                data={signList}
                actions={[
                    {
                        icon: "refresh",
                        iconProps: { color: "primary" },
                        tooltip: "Обновить",
                        isFreeAction: true,
                        onClick: () => refresh()
                    },
                    {
                        icon: "add",
                        iconProps: { color: "primary" },
                        tooltip: "Добавить подпись",
                        isFreeAction: true,
                        onClick: (event) => { setIsOpenSignDialog(true); }
                    },
                    {
                        icon: "download",
                        iconProps: { color: "primary" },
                        tooltip: "Скачать файлы",
                        onClick: downloadFile
                    },
                    {
                        icon: "search",
                        iconProps: { color: "primary" },
                        tooltip: "Посмотреть",
                        onClick: showDetail
                    },
                    {
                        icon: "delete",
                        iconProps: { color: "error" },
                        tooltip: "Удалить подпись",
                        onClick: removeSign
                    }
                ]}
                options={{
                    paging: false,
                    grouping: true,
                    actionsColumnIndex: -1,
                    search: false
                }}
                localization={{
                    toolbar: {
                        searchPlaceholder: "Найти"
                    },
                    header: {
                        actions: "Действия"
                    },
                    body: {
                        emptyDataSourceMessage: "Нет записей"
                    },
                    grouping:
                    {
                        placeholder: "Перетащите заголовок для группировки",
                        groupedBy: "Группировка по: "
                    }
                }}
                title="Подписи"
                />
            </MuiThemeProvider>
            <AddSingDialog isOpen={isOpenSignDialog} onClose={closeSignDialog} onSave={saveSignDialog} />
            <ShowCertDialog isOpen={isOpenShowSignDialog} onClose={closeShowDialog} certInfo={showCertInfo} />
            
        </div>
    );
}


const useStyles = makeStyles((theme) => ({
    button: {
        marginRight: theme.spacing(1),
    }
}));

export function AddSingDialog(props) {
    const classes = useStyles();
    const { onClose, isOpen, onSave } = props;
    const [errorMessage, setErrorMessage] = React.useState("");
    const [file, setFile] = React.useState(null);
  
    

    const [certInfo, setCertInfo] = React.useState(null);
   
    const selectFile = (e) => {
        var file = e.target.files[0];
        setFile(file);
    };
    const [processReadCertificate, setProcessReadCertificate] = React.useState(false);
    const readCertificate = async () => {
        try {
            setProcessReadCertificate(true);
            const formData = new FormData();
            formData.append("file", file, file.name);
            const requestOptions = {
                method: "POST",
                credentials: "same-origin",
                body: formData
            };

            const response = await window.fetch("GetCertificateINFO", requestOptions);
            const data = await response.json();
            if (data.Result === false) {
                setErrorMessage(data.Value);
            } else {
                setCertInfo(data.Value);
            }
        } catch (error) {
            setErrorMessage(error.toString());
        }
        finally {
            setProcessReadCertificate(false);
        }
    };
    const [activeStep, setActiveStep] = React.useState(0);
    const handleNext = () => {
        setActiveStep((prevActiveStep) => prevActiveStep + 1);
    };
    const handleReset = () => {
        setCertInfo(null);
        setFile(null);
        setActiveStep(0);
    };
    const handleSave = () => {
        onSave();
        handleReset();
    };

    const handleClose = () => {
        onClose();
        handleReset();
    };




   
    
    return (
        <div>
            <Dialog open={isOpen} onClose={handleClose} aria-labelledby="form-dialog-title" fullWidth={true} maxWidth={"lg"}>
                <DialogTitle id="form-dialog-title">Добавление подписи</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        <div>Введите наименование роли и нажмите на кнопку "Сохранить"</div>
                        <div className="RedText">{errorMessage}</div>
                    </DialogContentText>
                    <div>
                        <div>
                            <Stepper activeStep={activeStep} orientation="vertical">
                                <Step key={0}>
                                    <StepLabel>Загрузка сертификата</StepLabel>
                                    <StepContent>
                                        <div>
                                            <Typography>Выберите файл формата .cer</Typography>
                                            <input type="file" accept=".cer" onChange={selectFile} />
                                        </div>
                                        <br />
                                        <div>
                                            <Button className={classes.button} variant="contained" color="primary" onClick={handleNext} disabled={file == null}>Далее</Button>
                                        </div>
                                    </StepContent>
                                </Step>
                                <Step key={1}>
                                    <StepLabel>Просмотр данных сертификата</StepLabel>
                                    <StepContent>
                                        {certInfo != null ? <CertList items={certInfo.Data}  /> : <div></div>}
                                        <br />
                                        <div>
                                            <div>
                                                <Button className={classes.button} disabled={activeStep === 0} onClick={handleReset}>Назад</Button>
                                                <Button className={classes.button} variant="contained" color="primary" onClick={readCertificate}>Проверить сертификат</Button>
                                                <Button className={classes.button} variant="contained" color="primary" onClick={handleNext} disabled={(certInfo != null ? certInfo.Valid === false : true) || processReadCertificate}  >Далее</Button>
                                            </div>
                                        </div>
                                    </StepContent>
                                </Step>
                                <Step key={2}>
                                    <StepLabel>Сохранение сертификата</StepLabel>
                                    <StepContent>
                                        <AddCert file={file} onSave={handleSave} dateB={certInfo != null && certInfo.Data.length !== 0 ? certInfo.Data[0].DATE_B : null} dateE={certInfo != null && certInfo.Data.length !== 0 ? certInfo.Data[0].DATE_E : null}/>
                                        <br />
                                        <div>
                                            <div>
                                                <Button className={classes.button} disabled={activeStep === 0} onClick={handleReset}>Назад</Button>
                                            </div>
                                        </div>
                                    </StepContent>
                                </Step>
                            </Stepper>
                        </div>
                    </div>
                </DialogContent>
            </Dialog>
        </div>
    );
}

function AddCert(props) {
    const { file, onSave, dateB, dateE } = props;
    const [roles, setRoles] = React.useState(null);
    const [lpu, setLpu] = React.useState(null);
    const [certData, setCertData] = React.useState({ DATE_B: new Date(), DATE_E: null, MO: null, ROLE: null,fileConfirm:null });
    const [validationErr, setValidationErr] = React.useState([]);

    const validate = () => {
        const err = [];
        if (file === null)
            err.push('Укажите файл для загрузки');
        if (certData.MO === null)
            err.push('Укажите медицинскую организацию');
        if (certData.ROLE === null)
            err.push('Укажите роль');
        if (certData.DATE_B === null)
            err.push('Укажите дата начала');
        if (certData.DATE_B > certData.DATE_E && certData.DATE_E != null)
            err.push('Дата начала больше даты окончания');
        if (certData.fileConfirm === null)
            err.push('Укажите документ на основании которого вносится подпись');
        setValidationErr(err);
    }

    useEffect(() => {
        window.fetch("GetRole")
            .then(response => response.json())
            .then(result => {
                if (result.Result === true)
                    setRoles(result.Value);
                else
                    alert(`Ошибка получения справочника ролей:${result.Value}`);
            });
        window.fetch("GetF003")
            .then(response => response.json())
            .then(result => {
                if (result.Result === true)
                    setLpu(result.Value);
                else
                    alert(`Ошибка получения справочника МО:${result.Value}`);
            });
        setCertData(certData => ({ ...certData, DATE_B: dateB, DATE_E: dateE }));

    }, []);

    useEffect(() => { validate(); }, [certData, file]);

    const dateBChange = (date) => {
        setCertData(certData => ({ ...certData, DATE_B: date }));
    }
    const dateEChange = (date) => {
        setCertData(certData => ({ ...certData, DATE_E: date }));
    }
    const codeMoChange = (event, newValue) => {
        setCertData(certData => ({ ...certData, MO: newValue }));

    }
    const roleIdChange = (event, newValue) => {
        setCertData(certData => ({ ...certData, ROLE: newValue }));
    }
    const fileConfirmChange = (event, newValue) => {
        var file = event.target.files[0];
        setCertData(certData => ({ ...certData, fileConfirm: file }));
    }

    const [processSaveCert, setProcessSaveCert] = React.useState(false);
    const saveCert = async (event, newValue) => {
        try {
            setProcessSaveCert(true);
            const formData = new FormData();
            formData.append("File", file, file.name);
            formData.append("FileConfirm", certData.fileConfirm, certData.fileConfirm.name);
            formData.append("ROLE_ID", certData.ROLE.SIGN_ROLE_ID);
            formData.append("CODE_MO", certData.MO.MCOD);
            formData.append("DATE_B", new Date(certData.DATE_B).toLocaleDateString());
            if (certData.DATE_E != null)
                formData.append("DATE_E", new Date(certData.DATE_E).toLocaleDateString());


            const requestOptions = {
                method: "POST",
                credentials: "same-origin",
                body: formData
            };

            const response = await window.fetch("AddCert", requestOptions);
            const result = await response.json();
            if (result.Result === true)
                onSave();
            else
                setValidationErr(result.Value);
        } catch (err) {
            setValidationErr(err);
        } finally {
            setProcessSaveCert(false);
        }
    }

    return (
        <div>
            <div>
                {validationErr != null ? (Array.isArray(validationErr) ? <ul className="ErrorLi">{validationErr.map((value) => <li>{value}</li>)}</ul>: validationErr) : null}
            </div>
            <div><Autocomplete options={roles} getOptionLabel={(option) => option.CAPTION} style={{ width: 400 }} value={certData.ROLE} onChange={roleIdChange} renderInput={(params) => <TextField {...params} label="Роль" variant="outlined" />} /></div><br />
            <div><Autocomplete options={lpu} getOptionLabel={(option) => option.NAME} style={{ width: 400 }} value={certData.MO} onChange={codeMoChange} renderInput={(params) => <TextField {...params} label="Медицинская организация" variant="outlined" />} /></div><br />
            <div>
                <Typography>Документ на основании которого вносится подпись</Typography>
                <input type="file" onChange={fileConfirmChange} />
            </div>
            <br/>
            <MuiPickersUtilsProvider utils={DateFnsUtils} locale={ruLocale}>
                <KeyboardDatePicker autoOk variant="inline" inputVariant="outlined" label="Дата начала" format="dd.MM.yyyy" value={certData.DATE_B} InputAdornmentProps={{ position: "start" }} onChange={date => dateBChange(date)} />
                <KeyboardDatePicker autoOk variant="inline" inputVariant="outlined" label="Дата окончания" format="dd.MM.yyyy" value={certData.DATE_E} InputAdornmentProps={{ position: "start" }} onChange={date => dateEChange(date)} />
            </MuiPickersUtilsProvider>
            <br />
            <br />
            <Button variant="contained" color="primary" onClick={saveCert} disabled={validationErr == null || validationErr.length !== 0 || processSaveCert}>Сохранить</Button>
        </div>
    );
}

