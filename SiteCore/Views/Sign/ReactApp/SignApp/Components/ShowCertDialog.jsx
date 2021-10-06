import React, { Component, useRef, useEffect } from "react"

import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import Button from "@material-ui/core/Button";
import CertList from "./CertList.jsx"
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles((theme) => ({
    button: {
        marginRight: theme.spacing(1),
    }
}));

export default function ShowCertDialog(props) {
    const classes = useStyles();
    const { isOpen, certInfo, onClose } = props;

   
    const handleClose = () => {
        onClose();
    };
    return (
        <div>
            <Dialog open={isOpen} onClose={handleClose} aria-labelledby="form-dialog-title" fullWidth={true} maxWidth={"lg"}>
                <DialogContent>
                    <DialogContentText>
                        Просмотр данных сертификата
                    </DialogContentText>
                    <div>
                        {certInfo != null ? <CertList items={certInfo.Data} /> : <div></div>}
                    </div>
                    <div>
                        <Button className={classes.button} variant="contained" color="primary" onClick={handleClose}>Закрыть</Button>
                    </div>
                </DialogContent>
            </Dialog>
        </div>
    );
}
