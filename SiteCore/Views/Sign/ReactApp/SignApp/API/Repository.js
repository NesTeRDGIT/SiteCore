
function throwResponse(response){
    throw new Error(`${response.status}-${response.statusText}`);
}

export class FileData{
    FileName=null;
    Data = null;
    constructor(FileName, Data) {
        this.FileName = FileName;
        this.Data = Data;
      }
}

export class Repository {


    async GetTheme() {

        const response = await window.fetch("GetTheme", { credentials: "same-origin" });

        if (response.ok) {

            const result = await response.json();

            if (result.Result) {
                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }

    async AddTheme(theme) {
        const formData = new FormData();
        formData.append("Theme", theme);
        const requestOptions = {
            method: "POST",
            credentials: "same-origin",
            body: formData
        };
        const response = await window.fetch("AddTheme", requestOptions);
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {
                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }

    async RemoveTheme(themeId) {
        const formData = new FormData();
        formData.append("THEME_ID", themeId);
        const requestOptions = {
            method: "POST",
            credentials: "same-origin",
            body: formData
        };
        const response = await window.fetch("RemoveTheme", requestOptions);
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {
                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }

    async GetDocsList(themeId) {

        const response = await window.fetch(`GetDOC?THEME_ID=${themeId}`, { credentials: "same-origin" });
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {
                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }

    async DownloadFileForSign(docForSignId) {
        const requestOptions = {
            method: "GET",
            headers: { 'Content-Type': "application/json" },
            credentials: "same-origin"
        };

        const response = await window.fetch(`DownloadFileForSign?DOC_FOR_SIGN_ID=${docForSignId}`, requestOptions);
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {
                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }

    async DownloadDocAndSign(docForSignId) {

        const requestOptions = {
            method: "GET",
            headers: { 'Content-Type': "application/json" },
            credentials: "same-origin"
        };
        const response =
            await window.fetch(`DownloadFileForSignAndSign?DOC_FOR_SIGN_ID=${docForSignId}`, requestOptions);
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {
                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }

   


    getFileNameFromContentDisposition(zglv)
    { 
        const attr = zglv.split(';');
        let filename = null;
        let filenameRFC5987 = null;
        for(let element of attr)
        {
            let at = element.trim();
            if(at.startsWith('filename='))    
                filename = at.replace('filename=', '');    
            if(at.startsWith('filename*=UTF-8\'\''))    
                filenameRFC5987 = at.replace('filename*=UTF-8\'\'', '');    
        }
        var filenameRFC5987decode = decodeURI(filenameRFC5987);
        return filenameRFC5987decode??filename;
    }

     async DownloadAllFileTheme(themeId, connectionId){

        const requestOptions = {
            method: "GET",
            headers: { 'Content-Type': "application/json" },
            credentials: "same-origin"
        };
        const response = await window.fetch(`DownloadAllFileTheme?THEME_ID=${themeId}&&ConnectionId=${connectionId}`, requestOptions);
        if (response.ok) {
            const zglv = response.headers.get('Content-Disposition');
            const filename = this.getFileNameFromContentDisposition(zglv);
            return new FileData(filename,await response.blob());
        } else {
            throwResponse(response);
        }
    }

    async RemoveDoc(docForSignId) {

        const requestOptions = {
            method: "POST",
            headers: { 'Content-Type': "application/json" },
            credentials: "same-origin",
            body: JSON.stringify(docForSignId)
        };
        const response = await window.fetch("RemoveFileForSign", requestOptions);
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {
                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }

    async GetRoleSPR() {
        const response = await window.fetch("GetRole", { credentials: "same-origin" });
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {
                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }

    async RemoveRole(signRoleId) {
        const requestOptions = {
            method: "POST",
            headers: { 'Content-Type': "application/json" },
            credentials: "same-origin",
            body: JSON.stringify(signRoleId)
        };
        const response = await window.fetch("RemoveRole", requestOptions);
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {
                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }


    AddRole = async (caption, prefix) => {

        const formData = new FormData();
        formData.append("caption", caption);
        formData.append("prefix", prefix);
        const requestOptions = {
            method: "POST",
            credentials: "same-origin",
            body: formData
        };
        const response = await window.fetch("AddRole", requestOptions);
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {
                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    };

    async GetF003() {

        const response = await window.fetch("GetF003", { credentials: "same-origin" });
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {
                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }

    async AddFileForSign(file, themeId, mcod, signRoleId) {
        const formData = new FormData();
        formData.append("FILE", file, file.name);
        formData.append("THEME_ID", themeId);
        formData.append("CODE_MO", mcod);
        signRoleId.forEach((id) => {
            formData.append("ROLE_ID[]", id);
        });
        const requestOptions = {
            method: "POST",
            credentials: "same-origin",
            body: formData
        };
        const response = await window.fetch("AddFileForSign", requestOptions);
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {
                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }


    async AddSigFile(file, docForSignId ) {
        const formData = new FormData();
        formData.append("FILE", file, file.name);
        formData.append("DOC_FOR_SIGN_ID", docForSignId);

        const requestOptions = {
            method: "POST",
            credentials: "same-origin",
            body: formData
        };
        const response = await window.fetch("UploadFileSig", requestOptions);
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {

                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }

    async GetISSUER() {
        const requestOptions = {
            method: "GET",
            credentials: "same-origin"
        };
        const response = await window.fetch("GetISSUER", requestOptions);
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {

                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }


    async RemoveISSUER(singIssuerId) {
        const requestOptions = {
            method: "POST",
            headers: { 'Content-Type': "application/json" },
            body: JSON.stringify(singIssuerId)
        };
        const response = await window.fetch("RemoveISSUER", requestOptions);
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {

                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }

    async GetISSUERCertInfo(singIssuerId) {
        const requestOptions = {
            method: "GET"
        };
        const response = await window.fetch(`GetISSUERCertInfo?ID=${singIssuerId}`, requestOptions);
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {

                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }

    async AddISSUER(file, caption, dateB, dateE ) {
        const formData = new FormData();
        formData.append("File", file, file.name);
        formData.append("CAPTION", caption);
        formData.append("DATE_B", dateB.toLocaleDateString());
        if (dateE != null)
            formData.append("DATE_E", dateE.toLocaleDateString());


        const requestOptions = {
            method: "POST",
            body: formData
        };
        const response = await window.fetch(`AddISSUER`, requestOptions);
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {

                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }

    async GetSignList() {
        const requestOptions = {
            method: "GET",
            credentials: "same-origin"
        };
        const response = await window.fetch(`GetSignList`, requestOptions);
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {

                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }

    async DownloadCert(id) {
        const requestOptions = {
            method: "GET",
            credentials: "same-origin"
        };
        const response = await window.fetch(`DownloadCert?id=${id}`, requestOptions);
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {

                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }
    async GetCertificateINFO(file) {
        const formData = new FormData();
        formData.append("file", file, file.name);
        const requestOptions = {
            method: "POST",
            credentials: "same-origin",
            body: formData
        };

        const response = await window.fetch(`GetCertificateINFO`, requestOptions);
        if (response.ok) {
            const result = await response.json();
            if (result.Result) {

                return result.Value;
            }
            throw new Error(result.Value);
        } else {
            throwResponse(response);
        }
    }
      
    AddSignCert = async (file, fileConfirm, signRoleId, mcod, dateB, dateE) => {
      
            const formData = new FormData();
            formData.append("File", file, file.name);
            formData.append("FileConfirm", fileConfirm, fileConfirm.name);
            formData.append("ROLE_ID", signRoleId);
            formData.append("CODE_MO", mcod);
            formData.append("DATE_B", new Date(dateB).toLocaleDateString());
            if (dateE != null)
                formData.append("DATE_E", new Date(dateE).toLocaleDateString());


            const requestOptions = {
                method: "POST",
                credentials: "same-origin",
                body: formData
            };

            const response = await window.fetch("AddCert", requestOptions);
            if (response.ok) {
                const result = await response.json();
                if (result.Result) {

                    return result.Value;
                }
                throw new Error(result.Value);
            } else {
                throwResponse(response);
            }
    }


   

}
