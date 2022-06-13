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

    static _getSportBookResultUrl(rootUrl, matchid, bettype, raceNo, sporttype, league, isOutright, extension1, extension2) {
        let url = rootUrl + 'SportsResult/Index';
        url = QueryString.setParam(url, 'matchid', matchid);
        url = QueryString.setParam(url, 'bettype', bettype);
        url = QueryString.setParam(url, 'raceno', raceNo);
        url = QueryString.setParam(url, 'sporttype', sporttype);
        url = QueryString.setParam(url, 'isoutright', isOutright);

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

        let isParlay = bettype === BetType.MIXPARLAY || bettype === BetType.SYSTEMPARLAY || bettype === BetType.MIXPARLAY_BETRADAR;
        let url = isParlay
            ? this._getMixParlayResultUrl(rootUrl, refnoMixParlay, winlostdate, bettype)
            : this._getSportBookResultUrl(rootUrl, matchid, bettype, raceNo, sporttype, league, isOutright, extension1, extension2, custid);

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

    static getSexyGamingResultUrl(rootUrl, transdesc, matchCode, bettype, language, accessToken) {
        let url = rootUrl + 'NonSportsResult/SexyGaming';

        url = QueryString.setParam(url, 'language', language);
        url = QueryString.setParam(url, 'access_token', accessToken);
        url = QueryString.setParam(url, 'matchCode', matchCode);
        url = QueryString.setParam(url, 'bettype', bettype);
        url = QueryString.setParam(url, 'transdesc', encodeURIComponent(transdesc));

        return url;
    }

    static getSexyGamingResultUrl(rootUrl, transdesc, matchCode, bettype, language, accessToken) {
        let url = rootUrl + 'NonSportsResult/SexyGaming';

        url = QueryString.setParam(url, 'language', language);
        url = QueryString.setParam(url, 'access_token', accessToken);
        url = QueryString.setParam(url, 'matchCode', matchCode);
        url = QueryString.setParam(url, 'bettype', bettype);
        url = QueryString.setParam(url, 'transdesc', encodeURIComponent(transdesc));

        return url;
    }

    static getGlobalGamingResultUrl(rootUrl) {
        return rootUrl + 'Proxy/GlobalGamingResultPopup/Index';
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

    static getGDGResultUrl(rootUrl) {
        return rootUrl + 'Proxy/GDGResultPopup/Index';
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
}