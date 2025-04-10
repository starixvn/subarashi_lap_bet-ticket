import QueryString from '@jsroot/lib/QueryString'
import BetType from '../constants/BetType';

export default class UrlResultBuilder {
    static _isFastMarket(bettype) {
        return bettype >= BetType.MIN_FASTMARKET && bettype <= BetType.MAX_FASTMARKET;
    }

    static _getMixParlayResultUrl(rootUrl, refno, winlostdate, bettype) {
        let url = rootUrl + 'SportsResult/Index';
        url = QueryString.setParam(url, 'refno', refno);
        url = QueryString.setParam(url, 'winlossdate', winlostdate);
        url = QueryString.setParam(url, 'bettype', bettype);
        url = QueryString.setParam(url, 'matchid', 1);

        return url;
    }

    static _getSportBookResultUrl(rootUrl, matchid, bettype, raceNo, sporttype, league, isOutright, extension1, extension2, custid, winlostdate) {
        let url = rootUrl + 'SportsResult/Index';
        url = QueryString.setParam(url, 'matchid', matchid);
        url = QueryString.setParam(url, 'bettype', bettype);
        url = QueryString.setParam(url, 'raceno', raceNo);
        url = QueryString.setParam(url, 'sporttype', sporttype);
        url = QueryString.setParam(url, 'isoutright', isOutright);
        url = QueryString.setParam(url, 'winLossdate', winlostdate);

        if (this._isFastMarket(bettype)) {
            url = QueryString.setParam(url, 'handicap1', extension1);
            url = QueryString.setParam(url, 'handicap2', extension2);
        } else {
            url = QueryString.setParam(url, 'betid', extension1);
            url = QueryString.setParam(url, 'transdesc', extension2);
        }

        return url;
    }

    static getResultUrl(rootUrl, language, accessToken, matchid, raceNo, bettype, sporttype, winlostdate, refnoMixParlay, league, isOutright, extension1, extension2, custid) {
        bettype = Number(bettype);

        let isParlay = bettype === BetType.MIXPARLAY || bettype === BetType.SYSTEMPARLAY || bettype === BetType.MIXPARLAY_BETRADAR || bettype === BetType.MIXPARLAY_VS4 || bettype === BetType.MIXPARLAY_VS5;
        let url = isParlay
            ? this._getMixParlayResultUrl(rootUrl, refnoMixParlay, winlostdate, bettype)
            : this._getSportBookResultUrl(rootUrl, matchid, bettype, raceNo, sporttype, league, isOutright, extension1, extension2, custid, winlostdate);

        url = QueryString.setParam(url, 'language', language);
        url = QueryString.setParam(url, 'access_token', accessToken);

        return url;
    }

    static getRngCasinoResultUrl(rootUrl, language) {
        return rootUrl + 'Proxy/RNGCasinoResultPopup/Index';
    }

    static getVoidbridgeResultUrl(rootUrl) {
        return rootUrl + 'Proxy/VoidbridgeResultPopup/Index';
    }

    static getPPResultUrl(rootUrl) {
        return rootUrl + 'Proxy/PPResultPopup/Index';
    }

    static getCardClubResultUrl(rootUrl) {
        return rootUrl + 'Proxy/CardClubResultPopup/Index';
    }

    static getSabaClubResultUrl(rootUrl) {
        return rootUrl + 'Proxy/SabaClubResultPopup/Index';
    }

    static getSGResultUrl(rootUrl) {
        return rootUrl + 'Proxy/SGResultPopup/Index';
    }

    static getVRGResultUrl(rootUrl) {
        return rootUrl + 'Proxy/VirtualGamesResultPopup/Index';
    }

    static getCGResultUrl(rootUrl) {
        return rootUrl + 'Proxy/CGResultPopup/Index';
    }

    static getAdvantPlayResultUrl(rootUrl) {
        return rootUrl + 'Proxy/AdvantPlayResultPopup/Index';
    }

    static getAdvantPlayMiniResultUrl(rootUrl) {
        return rootUrl + 'Proxy/AdvantPlayMiniResultPopup/Index';
    }

    static getFGGLResultUrl(rootUrl) {
        return rootUrl + 'Proxy/FGGLResultPopup/Index';
    }

    static getMaxGameResultUrl(rootUrl) {
        return rootUrl + 'Proxy/MaxGameResultPopup/Index';
    }

    static getVS3ResultUrl(rootUrl, language, accessToken, transdecs, sporttype) {
        let url = rootUrl + 'SportsResult/Index';
        url = QueryString.setParam(url, 'language', language);
        url = QueryString.setParam(url, 'access_token', accessToken);
        url = QueryString.setParam(url, 'sporttype', sporttype);
        url = QueryString.setParam(url, 'transdesc', encodeURIComponent(transdecs));

        return url;
    }

