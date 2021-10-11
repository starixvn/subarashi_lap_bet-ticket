import ViewResultEventBuilder from './ViewResultEventBuilder' 
import ViewDetailEventBuilder from './ViewDetailEventBuilder' 
import LangHelper from '../../lib/langHelper';
import { formatDateString } from '../../lib/dateUtils';

class EventBuilder {

    rootUrl = '/';

    _getDomain = () => {
        var script = document.getElementById('event-builder-script'); 
        if(script) {
            const url = new URL(script.src);
            return url.origin + '/';
        } else {
            return '/';
        }
    }

    buildEvent() {
        this.rootUrl = this._getDomain();
        ViewResultEventBuilder.call(this);
        ViewDetailEventBuilder.call(this);
    }

    _getResource(key) {
        return window.getResource ? window.getResource(key) : LangHelper.getSingleResource(key);
    }

    _formatDateString(dateString) {
        return window.formatDateString ? window.formatDateString(dateString) : formatDateString(dateString);
    }

    _beforeShow() {
        window.beforeShow && window.beforeShow();
    }

    _afterShow() {
        window.afterShow && window.afterShow();
    }

    _openPopup(url, title, sporttype) {
        window.openPopup && window.openPopup(url, title, sporttype);
    }

    _getAccessToken() {
        return window.getAccessToken && window.getAccessToken();
    }

    _getBaseCurrency() {
        return window.getBaseCurrency && window.getBaseCurrency();
    }

    _getLanguage() {
        return window.getLanguage && window.getLanguage();
    }

    _resolveClientUrl(relativeUrl) {
        return this.rootUrl + relativeUrl;
    }
}

new EventBuilder().buildEvent();