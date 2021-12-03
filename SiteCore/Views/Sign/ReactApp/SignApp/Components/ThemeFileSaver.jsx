import React, { Component, useRef, useEffect } from "react"

import Dialog from "@material-ui/core/Dialog";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import LinearProgressWithLabel from "./LinearProgressWithLabel.jsx"
import { downloadBase64File ,downloadBase64FileAsync,saveFile} from "../API/FileAPI.js";
import HUBConnect from "../API/HubAPI.js"
import { Repository } from "../API/Repository.js";


const repo = new Repository();
const hub = new HUBConnect();
export default function ThemeFileSaver(props) {
  
    const { isOpen, onClose,  onSave, themeId } = props;
    const [errorMessage, setErrorMessage] = React.useState("");
    const [statusText, setStatusText] = React.useState("");
    const [progress, setProgress] = React.useState(50);
    let connectionId = "";
    const onProgress = (data) => {
        const maxProcess = data.maxProcess;
        const message = data.message;
        const process = data.process;
        setStatusText(message);
        if (maxProcess === 0) {
            setProgress(0);
        }
        else {
            setProgress(process / maxProcess * 100);
        }


    }

    const saveAllFile = async () => {
        try {
            const data = await repo.DownloadAllFileTheme(themeId, connectionId);  
            setStatusText("Сохранение файла"); 
            saveFile(data.Data, data.FileName);
            setStatusText("Сохранение файла завершено");
            onClose();
          
        } catch (error) {
            alert(error.toString());
        }
    }

    
    React.useEffect(async () => {
            
        if (isOpen) {
            connectionId = await hub.Connect();
            hub.onProgress(onProgress);
            saveAllFile();

        } else {
                hub.Disconnect();
            }
        },
        [isOpen]);

    const handleReset = () => {
        setStatusText("");
        setErrorMessage("");
        setProgress(0);
    };

    const handleClose = () => {
        onClose();
        handleReset();
    };

    return (
        <div>
            <Dialog open={isOpen} onClose={handleClose} aria-labelledby="form-dialog-title" fullWidth={true} maxWidth={false} >
                <DialogTitle id="form-dialog-title">Загрузка файла</DialogTitle>
                <DialogContent>
                    <div className="RedText">{errorMessage}</div>
                    <div>
                        <div>
                            <LinearProgressWithLabel value={progress} />
                        </div>
                        <div className="BoldText">{statusText}</div>
                    </div>
                </DialogContent>
            </Dialog>
        </div>
    );
}






