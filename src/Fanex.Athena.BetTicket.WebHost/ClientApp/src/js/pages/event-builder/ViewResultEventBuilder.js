import UrlResultBuilder from '../../lib/UrlResultBuilder';
import SportType from '../../constants/SportType';
import DataService from '../../lib/DataService';

export default function buildViewResultEvent() {
    window.ViewResult = (matchid, raceNo, bettype, sporttype, refno,
        username, winlostdate, refnoMixParlay, league, isOutright, extension1, extension2, custid, transdecs) => {

        const title = this._getResource('Result');
        const languageCode = this._getLanguage();
        const accessToken = this._getAccessToken();
        const defaultPlatform = 'd';
        
        if (sporttype === SportType.RNG_GAME || sporttype === SportType.RNG_MINIGAME || sporttype === SportType.RNG_MOBILE) {
            const url = UrlResultBuilder.getRngCasinoResultUrl(this.rootUrl, languageCode);
            _showRngCasinoResult.call(this, refno, url, title, sporttype);
        }
        else if (sporttype === SportType.VOIDBRIDGE) {
            const url = UrlResultBuilder.getVoidbridgeResultUrl(this.rootUrl, languageCode);
            _showThirdPartyResult.call(this, url, { languageCode, custid, gamePlayId: extension2 }, title, sporttype);
        }
        else if (sporttype === SportType.PP) {
            const url = UrlResultBuilder.getPPResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, gamePlayId: extension2 }, title, sporttype);
        }
        else if (sporttype === SportType.CARDCLUB) {
            const url = UrlResultBuilder.getCardClubResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, gameTypeId: bettype, transId3rd: extension2 }, title, sporttype);
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
            _showThirdPartyResult.call(this, url, { languageCode, custid, gamePlayId: extension2 }, title, sporttype);
        }
        else if (sporttype === SportType.VRG) {
            const url = UrlResultBuilder.getVRGResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, gamePlayId: refno, platform: defaultPlatform }, title, sporttype);
        }
        else if (sporttype === SportType.ALLBET) {
            const url = UrlResultBuilder.getAllBetResultUrl(this.rootUrl, transdecs, extension2, bettype, languageCode, accessToken);
            this._openPopup(url, title, sporttype);
        }
        else if (sporttype === SportType.SAG) {
            const url = UrlResultBuilder.getSAGamingResultUrl(this.rootUrl, transdecs, extension2, bettype, languageCode, accessToken);
            this._openPopup(url, title, sporttype);
        }
        else if (sporttype === SportType.GLOBALGAMING) {
            const url = UrlResultBuilder.getGlobalGamingResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { language: languageCode, custid, gameId: extension2 }, title, sporttype);
        }
        else if (sporttype === SportType.JK) {
            const url = UrlResultBuilder.getJokerResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, gamePlayId: transdecs, gameCode: extension2 }, title, sporttype);
        }
        else if (sporttype === SportType.SXG) {
            const url = UrlResultBuilder.getSexyGamingResultUrl(this.rootUrl, transdecs, extension2, bettype, languageCode, accessToken);
            this._openPopup(url, title, sporttype);
        }
        else if (sporttype === SportType.PGS) {
            const url = UrlResultBuilder.getPGSoftResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, gamePlayId: transdecs, gameId3rd: extension2 }, title, sporttype);
        }
        else if (sporttype === SportType.Happy5LinkedGames) {
            const url = UrlResultBuilder.getHappy5LinkedGamesResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid: extension1, transId3rd: refno, platform: defaultPlatform }, title, sporttype);
        }
        else if (sporttype === SportType.GDG) {
            const url = UrlResultBuilder.getGDGResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, siteId: league }, title, sporttype);
        } else if (sporttype === SportType.BGG) {
            const url = UrlResultBuilder.getBGGResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, currencyId: league, siteId: extension2 }, title, sporttype);
        }
        else if (sporttype === SportType.SLT) {
            const url = UrlResultBuilder.getSportsLotteryResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, platform: defaultPlatform }, title, sporttype);
        }
        else if (sporttype === SportType.HB) {
            const url = UrlResultBuilder.getHabaneroResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, transId3rd: transdecs, gameId3rd: extension2, siteId: extension1 }, title, sporttype);
        }
        else if (sporttype === SportType.WM) {
            const url = UrlResultBuilder.getWMResultUrl(this.rootUrl, transdecs, extension2, bettype, languageCode, accessToken);
            this._openPopup(url, title, sporttype);
        }
        else if (sporttype === SportType.VGA || sporttype === SportType.VGA_FreeBets) {
            const url = UrlResultBuilder.getVGamingResultUrl(this.rootUrl);
            _showThirdPartyResult.call(this, url, { languageCode, custid, gameId: bettype, siteId: extension2, gameRoundId: transdecs, sporttype }, title, sporttype);
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

function _showRngCasinoResult(refno, url, title, sporttype) {
    this._beforeShow();
    DataService.postData(url, { refno }, this._getAccessToken())
        .then((result) => {
            if (result.data.Success) {
                if (result.data.Result === '') {
                    return;
                }

                this._openPopup(result.data.Result, title, sporttype);
            }
            else {
                alert(this._getResource("ErrorProcessingMsg"));
            }

            this._afterShow();
        }).catch((error) => {
            this._afterShow();
            console.log(error);
        });
}