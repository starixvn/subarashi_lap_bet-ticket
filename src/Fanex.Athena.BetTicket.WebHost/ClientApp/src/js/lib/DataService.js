import axios from 'axios'

export default class DataService {
    static getData(url, accessToken) {
        return axios.get(url, {
            headers: this._buidlHeaders(accessToken)
        });
    }

    static postData(url, payload, accessToken) {
        return axios.post(url, payload, {
            headers: this._buidlHeaders(accessToken)
        });
    }

    static getFile(url, payload, accessToken) {
        return axios.post(url, payload, {
            responseType: 'blob',
            headers: this._buidlHeaders(accessToken)
        });
    }

    static _buidlHeaders(accessToken) {
        return  {
            'X-Requested-With': 'XMLHttpRequest',
            'Authorization': `Bearer ${accessToken}`
        }
    }
}