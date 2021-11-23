export class FileAPI {
    static downloadBase64File(contentBase64, contentType, fileName) {
        const linkSource = `data:${contentType};base64,${contentBase64}`;
        const a = document.createElement('a');
        document.body.appendChild(a);
        a.href = linkSource;
        a.download = fileName;
        a.click();
        setTimeout(() => { document.body.removeChild(a); }, 0);
    }
}
//# sourceMappingURL=FileAPI.js.map