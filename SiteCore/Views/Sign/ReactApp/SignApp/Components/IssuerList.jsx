import Button from "@material-ui/core/Button";
import Dialog from "@material-ui/core/Dialog";

import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import TextField from "@material-ui/core/TextField";
import MaterialTable from "material-table";
import React,{ Component, useEffect, useRef} from "react";
import ReactDOM from "react-dom"

import { makeStyles, createMuiTheme, MuiThemeProvider } from '@material-ui/core/styles';
import DateFnsUtils from '@date-io/date-fns'; // choose your lib
import ruLocale from "date-fns/locale/ru";
import { KeyboardDatePicker, MuiPickersUtilsProvider, } from '@material-ui/pickers';




import Typography from '@material-ui/core/Typography';


import Stepper from '@material-ui/core/Stepper';
import Step from '@material-ui/core/Step';
import StepLabel from '@material-ui/core/StepLabel';
import StepContent from '@material-ui/core/StepContent';

import CertList from "./CertList.jsx"
import ShowCertDialog from "./ShowCertDialog.jsx"

export default function IssuerList() {
    const tableRef = React.useRef(null);
    const [isOpenRoleDialog, setisOpenRoleDialog] = React.useState(false);
    const [issuerList, setIssuerList] = React.useState([]);


    const getIssuerList = async () => {
        const response = await window.fetch("GetISSUER");
        const result = await response.json();
        setIssuerList(result.Value);
    }

    React.useEffect(() => {
        getIssuerList();
    }, []);
    const refresh = () => { getIssuerList(); }
  
    const closeIssuerDialog = () => {
        setisOpenRoleDialog(false);
        refresh();
    };

    const saveIssuerDialog = () => {
        closeIssuerDialog();
        refresh();
    };

    const removeIssuer = (event, rowData) => {
        try {
            if (confirm(`Вы уверены что хотите удалить издателя: ${rowData.CAPTION}?`)) {
                const requestOptions = {
                    method: "POST",
                    headers: { 'Content-Type': "application/json" },
                    body: JSON.stringify(rowData.SING_ISSUER_ID)
                };
                window.fetch("RemoveISSUER", requestOptions)
                    .then(response => response.json())
                    .then(data => {
                        if (data) {
                            if (data.Result === false) {
                                alert(data.Value);
                            } else {
                                refresh();
                            }
                        }
                    })
                    .catch(error => {
                        alert(error.toString());
                    });
            }
        } catch (error) {

            alert(error.toString());
        }
    }
    const [isOpenShowSignDialog, setisOpenShowSignDialog] = React.useState(false);
    const [showCertInfo, setshowCertInfo] = React.useState(null);

    const closeShowDialog = () => {
        setisOpenShowSignDialog(false);
        setshowCertInfo(null);
    };

    const showDetail = (event, rowData) => {
        try {
            const requestOptions = {
                method: "GET",
                headers: { 'Content-Type': "application/json" }
            };
            window.fetch(`GetISSUERCertInfo?ID=${rowData.SING_ISSUER_ID}`, requestOptions)
                .then(response => response.json())
                .then(data => {
                    if (data) {
                        if (data.Result === false) {
                            alert(data.Value);
                        } else {
                            setshowCertInfo(data.Value);
                            setisOpenShowSignDialog(true);
                        }
                    }
                })
                .catch(error => {
                    alert(error.toString());
                });
        } catch (error) {

            alert(error.toString());
        }
    }


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



    return (
        <div style={{ maxWidth: "100%" }}>
            <MuiThemeProvider theme={theme}>
            <MaterialTable tableRef={tableRef}
                columns={[
                    { title: "ID", field: "SING_ISSUER_ID" },
                    { title: "Наименование издателя", field: "CAPTION" },
                    { title: "Дата начала", field: "DATE_B", type: "date" },
                    { title: "Дата окончания", field: "DATE_E", type: "date" }
                ]}
                data={issuerList}
                actions={[
                    {
                        icon: "refresh",
                        tooltip: "Обновить",
                        iconProps: { color: "primary" },
                        isFreeAction: true,
                        onClick: () => refresh()
                    },
                    {
                        icon: "add",
                        tooltip: "Добавить издателя",
                        iconProps: { color: "primary" },
                        isFreeAction: true,
                        onClick: (event) => {
                            setisOpenRoleDialog(true);
                        }
                    },
                    {
                        icon: "search",
                        tooltip: "Посмотреть",
                        iconProps: { color: "primary" },
                        onClick: showDetail
                    },
                    {
                        icon: "delete",
                        iconProps: { color: "error" },
                        tooltip: "Удалить издателя",
                        onClick: removeIssuer
                    }
                ]}
                title="Список доверенных издателей"
                options={{
                    paging: false,
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
                        emptyDataSourceMessage:"Нет записей"
                    }
                }}
                />
            </MuiThemeProvider>
            <AddISSUERDialog isOpen={isOpenRoleDialog} onClose={closeIssuerDialog} onSave={saveIssuerDialog} />
            <ShowCertDialog isOpen={isOpenShowSignDialog} onClose={closeShowDialog} certInfo={showCertInfo} />
        </div>
    );

}



const useStyles = makeStyles((theme) => ({
    button: {
        marginRight: theme.spacing(1),
    }
}));

