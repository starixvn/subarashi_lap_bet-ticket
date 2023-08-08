import language_en_US from './languages/ViewResources';
import language_zh_CN from './languages/ViewResources.zh-CN';
import language_vi_VN from './languages/ViewResources.vi-VN';

const resources = {
    'en-US': language_en_US,
    'zh-CN': language_zh_CN,
    'vi-VN': language_vi_VN
}

const DEFAULT_RESOURCE_NAME = "_betticket_language_en_US";
const langCode = window.PageLanguage || "en-US";

export default class LangHelper {

    static languageKeys = {
        eng: "en-US", 
        zhCN: "zh-CN",
        viVN: "vi-VN"
    }

    static get currentLanguage() {
        return langCode;
    }

    static getSingleResource(key, _langCode) {
        var langHandler = resources[_langCode || langCode];
        return (langHandler && langHandler[key]) || language_en_US[key] || key;
    }

    static getResources(keys) {
        var langHandler = window[resourceName];

        return keys.map((key) => (langHandler && langHandler[key]) || (window[DEFAULT_RESOURCE_NAME] && window[DEFAULT_RESOURCE_NAME][key]) || key);
    }
}