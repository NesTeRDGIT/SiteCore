import React, { Component, useRef, useEffect } from "react"
import MaterialTable from "material-table"
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogTitle from "@material-ui/core/DialogTitle";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import { Repository } from "../API/Repository.js";
import { makeStyles, createTheme, MuiThemeProvider } from '@material-ui/core/styles';

const theme = createTheme({
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



const repo = new Repository();

export default function RoleList() {
    const tableRef = React.useRef(null);
    
    const [isOpenRoleDialog, setIsOpenRoleDialog] = React.useState(false);
    const closeRoleDialog = () => {
        setIsOpenRoleDialog(false);
        refresh();
    };
    const [roleList, setRoleList] = React.useState([]);
    const refresh =async () => {
        try {
            setRoleList(await repo.GetRoleSPR());
        } catch (err) {
            alert(err.toString());
        }
       
    }

    React.useEffect(() => { refresh(); }, []);
  
   

    const removeRole = async (event,rowData) => {
        try {
            if (confirm(`Вы уверены что хотите удалить роль: ${rowData.CAPTION}?`)) {
                await repo.RemoveRole(rowData.SIGN_ROLE_ID);
                await refresh();
            }
        } catch (error) {
            alert(error.toString());
        }
    }

    return (
        <div style={{ maxWidth: "100%" }}>
            <MuiThemeProvider theme={theme}>
            <MaterialTable tableRef={tableRef}
                columns={[
                    { title: "ID", field: "SIGN_ROLE_ID" },
                    { title: "Наименование роли", field: "CAPTION" },
                    { title: "Префикс", field: "PREFIX" }
                ]}
                data={roleList}
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
                        tooltip: "Добавить роль",
                        isFreeAction: true,
                        onClick: (event) => { setIsOpenRoleDialog(true); }
                    },
                    {
                        icon: "delete",
                        iconProps: { color: "error" },
                        tooltip: "Удалить",
                        onClick: removeRole
                    }
                ]}
                title="Список доступных ролей"
                options={{
                    paging: false,
                    actionsColumnIndex: -1,
                    search: false,
                    sorting: true
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
                    }
                }}
                />
            </MuiThemeProvider>
            <RoleDialog isOpen={isOpenRoleDialog} onClose={closeRoleDialog} />
        </div>
    );

}


export function RoleDialog(props) {
    const { onClose, isOpen } = props;

    const [errorMessage, setErrorMessage] = React.useState("");
    const [caption, setCaption] = React.useState("");
    const handleCaptionChange = (event) => { setCaption(event.target.value); };

    const [prefix, setPrefix] = React.useState("");
    const prefixChange = (event) => { setPrefix(event.target.value); };

    const handleClose = () => {
        onClose();
        setCaption("");
        setPrefix("");
    };


    const [processSaveRole, setProcessSaveRole] = React.useState(false);
    const saveRole =async  () => {
        try {
            setProcessSaveRole(true);
            await repo.AddRole(caption, prefix);
            setErrorMessage("");
            setCaption("");
            handleClose();
        } catch (err) {
            setErrorMessage(err.toString());
        } finally {
            setProcessSaveRole(false);
        }
    };
    return (
        <div>
            <div>{isOpen}</div>
            <Dialog open={isOpen} onClose={handleClose} aria-labelledby="form-dialog-title">
                <DialogTitle id="form-dialog-title">Добавление роли</DialogTitle>
                <DialogContent>
                    <div>Введите наименование роли и нажмите на кнопку "Сохранить"</div>
                    {Array.isArray(errorMessage) ? errorMessage.map((value, index) => <div key={index} className="RedText">{value}</div>) : <div className="RedText">{errorMessage}</div>}
                    <TextField autoFocus margin="dense" label="Наименование роли" type="text" required fullWidth value={caption} onChange={handleCaptionChange}></TextField>
                    <br/>
                    <TextField margin="dense" label="Префикс роли" type="text" fullWidth required value={prefix} onChange={prefixChange}></TextField>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose} color="primary">Отменить</Button>
                    <Button onClick={saveRole} color="primary" disabled={processSaveRole}>Сохранить</Button>
                </DialogActions>
            </Dialog>
        </div>
    );
}