export function AddISSUERDialog(props) {
    const classes = useStyles();
    const { onClose, isOpen, onSave } = props;
    const [errorMessage, setErrorMessage] = React.useState("");
    const [file, setFile] = React.useState(null);

   

    const [certInfo, setcertInfo] = React.useState(null);
    const [processReadCertificate, setProcessReadCertificate] = React.useState(false);
    const selectFile = (e) => {
        var file = e.target.files[0];
        setFile(file);
    };
    const readCertificate = async () => {
        try {
            setProcessReadCertificate(true);
            const formData = new FormData();
            formData.append("file", file, file.name);
            formData.append("isIssuer", true);

            const requestOptions = {
                method: "POST",
                body: formData
            };
            const response = await window.fetch("GetCertificateINFO", requestOptions)
            const data = await response.json();
            if (data) {
                if (data.Result === false) {
                    setErrorMessage(data.Value);
                } else {
                    setcertInfo(data.Value);

                }
            }
        } catch (e) {
            setErrorMessage(error.toString());
        } finally {
            setProcessReadCertificate(false);
        }
    };
    const [activeStep, setActiveStep] = React.useState(0);
    const handleNext = () => {
        setActiveStep((prevActiveStep) => prevActiveStep + 1);
    };
    const handleReset = () => {
        setActiveStep(0);
        setcertInfo(null);
        setFile(null);
    };
    const handleSave = () => {
        handleReset();
        onSave();
    };
    const handleClose = () => {
        handleReset();
         onClose();
    };

    return (
        <div>
            <div>{isOpen}</div>
            <Dialog open={isOpen} onClose={handleClose} aria-labelledby="form-dialog-title" fullWidth={true} maxWidth={"lg"}>
                <DialogTitle id="form-dialog-title">Добавление издателя</DialogTitle>
                <DialogContent>
                    <DialogContentText>
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
                                        {certInfo != null ? <CertList items={certInfo.Data} /> : <div></div>}
                                        <br />
                                        <div>
                                            <div>
                                                <Button className={classes.button} disabled={activeStep === 0} onClick={handleReset}>Назад</Button>
                                                <Button className={classes.button} variant="contained" color="primary" onClick={readCertificate} disabled={processReadCertificate}>Проверить сертификат</Button>
                                                <Button className={classes.button} variant="contained" color="primary" onClick={handleNext} disabled={certInfo != null ? certInfo.Valid === false : true}>Далее</Button>
                                            </div>
                                        </div>
                                    </StepContent>
                                </Step>
                                <Step key={2}>
                                    <StepLabel>Сохранение сертификата</StepLabel>
                                    <StepContent>
                                        <AddISSUER file={file} onSave={handleSave} CAPTION={certInfo!=null ? certInfo.Data[0].CAPTION: null} />
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

function AddISSUER(props) {
    const { file, onSave } = props;
   
    const [certData, setCertData] = React.useState({ DATE_B: new Date(), DATE_E: null, CAPTION:props.CAPTION });
    const [validationErr, setValidationErr] = React.useState([]);

    const validate = () => {
        const err = [];
        if (file === null)
            err.push('Укажите файл для загрузки');
        if (certData.DATE_B === null)
            err.push('Укажите дату начала');
        if (certData.DATE_B === null)
            err.push('Укажите дату начала');
        if (certData.CAPTION === null || certData.CAPTION === "")
            err.push('Укажите наименование');
        if (certData.DATE_B > certData.DATE_E && certData.DATE_E != null)
            err.push('Дата начала больше даты окончания');
        setValidationErr(err);
    }
    useEffect(() => { validate(); }, [certData, file]);

    const dateBChange = (date) => {
        setCertData(certData => ({ ...certData, DATE_B: date }));
    }
    const dateEChange = (date) => {
        setCertData(certData => ({ ...certData, DATE_E: date }));
    }
    const captionChange = (event, newValue) => {
        setCertData(certData => ({ ...certData, CAPTION: newValue }));

    }
    const [processSaveIssuer, setProcessSaveIssuer] = React.useState(false);
    const saveIssuer = async () => {
        try {
            setProcessSaveIssuer(true);
            const formData = new FormData();
            formData.append("File", file, file.name);
            formData.append("CAPTION", certData.CAPTION);
            formData.append("DATE_B", certData.DATE_B.toLocaleDateString());
            if (certData.DATE_E != null)
                formData.append("DATE_E", certData.DATE_E.toLocaleDateString());


            const requestOptions = {
                method: "POST",
                body: formData
            };

            const response = await window.fetch("AddISSUER", requestOptions);
            const result = await response.json();
            if (result.Result === true)
                onSave();
            else
                setValidationErr(result.Value);
        } catch (err) {
            setValidationErr(err.toString());
        } finally {
            setProcessSaveIssuer(false);
        }
    }

    return (
        <div>
            <div>
                {validationErr != null ? <ul className="ErrorLi">{Array.isArray(validationErr) ? validationErr.map((value) => <li>{value}</li>) : validationErr}</ul> : null}
            </div>
            <div> <TextField autoFocus margin="dense" label="Наименование" type="text" fullWidth value={certData.CAPTION} onChange={captionChange}></TextField></div><br />
            <MuiPickersUtilsProvider utils={DateFnsUtils} locale={ruLocale}>
                <KeyboardDatePicker autoOk variant="inline" inputVariant="outlined" label="Дата начала" format="dd.MM.yyyy" value={certData.DATE_B} InputAdornmentProps={{ position: "start" }} onChange={date => dateBChange(date)} />
                <KeyboardDatePicker autoOk variant="inline" inputVariant="outlined" label="Дата окончания" format="dd.MM.yyyy" value={certData.DATE_E} InputAdornmentProps={{ position: "start" }} onChange={date => dateEChange(date)} />
            </MuiPickersUtilsProvider>
            <br />
            <br />
            <Button variant="contained" color="primary" onClick={saveIssuer} disabled={validationErr == null || validationErr.length !== 0 || processSaveIssuer}>Сохранить</Button>
        </div>
    );
}
