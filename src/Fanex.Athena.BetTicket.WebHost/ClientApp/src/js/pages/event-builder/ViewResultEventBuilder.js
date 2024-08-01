import UrlResultBuilder from '../../lib/UrlResultBuilder';
import SportType from '../../constants/SportType';
import DataService from '../../lib/DataService';

export default function ViewResultEventBuilder() {
    window.ViewResult = (matchid, raceNo, bettype, sporttype, refno,
        currency, winlostdate, refnoMixParlay, league, isOutright, extension1, extension2, custid, transdecs, siteId) => {
        const title = this._getResource('Result');
        const languageCode = this._getLanguage();
        const accessToken = this._getAccessToken();

        if (sporttype === SportType.RNG_GAME || sporttype === SportType.RNG_MINIGAME || sporttype === SportType.RNG_MOBILE) {
            const url = UrlResultBuilder.getRngCasinoResultUrl(this.rootUrl, languageCode);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype }, title, sporttype);
        }
        else if (sporttype === SportType.VOIDBRIDGE) {
            const url = UrlResultBuilder.getVoidbridgeResultUrl(this.rootUrl, languageCode);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype }, title, sporttype);
        }
        else if (sporttype === SportType.PP) {
            const url = UrlResultBuilder.getPPResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype }, title, sporttype);
        }
        else if (sporttype === SportType.CARDCLUB) {
            const url = UrlResultBuilder.getCardClubResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype }, title, sporttype);
        }
        else if (sporttype === SportType.SBC || sporttype === SportType.SBC_FreeBets) {
            const url = UrlResultBuilder.getSabaClubResultUrl(this.rootUrl, custid, bettype, extension2, transdecs, sporttype);
            _showThirdPartyResult.call(this, url, {
                languageCode,
                custid,
                gameId: bettype,
                siteId: extension2,
                gameRoundId: transdecs,
                sportType
            }, title, sporttype);
        }
        else if (sporttype === SportType.SG) {
            const url = UrlResultBuilder.getSGResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype }, title, sporttype);
        }
        else if (sporttype === SportType.VRG) {
            const url = UrlResultBuilder.getVRGResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype }, title, sporttype);
        }
        else if (sporttype === SportType.ALLBET) {
            const url = UrlResultBuilder.getAllBetResultUrl(this.rootUrl, transdecs, extension2, bettype, languageCode, accessToken);
            this._openPopup(url, title, sporttype);
        }
        else if (sporttype === SportType.SAG) {
            const url = UrlResultBuilder.getSAGamingResultUrl(this.rootUrl, transdecs, extension2, bettype, languageCode, accessToken);
            this._openPopup(url, title, sporttype);
        }
        else if (sporttype === SportType.JK) {
            const url = UrlResultBuilder.getJokerResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype }, title, sporttype);
        }
        else if (sporttype === SportType.SXG) {
            const url = UrlResultBuilder.getSexyGamingResultUrl(this.rootUrl, transdecs, extension2, bettype, languageCode, accessToken);
            this._openPopup(url, title, sporttype);
        }
        else if (sporttype === SportType.PGS) {
            const url = UrlResultBuilder.getPGSoftResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype }, title, sporttype);
        }
        else if (sporttype === SportType.Happy5LinkedGames) {
            const url = UrlResultBuilder.getHappy5LinkedGamesResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: refno, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype }, title, sporttype);
        }
        else if (sporttype === SportType.BGG) {
            const url = UrlResultBuilder.getBGGResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype }, title, sporttype);
        }
        else if (sporttype === SportType.SLT) {
            const url = UrlResultBuilder.getSportsLotteryResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype }, title, sporttype);
        }
        else if (sporttype === SportType.HB) {
            const url = UrlResultBuilder.getHabaneroResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype }, title, sporttype);
        }
        else if (sporttype === SportType.WM || sporttype === SportType.YB) {
            const url = UrlResultBuilder.getWMResultUrl(this.rootUrl, transdecs, extension2, bettype, languageCode, accessToken);
            this._openPopup(url, title, sporttype);
        }
        else if (sporttype === SportType.VGA || sporttype === SportType.VGA_FreeBets) {
            const url = UrlResultBuilder.getVGamingResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype }, title, sporttype);
        }
        else if (sporttype === SportType.CG) {
            const url = UrlResultBuilder.getCGResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype }, title, sporttype);
        }
        else if (sporttype === SportType.AP) {
            const url = UrlResultBuilder.getAdvantPlayResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype }, title, sporttype);
        }
        else if (sporttype === SportType.APM) {
            const url = UrlResultBuilder.getAdvantPlayMiniResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype }, title, sporttype);
        }
        else if (sporttype === SportType.VS3) {
            const url = UrlResultBuilder.getVS3ResultUrl(this.rootUrl, languageCode, accessToken, transdecs, sporttype);
            this._openPopup(url, title, sporttype);
        }
        else if (sporttype === SportType.FGGL) {
            const url = UrlResultBuilder.getFGGLResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype, WinLostDate: winlostdate }, title, sporttype);
        }
        else if (sporttype === SportType.MXG) {
            const url = UrlResultBuilder.getMaxGameResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype, WinLostDate: winlostdate }, title, sporttype);
        }
        else if (sporttype === SportType.JIG) {
            const url = UrlResultBuilder.getJiliResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype, WinLostDate: winlostdate }, title, sporttype);
        }
        else if (sporttype === SportType.MGRNG) {
            const url = UrlResultBuilder.getMGRNGResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype, WinLostDate: winlostdate }, title, sporttype);
        }
        else if (sporttype === SportType.SEA) {
            const url = UrlResultBuilder.getSealEntertainmentResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype, WinLostDate: winlostdate }, title, sporttype);
        }
        else if (sporttype === SportType.WE) {
            const url = UrlResultBuilder.getWELiveCasinoResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype, WinLostDate: winlostdate }, title, sporttype);
        }
        else if (sporttype === SportType.L22) {
            const url = UrlResultBuilder.getLive22ResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype, WinLostDate: winlostdate }, title, sporttype);
        }
        else if (sporttype === SportType.UUSL) {
            const url = UrlResultBuilder.getUUSlotsResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype, WinLostDate: winlostdate }, title, sporttype);
        }
        else if (sporttype === SportType.FS) {
            const url = UrlResultBuilder.getFastSpinResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, currencyId: extension1, siteId, betTypeId: bettype, WinLostDate: winlostdate }, title, sporttype);
        }
        else {
            const url = UrlResultBuilder.getResultUrl(this.rootUrl, languageCode, accessToken, matchid, raceNo, bettype,
                sporttype, winlostdate, refnoMixParlay, league, isOutright, extension1, extension2, custid);
            this._openPopup(url, title, sporttype);
        }
    }
}

function _showThirdPartyResult(url, payload, title, sporttype) {
    this._beforeShow();
    DataService.postData(url, payload, this._getAccessToken())
        .then((result) => {
            if (result.data !== '') {
                this._openPopup(result.data, title, sporttype);
            }

            this._afterShow();
        }).catch((error) => {
            this._afterShow();
            console.log(error);
        });
}