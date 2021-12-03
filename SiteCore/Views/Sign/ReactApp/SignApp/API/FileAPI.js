
 import * as streamSaver from 'streamsaver'

export function saveFile(blob, filename) {
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
    debugger;
    const linkSource = `data:${ContentType};base64,${contentBase64}`;
    const a = document.createElement('a');
    document.body.appendChild(a);
    a.href = linkSource;
    a.download = fileName;
    a.click();
    setTimeout(() => { document.body.removeChild(a); }, 0);
}


export function downloadBase64FileAsync(contentBase64, ContentType, fileName) {
    return  new Promise(()=>
    {
        downloadBase64File(contentBase64,ContentType, fileName);
    });
}

export const downloadFileTest = (url) => {
    debugger;
  
    const fileStream = streamSaver.createWriteStream('archive.zip')
    /*let fileName = "test.zip"; 
    return window.fetch(url).then(res => {
        debugger;
    
      
        const fileStream = streamsaver.createWriteStream(fileName);
        const writer = fileStream.getWriter();

        
       

        const reader = res.body.getReader();
        const pump = () =>
            reader
            .read()
            .then(({ value, done }) => (done ? writer.close() : writer.write(value).then(pump)));

        return pump();
    });*/
};