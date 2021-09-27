import React, { Component, useRef, useEffect } from "react"
import ReactDOM from "react-dom"

import MaterialTable from "material-table"
import Stepper from '@material-ui/core/Stepper';
import Step from '@material-ui/core/Step';
import StepLabel from '@material-ui/core/StepLabel';
import StepContent from '@material-ui/core/StepContent';
import { makeStyles, createMuiTheme, MuiThemeProvider } from '@material-ui/core/styles';
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

import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import Typography from '@material-ui/core/Typography';
import Autocomplete from '@material-ui/lab/Autocomplete';


import SingDialog from "./SignDialog.jsx"


import Box from '@material-ui/core/Box';
import LinearProgress from '@material-ui/core/LinearProgress';

import { downloadBase64File } from "../API/FileAPI.js";

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

export default function DOC_LIST(props) {
    const { isAdmin } = props;
    const tableRef = React.useRef(null);


    const [docsList, setDocsList] = React.useState([]);


    const getDocsList = async () => {
        try {
            const response = await window.fetch("GetDOC", { credentials: "same-origin"});
            const result = await response.json();
            setDocsList(result.Value);
        } catch (err) {
            alert(err.toString());
        }
    }
    React.useEffect(() => {
      
        getDocsList();

    }, []);
    function refresh() { getDocsList(); }

    const [isOpenAddDocsDialog, setIsOpenAddDocsDialog] = React.useState(false);
    const closeAddDocsDialog = () => {
        setIsOpenAddDocsDialog(false);
    };
    const saveDocsDialog = () => {
        closeAddDocsDialog();
        refresh();
    };

    const [isOpenSignDialog, setIsOpenSignDialog] = React.useState(false);

    const closeSignDialog = () => {
        setIsOpenSignDialog(false);
    };
    const saveSignDialog = () => {
        setIsOpenSignDialog(false);
        refresh();
    };

    const [selectedFiles, setSelectedFiles] = React.useState(null);
   
    const signDocs = (event, rowsData) => {
        setSelectedFiles(rowsData);
        setIsOpenSignDialog(true);
    }

   
    const downloadDoc = (event, rowData) => {
        try {
           
            const requestOptions = {
                method: "GET",
                headers: { 'Content-Type': "application/json" },
                credentials: "same-origin"
            };
            
            window.fetch(`DownloadFileForSign?DOC_FOR_SIGN_ID=${rowData.DOC_FOR_SIGN_ID}`, requestOptions)
                .then(response => response.json())
                .then(data => {
                    if (data) {
                      
                        if (data.Result === false) {
                            alert(data.Value);
                        } else {
                            downloadBase64File(data.Value.FileContents, data.Value.ContentType, data.Value.FileDownloadName);
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
    const downloadDocAndSign = (event, rowData) => {
        try {

            const requestOptions = {
                method: "GET",
                headers: { 'Content-Type': "application/json" },
                credentials: "same-origin"
            };

            window.fetch(`DownloadFileForSignAndSign?DOC_FOR_SIGN_ID=${rowData.DOC_FOR_SIGN_ID}`, requestOptions)
                .then(response => response.json())
                .then(data => {
                    if (data) {

                        if (data.Result === false) {
                            alert(data.Value);
                        } else {
                            downloadBase64File(data.Value.FileContents, data.Value.ContentType, data.Value.FileDownloadName);
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

    const removeDoc = async (docForSignId) => {
        const requestOptions = {
            method: "POST",
            headers: { 'Content-Type': "application/json" },
            credentials: "same-origin",
            body: JSON.stringify(docForSignId)
        };
        const response = await window.fetch("RemoveFileForSign", requestOptions);
        if (response.ok) {
            const data = await response.json();
            if (data.Result === false) {
                alert(data.Value);
            } 
        } else {
            throw new Error(`${response.status}-${response.statusText}`);
        }
    }
    const removeDocs = async (event, rowsData) => {
        try {
            if (confirm(`Вы уверены что хотите удалить ${rowsData.length} документ?`)) {
                for (const item of rowsData) {
                    await removeDoc(item.DOC_FOR_SIGN_ID);
                }
                refresh();
            }
        } catch (error) {
            alert(error.toString());
        }
    }

    function tableAction() {
        const action = [
            {
                icon: "refresh",
                iconProps: { color: "primary" },
                tooltip: "Обновить",
                isFreeAction: true,
                onClick: () => refresh()
            },
            {
                icon: 'fingerprint',
                iconProps: { style: { color: "#5FE3FF" } },
                tooltip: 'Подписать',
                onClick: signDocs
            },



            {
                icon: "fingerprint",
                iconProps: { style: { color: "#5FE3FF" } },
                tooltip: "Подписать",
                position: 'row',
                onClick: (e, data) => { signDocs(e, [data]) }
            },
            {
                icon: "uploadfile",
                iconProps: { style: { color: "#5FE3FF" } },
                tooltip: "Загрузить открепленную подпись",
                position: 'row'

            },
            {
                icon: "download",
                iconProps: { color: "primary" },
                tooltip: "Скачать файл",
                position: 'row',
                onClick: downloadDoc
            },
            {
                icon: "archive",
                iconProps: { color: "primary" },
                tooltip: "Скачать файл с подписями",
                position: 'row',
                onClick: downloadDocAndSign
            }

        ];
        if (isAdmin) {
            action.push(
                {
                    icon: "add",
                    iconProps: { color: "primary" },
                    tooltip: "Добавить документы",
                    isFreeAction: true,
                    onClick: (event) => { setIsOpenAddDocsDialog(true); }
                },
                {
                    icon: "delete",
                    iconProps: { color: "error" },
                    tooltip: "Удалить документ",
                    position: 'row',
                    onClick: (e, data) => { removeDocs(e, [data]) }
                },
                {
                    icon: "delete",
                    iconProps: { color: "error" },
                    tooltip: "Удалить документы",
                    onClick: removeDocs
                });
        }
        return action;
    }



   
    return (
        <div style={{ maxWidth: "100%" }}>
            <MuiThemeProvider theme={theme}>
            <MaterialTable tableRef={tableRef}
                           columns={[
                               { title: "Медицинская организация", field: "MO_NAME" },
                               { title: "Тема", field: "Theme" },
                    { title: "Имя файла", field: "FILENAME" },
                    { title: "Дата", field: "DateCreate", type: "date" },
                               {
                                   title: "Подписано",
                                   field: "ROLE_SIGN",
                                   render: rowData =>
                                       <div>
                                           {rowData.SIGNS.map((value) => <div><span className={value.IsSIGN ? "GreenText" : "RedText"}>{value.ROLE_NAME}</span><br/></div>)}
                                       </div>
                               }]}
                    data={docsList}
                            actions={tableAction()}
                           options={{
                               paging: false,
                               grouping: true,
                               actionsColumnIndex: -1,
                               search: false,
                               selection: true
                           }}
                           localization={{
                               toolbar: {
                                   searchPlaceholder: "Найти",
                                   nRowsSelected: "{0} строк выбрано"
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
                    title="Подписи" />
            </MuiThemeProvider>
            <AddDocsDialog onClose={closeAddDocsDialog} isOpen={isOpenAddDocsDialog} onSave={saveDocsDialog}/>
            <SingDialog onClose={closeSignDialog} onSave={saveSignDialog} isOpen={isOpenSignDialog} files={selectedFiles}/>
        </div>
    );
}


const useStyles = makeStyles((theme) => ({
    button: {
        marginRight: theme.spacing(1),
    }
}));

export function AddDocsDialog(props) {
    const classes = useStyles();
    const { onClose, isOpen, onSave } = props;
    const [errorMessage, setErrorMessage] = React.useState("");
    const [files, setFiles] = React.useState(null);
  
    

   
    const selectFile = (e) => {
        var files = e.target.files;
        setFiles(Array.from(files));
    };
   
    const [activeStep, setActiveStep] = React.useState(0);
    const handleNext = () => {
        setActiveStep((prevActiveStep) => prevActiveStep + 1);
    };
    const handleReset = () => {
        setFiles(null);
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
            <Dialog open={isOpen} onClose={handleClose} aria-labelledby="form-dialog-title" fullWidth={true} maxWidth={"false"} >
                <DialogTitle id="form-dialog-title">Добавление файлов на подпись</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        <div className="RedText">{errorMessage}</div>
                    </DialogContentText>
                    <div>
                        <div>
                            <Stepper activeStep={activeStep} orientation="vertical">
                                <Step key={0}>
                                    <StepLabel>Выбор файлов</StepLabel>
                                    <StepContent>
                                        <div>
                                            <Typography>Выберите файлы</Typography>
                                            <input type="file" multiple="true" onChange={selectFile} />
                                        </div>
                                        <br />
                                        <div>
                                            <Button className={classes.button} variant="contained" color="primary" onClick={handleNext} disabled={files == null}>Далее</Button>
                                        </div>
                                    </StepContent>
                                </Step>
                                <Step key={1}>
                                    <StepLabel>Настройка и сохранение файлов</StepLabel>
                                    <StepContent>
                                        <FileViwer files={files} onSave={handleSave}/>
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

function FileViwer(props) {
    const { files, onSave } = props;
    const [roles, setRoles] = React.useState(null);
    const [lpu, setLpu] = React.useState(null);
    const [items, setItems] = React.useState([]);
    const [validationErr, setValidationErr] = React.useState([]);
    const [selectAllRole, setSelectAllRole] = React.useState([]);
   

    const findMo = (filename,lpu) => {
        var regex = /\d{6}/;
        var match = regex.exec(filename);
        if (match != null) {
            if (lpu[match[0]])
                return lpu[match[0]];
        }
        return "";
    }

    
    const createItems = (moList) => {
        const dicLpu = {};
        moList.forEach(x => {
            dicLpu[x.MCOD] = x;
        });
        var list = [];
        files.forEach((x) => {
            list.push({
                FILE: x,
                FILENAME: x.name,
                SIZE: Math.round(x.size / 1024 * 100) / 100,
                MO: findMo(x.name, dicLpu),
                ROLE: [],
                Validation() {
                    const err = [];
                    if (!this.MO)
                        err.push(`Укажите МО для файла ${this.FILENAME}`);
                    if (this.ROLE.length === 0)
                        err.push(`Укажите роли для файла ${this.FILENAME}`);
                    return err;
                },
                isLoad: false,
                Text: ""
        }) });
        setItems(list);
    }
    useEffect(() => {
        window.fetch("GetRole", { credentials: "same-origin" })
            .then(response => response.json())
            .then(result => {
                if (result.Result === true)
                    setRoles(result.Value);
                else
                    alert("Ошибка получения справочника ролей:" + result.Value);
            });
     
        window.fetch("GetF003", { credentials: "same-origin"})
            .then(response => response.json())
            .then(result => {
                if (result.Result === true) {
                    setLpu(result.Value);
                    createItems(result.Value);
                }
                else
                    alert("Ошибка получения справочника МО:" + result.Value);
            });
        ;
    }, []);

  
    const [theme, setTheme] = React.useState("");
    const themeChange = (event) => {
        setTheme(event.target.value);
    }

    const moChange = (event, newValue, index) => {
        const newItems = [...items]; 
        newItems[index].MO =  newValue;
        setItems(newItems);
    }

    const roleChange = (event, newValue, index) => {
        const newItems = [...items];
        newItems[index].ROLE = newValue;
        setItems(newItems);
    }


    const selectAllRoleChange = (event, newValue) => {
        setSelectAllRole(newValue);
    }
    const setAllRole = () => {
        const newItems = [...items];
        newItems.forEach((value) => { value.ROLE = selectAllRole; });
        setItems(newItems);
    }

    const validation = () => {
        var validationError = [];
        items.forEach((value) => {
            var validationResult = value.Validation();
            if (validationResult.length !== 0)
                validationError = validationError.concat(validationResult);
        });
        if (theme === null || theme === "")
            validationError.push('Тема файлов обязательна к заполнению');
        setValidationErr(validationError);
        return validationError;
    }
    useEffect(() => {
        validation();
    }, [items,theme]);


    const updateItemState = async (item, isLoad, text) => {
        item.isLoad = isLoad;
        item.Text = text;
        setItems(items);
    }

    const [progress, setProgress] = React.useState(0);
    const [processSaveFiles, setProcessSaveFiles] = React.useState(false);


    const loadFile = async (item)=>
    {
        try {
           
            updateItemState(item, null, "Загрузка файла на сервер");

            const formData = new FormData();
            formData.append("FILE", item.FILE, item.FILE.name);
            formData.append("Theme", theme);
            
            formData.append("CODE_MO", item.MO.MCOD);
            item.ROLE.forEach((role) => {
                formData.append("ROLE_ID[]", role.SIGN_ROLE_ID);
            });
            const requestOptions = {
                method: "POST",
                credentials: "same-origin",
                body: formData
            };

            const response = await window.fetch("AddFileForSign", requestOptions);
            const result = await response.json();
            if (result.Result === true) {
                updateItemState(item, true, "Файл загружен");
            } else {
                updateItemState(item, false, "Ошибка при загрузке:" + result.Value);
            }
        } catch (err) {
            updateItemState(item, false, err.toString());
        }
        
    };
    const saveFiles = async () => {
        try {
            setProgress(0);
            setProcessSaveFiles(true);
            const length = items.length;
           
            for (let index = 0; index < items.length; index++) {
                await loadFile(items[index]);
                setProgress(index / length * 100);
            }
            if (items.every((val) => val.isLoad === true))
                onSave();
        } finally {
            setProcessSaveFiles(false);
        }
    }




    return (
        <div>
            <div>
                {validationErr != null ? <ul className="ErrorLi">{validationErr.map((value) => <li>{value}</li>)}</ul> : null}
            </div>
            <div>
                <div>
                    <div><Autocomplete multiple options={roles} getOptionLabel={(option) => option.CAPTION} value={selectAllRole} onChange={selectAllRoleChange} renderInput={(params) => <TextField {...params} variant="standard" label="Роли" variant="outlined" />}/></div>
                    <br/>
                    <div><Button variant="contained" color="primary" onClick={setAllRole}>Установить всем</Button></div>
                    <br />
                    <TextField margin="dense" label="Тема файлов" type="text" required fullWidth value={theme} onChange={themeChange}></TextField>
                </div>
                <br/>
                <table className="table_report">
                    <tr>
                        <th width="10%">Статус</th>
                        <th width="30%">Наименование файла</th>
                        <th width="10%">Размер файла(Кб)</th>
                        <th width="25%">Медицинская организация</th>
                        <th width="25%">Ожидаемые подписи</th>
                    </tr>
                    {items.map((value, index) =>
                        <tr>
                            <td>{value.Text}</td>
                            <td>{value.FILENAME}</td>
                            <td>{value.SIZE}</td>
                            <td  style={{margin:'5,5,5,5'}}><Autocomplete options={lpu} getOptionLabel={(option) => option.NAME} value={value.MO} onChange={(event, newValue)=>{moChange(event,newValue,index)}} renderInput={(params) => <TextField {...params} label="Медицинская организация" variant="outlined" />} /></td>
                            <td style={{margin: '5,5,5,5' }}><Autocomplete multiple options={roles} getOptionLabel={(option) => option.CAPTION} value={value.ROLE} onChange={(event, newValue) => { roleChange(event, newValue, index) }} renderInput={(params) => <TextField {...params}  variant="standard" label="Роли" variant="outlined" />} /></td>
                        </tr>
                    )}
                </table>
            </div>
            <br />
            {processSaveFiles === true ?
                <div>
                    <LinearProgressWithLabel value={progress} />
                    <br />
                </div> : null}
          
            <Button variant="contained" color="primary" onClick={saveFiles} disabled={validationErr == null || validationErr.length !== 0 || processSaveFiles}>Сохранить</Button>
        </div>
    );



    function LinearProgressWithLabel(props) {
        return (
            <Box display="flex" alignItems="center">
                <Box width="100%" mr={1}>
                    <LinearProgress variant="determinate" {...props} />
                </Box>
                <Box minWidth={35}>
                    <Typography variant="body2" color="textSecondary">{`${Math.round(props.value,)}%`}</Typography>
                </Box>
            </Box>
        );
    }

}
