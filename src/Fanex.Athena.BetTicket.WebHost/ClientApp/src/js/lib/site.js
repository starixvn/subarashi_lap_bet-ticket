const _rootUrl = window.rootUrl || "/";
export default class Site {

    static resolveClientUrl(relativeUrl) {
        return _rootUrl + relativeUrl;
    }

    static convertDateToString(date) {
        return typeof date === "string" ? date : date.format("MM/DD/YYYY");
    }

    static downloadFile(data, fileName){
        const url = window.URL.createObjectURL(new Blob([data]));
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', fileName);
        document.body.appendChild(link);
        link.click();
        link.parentNode.removeChild(link);
        window.URL.revokeObjectURL(url);
    }

    static getRootUrl() {
        return _rootUrl;
    }
}