﻿export function saveFile(blob, filename) {
    if (window.navigator.msSaveOrOpenBlob) {
        window.navigator.msSaveOrOpenBlob(blob, filename);
    } else {
        const a = document.createElement('a');
        document.body.appendChild(a);
        const url = window.URL.createObjectURL(blob);
        a.href = url;
        a.download = filename;
        a.click();
        setTimeout(() => {
                window.URL.revokeObjectURL(url);
                document.body.removeChild(a);
            },
            0);
    }
}


export function downloadBase64File(contentBase64, ContentType, fileName) {
    const linkSource = `data:${ContentType};base64,${contentBase64}`;
    const a = document.createElement('a');
    document.body.appendChild(a);
    a.href = linkSource;
    a.download = fileName;
    a.click();
    setTimeout(() => { document.body.removeChild(a); }, 0);
}