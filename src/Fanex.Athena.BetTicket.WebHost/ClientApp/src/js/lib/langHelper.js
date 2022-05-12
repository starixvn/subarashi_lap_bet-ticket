import language_en_US from './languages/ViewResources';
import language_zh_CN from './languages/ViewResources.zh-CN';

const resources = {
    'en-US': language_en_US,
    'zh-CN': language_zh_CN 
}

const DEFAULT_RESOURCE_NAME = "_betticket_language_en_US";
const langCode = window.PageLanguage || "en-US";

export default class LangHelper {

    static languageKeys = {
        eng: "en-US", 
        zhCN: "zh-CN"
    }

    static get currentLanguage() {
        return langCode;
    }

    static getSingleResource(key, lang) {
        var langHandler = resources[lang || langCode] || language_en_US;
        return (langHandler && langHandler[key]) || key;
    }

    static getResources(keys) {
        var langHandler = window[resourceName];

        return keys.map((key) => (langHandler && langHandler[key]) || (window[DEFAULT_RESOURCE_NAME] && window[DEFAULT_RESOURCE_NAME][key]) || key);
    }
}