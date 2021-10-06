import React, { Component, useRef, useEffect } from "react"
import ReactDOM from "react-dom"

import MaterialTable from "material-table"
import Stepper from '@material-ui/core/Stepper';
import Step from '@material-ui/core/Step';
import StepLabel from '@material-ui/core/StepLabel';
import StepContent from '@material-ui/core/StepContent';
import { makeStyles, createTheme , MuiThemeProvider } from '@material-ui/core/styles';
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
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import Menu from '@material-ui/core/Menu';
import MenuItem from '@material-ui/core/MenuItem';
import MenuList from '@material-ui/core/MenuList';

import SingDialog from "./SignDialog.jsx"


import Box from '@material-ui/core/Box';
import LinearProgress from '@material-ui/core/LinearProgress';

import { downloadBase64File } from "../API/FileAPI.js";
import { Repository } from "../API/Repository.js";
import Grid from '@material-ui/core/Grid';
import Divider from '@material-ui/core/Divider';
import Fade from '@material-ui/core/Fade';
import Popper from '@material-ui/core/Popper';
import { ContextMenu, MenuItem as ContextMenuItem, ContextMenuTrigger } from "react-contextmenu";
import CircularProgress from '@material-ui/core/CircularProgress';

import LinearProgressWithLabel from "./LinearProgressWithLabel.jsx"

import ThemeFileSaver from "./ThemeFileSaver.jsx";