    static getAllBetResultUrl(rootUrl, transdesc, betcheck, bettype, language, accessToken) {
        let url = rootUrl + 'NonSportsResult/AllBet';
        url = QueryString.setParam(url, 'language', language);
        url = QueryString.setParam(url, 'access_token', accessToken);
        url = QueryString.setParam(url, 'betcheck', betcheck);
        url = QueryString.setParam(url, 'bettype', bettype);
        url = QueryString.setParam(url, 'transdesc', encodeURIComponent(transdesc));

        return url;
    }

    static getSAGamingResultUrl(rootUrl, transdesc, matchCode, bettype, language, accessToken) {
        let url = rootUrl + 'NonSportsResult/SAGaming';
        url = QueryString.setParam(url, 'language', language);
        url = QueryString.setParam(url, 'access_token', accessToken);
        url = QueryString.setParam(url, 'matchCode', matchCode);
        url = QueryString.setParam(url, 'bettype', bettype);
        url = QueryString.setParam(url, 'transdesc', encodeURIComponent(transdesc));

        return url;
    }

    static getJokerResultUrl(rootUrl) {
        return rootUrl + 'Proxy/JokerResultPopup/Index';
    }

    static getPGSoftResultUrl(rootUrl) {
        return rootUrl + 'Proxy/PGSoftResultPopup/Index';
    }

    static getBGGResultUrl(rootUrl) {
        return rootUrl + 'Proxy/BGGResultPopup/Index';
    }

    static getHappy5LinkedGamesResultUrl(rootUrl) {
        return rootUrl + 'Proxy/Happy5LinkedGamesResultPopup/Index';
    }

    static getSportsLotteryResultUrl(rootUrl) {
        return rootUrl + 'Proxy/SportsLotteryResultPopup/Index';
    }

    static getHabaneroResultUrl(rootUrl) {
        return rootUrl + 'Proxy/HabaneroResultPopup/Index';
    }

    static getWMResultUrl(rootUrl, transDesc, gameId3rd, bettype, language, accessToken) {
        let url = rootUrl + 'NonSportsResult/WM';
        url = QueryString.setParam(url, 'language', language);
        url = QueryString.setParam(url, 'access_token', accessToken);
        url = QueryString.setParam(url, 'gameId3rd', gameId3rd);
        url = QueryString.setParam(url, 'bettype', bettype);
        url = QueryString.setParam(url, 'transdesc', encodeURIComponent(transDesc));

        return url;
    }

    static getVGamingResultUrl(rootUrl) {
        return rootUrl + 'Proxy/VGamingResultPopup/Index';
    }

    static getJiliResultUrl(rootUrl) {
        return rootUrl + 'Proxy/JiliResultPopup/Index';
    }

    static getMGRNGResultUrl(rootUrl) {
        return rootUrl + 'Proxy/MGRNGResultPopup/Index';
    }

    static getSealEntertainmentResultUrl(rootUrl) {
        return rootUrl + 'Proxy/SealEntertainmentResultPopup/Index';
    }

    static getWELiveCasinoResultUrl(rootUrl) {
        return rootUrl + 'Proxy/WELiveCasinoResultPopup/Index';
    }

    static getLive22ResultUrl(rootUrl) {
        return rootUrl + 'Proxy/Live22ResultPopup/Index';
    }

    static getUUSlotsResultUrl(rootUrl) {
        return rootUrl + 'Proxy/UUSlotsResultPopup/Index';
    }

    static getFastSpinResultUrl(rootUrl) {
        return rootUrl + 'Proxy/FastSpinResultPopup/Index';
    }

    static getFACHAIResultUrl(rootUrl) {
        return rootUrl + 'Proxy/FACHAIResultPopup/Index';
    }

    static getNextspinResultUrl(rootUrl) {
        return rootUrl + 'Proxy/NextspinResultPopup/Index';
    }
    static getGPIResultUrl(rootUrl) {
        return rootUrl + 'Proxy/GPIResultPopup/Index';
    }

    static getONCasinoResultUrl(rootUrl) {
        return rootUrl + 'Proxy/ONCasinoResultPopup/Index';
    }

    static getSexyGamingResultUrl(rootUrl) {
        return rootUrl + 'Proxy/SexyGamingResultPopup/Index';
    }

    static getPlaytechResultUrl(rootUrl) {
        return rootUrl + 'Proxy/PlaytechResultPopup/Index';
    }

    static getGPILiveCasinoResultUrl(rootUrl) {
        return rootUrl + 'Proxy/GPILiveCasinoResultPopup/Index';
    }

    static getPlayStarResultUrl(rootUrl) {
        return rootUrl + 'Proxy/PlayStarResultPopup/Index';
    }

    static getASKMESLOTResultUrl(rootUrl) {
        return rootUrl + 'Proxy/ASKMESLOTResultPopup/Index';
    }

    static getFunkyGamesResultUrl(rootUrl) {
        return rootUrl + 'Proxy/FunkyGamesResultPopup/Index';
    }

    static getHOTDOGResultUrl(rootUrl) {
        return rootUrl + 'Proxy/HOTDOGResultPopup/Index';
    }
}