const repo = new Repository();
const theme = createTheme ({
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

    const [themeList, setThemeList] = React.useState([]);
    const onRefreshTheme = async () => {
        setThemeList(await repo.GetTheme());
    }

    React.useEffect(async () => {
        try {
            onRefreshTheme();
        }
        catch(err) {
            alert(err.message);
        }
    }, []);
    const [currentTheme, setCurrentTheme] = React.useState([]);
   
    const [docsList, setDocsList] = React.useState([]);
    const onSelectThemeChanged = async (themeItem) => {
        try {
            setCurrentTheme(themeItem);
            setDocsList(await repo.GetDocsList(themeItem.THEME_ID));
        } catch (err) {
            alert(err.toString());
        }
    }


    async function refresh() {
        try {
            if (currentTheme!=null)
                setDocsList(await repo.GetDocsList(currentTheme.THEME_ID));
        } catch (err) {
            alert(err.toString());
        }
    }

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

   
   
    const downloadDoc = async (event, rowData) => {
        try {
        
            const data = await repo.DownloadFileForSign(rowData.DOC_FOR_SIGN_ID);
            downloadBase64File(data.FileContents, data.ContentType, data.FileDownloadName);
        } catch (error) {

            alert(error.toString());
        }
    }
    const downloadDocAndSign = async (event, rowData) => {
        try {
            const data = await repo.DownloadDocAndSign(rowData.DOC_FOR_SIGN_ID);
            downloadBase64File(data.FileContents, data.ContentType, data.FileDownloadName);
        } catch (error) {
            alert(error.toString());
        }
    }

 
    const removeDocs = async (event, rowsData) => {
        try {
            if (confirm(`Вы уверены что хотите удалить ${rowsData.length} документ?`)) {
                for (const item of rowsData) {
                    await repo.RemoveDoc(item.DOC_FOR_SIGN_ID);
                }
                refresh();
            }
        } catch (error) {
            alert(error.toString());
        }
    }

    const [isOpenSigFileDialog, setIsOpenSigFileDialog] = React.useState(false);
    const [docForSignId, setDocForSignId] = React.useState(null);
    
    const closeSigFileDialog = () => {
        setIsOpenSigFileDialog(false);
    }
    const saveSigFileDialog = () => {
        closeSigFileDialog();
        refresh();
    }

    const uploadSigFile = (event, rowsData) => {
        setDocForSignId(rowsData.DOC_FOR_SIGN_ID);
        setIsOpenSigFileDialog(true);
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
                position: 'row',
                onClick: (e, data) => { uploadSigFile(e, data) }

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

            <Grid container spacing={1}>
                <Grid item={true} xs={2}>
                    <ListTheme themeItems={themeList} onSelectChanged={onSelectThemeChanged} isAdmin={isAdmin} onRefresh={onRefreshTheme}/>
                </Grid>
                <Grid item={true} xs={10}>
                    <MuiThemeProvider theme={theme}>
                        <MaterialTable tableRef={tableRef}
                                       columns={[
                                    { title: "Медицинская организация", field: "MO_NAME" },
                                    { title: "Имя файла", field: "FILENAME" },
                                    { title: "Дата", field: "DateCreate", type: "date" },
                                    {
                                        title: "Подписано",
                                        field: "ROLE_SIGN",
                                        render: rowData =>
                                            <div>
                                                {rowData.SIGNS.map((value,index) =>
                                                    <div key={index}>
                                                        <span className={value.IsSIGN ? "GreenText" : "RedText"}>{value.ROLE_NAME}</span><br />
                                                    </div>)}
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
                                       title="Подписи"/>
                    </MuiThemeProvider>
                    <AddDocsDialog onClose={closeAddDocsDialog} isOpen={isOpenAddDocsDialog} onSave={saveDocsDialog} themeId={currentTheme!=null? currentTheme.THEME_ID: null}/>
                    <SingDialog onClose={closeSignDialog} onSave={saveSignDialog} isOpen={isOpenSignDialog} files={selectedFiles}/>
                    <AddSigFileDialog onClose={closeSigFileDialog} onSave={saveSigFileDialog} isOpen={isOpenSigFileDialog} docForSignId={docForSignId}/>
                </Grid>
            </Grid>


        </div>
    );


}

const useStyles = makeStyles((theme) => ({
    root: {
        width: '100%',
        maxWidth: 360,
        backgroundColor: theme.palette.background.paper,
    },
    themeListItem: {
        color: 'green !important'
    }
}));

function ListTheme(props) {
    const classes =  useStyles();
    const { themeItems, onSelectChanged, isAdmin, onRefresh } = props;
    const [selectedIndex, setSelectedIndex] = React.useState(-1);
    const [currentThemeId, setCurrentThemeId] = React.useState(null);
    const [openAddDialog, setOpenAddDialog] = React.useState(false);
    const changeSelectedIndex = (newIndex) => {
        setSelectedIndex(newIndex);
        setCurrentThemeId(themeItems[newIndex].THEME_ID);
        onSelectChanged(themeItems[newIndex]);
    }

    React.useEffect(() => {
        if (selectedIndex === -1 && themeItems.length !== 0)
            changeSelectedIndex(0);
    }, [themeItems]);


    function renderList() {
        return <List>
                   {themeItems != null
                       ? themeItems.map((value, index) =>
                           <ListItem key={value.THEME_ID} selected={selectedIndex === index} className={classes.themeListItem}  onClick={()=>changeSelectedIndex(index)} button>
                               <ListItemText primary={value.CAPTION}/>
                           </ListItem>
                       )
                       : null}
               </List>;
    }
   
    const removeThemeClick = async () => {
        try {
            await repo.RemoveTheme(themeItems[selectedIndex].THEME_ID);
            onRefresh();
        } catch (err) {
            alert(err.message);
        }
      
    }
    const [openThemeFileSaver, setOpenThemeFileSaver] = React.useState(false);
    const saveAllFileThemeClick =  () => {
        try {
            setOpenThemeFileSaver(true);
        } catch (error) {
            alert(error.toString());
        }
    }

    const closeOpenThemeFileSaver = async () => {
        try {
            setOpenThemeFileSaver(false);
        } catch (error) {
            alert(error.toString());
        }
    }


   
    const showAddDialog = () => {
        setOpenAddDialog(true);
    }
    const hideAddDialog = () => {
        setOpenAddDialog(false);
    }
    const onSaveAddDialog = () => {
        hideAddDialog();
        onRefresh();
    }
    const handleRefresh = () => {
        onRefresh();
    }
   
   

    return (
        <div>
            <div>Список тем:</div>
            <Divider />
            <div>
                <ContextMenuTrigger id="ThemeContextMenu1">
                    <div>{renderList()}</div>
                </ContextMenuTrigger>
                <ContextMenu id="ThemeContextMenu1">
                    {isAdmin ? <ContextMenuItem onClick={showAddDialog}><MenuItem>Добавить</MenuItem></ContextMenuItem> : null}
                    {isAdmin ? <ContextMenuItem onClick={saveAllFileThemeClick} ><MenuItem>Скачать все файлы</MenuItem></ContextMenuItem> : null}
                    {isAdmin ? <Divider /> : null}
                  
                   
                    <ContextMenuItem onClick={handleRefresh} ><MenuItem>Обновить</MenuItem></ContextMenuItem>
                    {isAdmin ? <Divider /> : null}
                    {isAdmin ? <ContextMenuItem onClick={removeThemeClick}><MenuItem>Удалить</MenuItem></ContextMenuItem> : null}
                </ContextMenu>
                <AddThemeDialog isOpen={openAddDialog} onClose={hideAddDialog} onSave={onSaveAddDialog}/>
                {isAdmin ? <ThemeFileSaver isOpen={openThemeFileSaver} themeId={currentThemeId} onClose={closeOpenThemeFileSaver}/> : null}
                
            </div>
        </div>
    );
}


export function AddThemeDialog(props) {
    const { onClose, isOpen, onSave } = props;
    const [errorMessage, setErrorMessage] = React.useState("");
    const [theme, setTheme] = React.useState("");

    React.useEffect(() => { setTheme(""); },[]);

    const themeChange = (event) => {
        setTheme(event.target.value);
    }
    const handleClose = () => {
        onClose();
    };
    const handleSave = async () => {
        try {
            await repo.AddTheme(theme);
            onSave();
            handleClose();
        } catch (err) {
            setErrorMessage(err.toString());
        }

    };
    return (
        <div>
            <Dialog open={isOpen} onClose={handleClose} aria-labelledby="form-dialog-title" fullWidth={true} maxWidth={false} >
                <DialogTitle id="form-dialog-title">Добавление файлов на подпись</DialogTitle>
                <DialogContent>
                    <div className="RedText">{errorMessage}</div>
                    <div>
                        <TextField margin="dense" label="Наименование темы" type="text" required fullWidth value={theme} onChange={themeChange}></TextField>
                    </div>
                    <Button variant="contained" color="primary" disabled={theme === ""} onClick={handleSave}>Сохранить</Button>
                </DialogContent>
            </Dialog>
        </div>
    );
}



export function AddDocsDialog(props) {
    const classes = useStyles();
    const { onClose, isOpen, onSave, themeId } = props;
    
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
            <Dialog open={isOpen} onClose={handleClose} aria-labelledby="form-dialog-title" fullWidth={true} maxWidth={false} >
                <DialogTitle id="form-dialog-title">Добавление файлов на подпись</DialogTitle>
                <DialogContent>
                    <div>
                        <div>
                            <Stepper activeStep={activeStep} orientation="vertical">
                                <Step key={0}>
                                    <StepLabel>Выбор файлов</StepLabel>
                                    <StepContent>
                                        <div>
                                            <Typography>Выберите файлы</Typography>
                                            <input type="file" multiple={true}  onChange={selectFile} />
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
                                        <FileViwer files={files} themeId={themeId} onSave={handleSave}/>
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

export function AddSigFileDialog(props) {
    const classes = useStyles();
    const { onClose, isOpen, onSave, docForSignId } = props;
    const [errorMessage, setErrorMessage] = React.useState("");
    const [file, setFile] = React.useState(null);
    const [processing, setProcessing] = React.useState(false);



    const selectFile = (e) => {
        var file = e.target.files[0];
        setFile(file);
    };

  
    const handleReset = () => {
        setFile(null);
        setErrorMessage("");
    };
    const handleSave = async () => {
        try {
            setErrorMessage("");
            setProcessing(true);
            await repo.AddSigFile(file, docForSignId);
            onSave();
            handleReset();
        } catch (err) {
            setErrorMessage(err.toString());
        } finally {
            setProcessing(false);
        }
      
    };

    const handleClose = () => {
        onClose();
        handleReset();
    };

    return (
        <div>
            <Dialog open={isOpen} onClose={handleClose} aria-labelledby="form-dialog-title" fullWidth={false} maxWidth={false}>
                <DialogTitle id="form-dialog-title">Добавление открепленной подписи</DialogTitle>
                <DialogContent>
                    <div className="RedText">{errorMessage}</div>
                    <div>
                        <div>
                            <Typography>Выберите файлы</Typography>
                            <input type="file" accept=".sig" onChange={selectFile}/>
                        </div>
                        <br/>
                        {processing ? 
                            <div>
                                <CircularProgress size="25px" /> <br/>
                                <span className="BoldText">Отправка данных</span>
                            </div> : null}
                        <div>
                            <Button className={classes.button} variant="contained" color="primary" onClick={handleSave} disabled={file == null || processing}>Отправить</Button>
                        </div>
                    </div>
                </DialogContent>
            </Dialog>
        </div>
    );
}



function FileViwer(props) {
    const { files, themeId, onSave } = props;
    const [roles, setRoles] = React.useState([]);
    const [lpu, setLpu] = React.useState([]);
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
        moList.forEach(x => { dicLpu[x.MCOD] = x; });
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
    useEffect(async () => {
        try {
            setRoles(await repo.GetRoleSPR());
            const f003 = await repo.GetF003();
           createItems(f003);
            setLpu(f003);
        } catch (err) {
            alert(err.toString());
        }
    
    }, []);

  
 
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
        setValidationErr(validationError);
        return validationError;
    }
    useEffect(() => { validation(); }, [items]);


    const updateItemState = async (item, isLoad, text) => {
        item.isLoad = isLoad;
        item.Text = text;
        setItems(items);
    }

    const [progress, setProgress] = React.useState(0);
    const [processSaveFiles, setProcessSaveFiles] = React.useState(false);


    
    const saveFiles = async () => {
        try {
            setProgress(0);
            setProcessSaveFiles(true);
            const length = items.length;
           
            for (let index = 0; index < items.length; index++) {
                const item = items[index];
                updateItemState(item, null, "Загрузка файла на сервер");
                try {
                    
                    await repo.AddFileForSign(item.FILE, themeId, item.MO.MCOD, item.ROLE.map((value)=>value.SIGN_ROLE_ID));
                    updateItemState(item, true, "Файл загружен");
                } catch (err) {
                    updateItemState(item, false, `Ошибка при загрузке:${err.toString()}`);
                }
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
                {validationErr != null ? <ul className="ErrorLi">{validationErr.map((value, index) => <li key={index}>{value}</li>)}</ul> : null}
            </div>
            <div>
                <div>
                    <div><Autocomplete multiple={true}  options={roles} getOptionLabel={(option) => option.CAPTION} value={selectAllRole} onChange={selectAllRoleChange} renderInput={(params) => <TextField {...params} variant="standard" label="Роли" variant="outlined" />}/></div>
                    <br/>
                    <div><Button variant="contained" color="primary" onClick={setAllRole}>Установить всем</Button></div>
                </div>
                <br/>
                <table className="table_report">
                    <tbody>
                    <tr>
                        <th width="10%">Статус</th>
                        <th width="30%">Наименование файла</th>
                        <th width="10%">Размер файла(Кб)</th>
                        <th width="25%">Медицинская организация</th>
                        <th width="25%">Ожидаемые подписи</th>
                    </tr>
                    {items.map((value, index) =>
                        <tr key={index}>
                                <td>{value.Text}</td>
                                <td>{value.FILENAME}</td>
                                <td>{value.SIZE}</td>
                            <td style={{ margin: 5 }}><Autocomplete options={lpu} getOptionLabel={(option) => option ? option.NAME:""} value={value.MO} onChange={(event, newValue) => { moChange(event, newValue, index) }} renderInput={(params) => <TextField {...params} label="Медицинская организация" variant="outlined" />} /></td>
                            <td style={{ margin: 5 }}><Autocomplete multiple={true} options={roles} getOptionLabel={(option) => option ? option.CAPTION : ""} value={value.ROLE} onChange={(event, newValue) => { roleChange(event, newValue, index) }} renderInput={(params) => <TextField {...params} variant="standard" label="Роли" variant="outlined" />} /></td>
                            </tr>
                        )}
                    </tbody>
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



}
