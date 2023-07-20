import QueryString from '@jsroot/lib/QueryString'
import BetType from '../constants/BetType';

export default class UrlDetailBuilder {
    static _assignCommonParams(url, { baseCurrency, language, accessToken }) {
        url = QueryString.setParam(url, 'baseCurrency', baseCurrency);
        url = QueryString.setParam(url, 'language', language);
        url = QueryString.setParam(url, 'access_token', accessToken);
        return url;
    }

    static getMixParlayDetailUrl(rootUrl, language) {
        let url = rootUrl + 'Proxy/MixParlayBetList/Detail';
        url = QueryString.setParam(url, 'language', language);
        return url;
    }

    static buildChoiceMixParlayDetail(data) {
        let choice = '';
        const length = data.length;
        for (let i = 0; i < length; i++) {
            choice += data[i];
            if (i < length - 1) {
                choice += '<div class="line"></div>';
            }
        }
        return choice;
    }

    static getChoiceParlayDetailUrl(rootUrl, language) {
        let url = rootUrl + 'Proxy/MixParlayBetList/NewParlayDetail';
        url = QueryString.setParam(url, 'language', language);
        return url;
    }

    static buildChoiceParlayDetail(response) {
        let choice = '';
        let length = response.Data === null ? 0 : response.Data.length;
        if (length > 0) {
            for (let i = 0; i < response.Texts.length; i++) {
                choice += response.Texts[i];
            }

            choice += '<div class="line"></div>';

            for (let i = 0; i < response.Data.length; i++) {
                let item = response.Data[i];
                choice += item;
                if (i < length - 1) {
                    choice += '<div class="line"></div>';
                }
            }
        }

        return choice;
    }

    static buildSingleMatchParlayDetail(data) {
        let selection = '';
        let length = data === null ? 0 : data.length;
        if (length > 0) {
            selection = '<div class="single-match-parlay-detail">';
            for (let i = 0; i < data.length; i++) {
                selection += data[i];
            }
            selection += '</div>';
        }

        return selection;
    }

    static getSingleMatchParlayDetailUrl(rootUrl, language, transId) {
        if (Number(transId) === 0) { return; }

        let url = rootUrl + 'Proxy/SingleMatchParlayBetList/Detail';
        url = QueryString.setParam(url, 'language', language);
        return url;
    }

    static getParlayDetailUrl(rootUrl, commonParams, transId, refNo, betId, custId, betTeam) {
        if (Number(transId) === 0) { return; }
        let url = rootUrl + 'SystemParlayBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'refno', refNo);
        url = QueryString.setParam(url, 'betteam', betTeam);
        return url;
    }

    static getCashOutHistoryPopupSetting(rootUrl, commonParams, transId, refNo, betType, betdaqId) {
        const isCashOut = betdaqId < 1 || betType === BetType.CASHOUT;
        let url = rootUrl + 'CashOutBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'refNo', refNo);
        url = QueryString.setParam(url, 'betType', betType);
        url = QueryString.setParam(url, 'betdaqId', betdaqId);
        url = QueryString.setParam(url, 'isCashout', isCashOut);
        return {
            url: url,
            isCashOut: isCashOut
        };
    }

    static getNewCashOutHistoryPopupSetting(rootUrl, commonParams, transId, custId, winlostDate, currency) {
        let url = rootUrl + 'NewCashOutBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'currency', currency);
        return url;
    }

    static getBonusDetailUrl(rootUrl, commonParams, refCode, transId) {
        let url = rootUrl + 'BonusBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'RefCode', refCode);
        url = QueryString.setParam(url, 'TransId', transId);

        return url;
    }

    static getAGCasinoDetailUrl(rootUrl, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'AGCasinoBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transid', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custid', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getRnGCasinoDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'RnGCasinoBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getVoidBridgeDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'VoidbridgeBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getAllBetDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'AllBetBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transid', transId);
        url = QueryString.setParam(url, 'custid', custId);
        url = QueryString.setParam(url, 'transType', transType);
        url = QueryString.setParam(url, 'date', winlostDate);

        return url;
    }

    static getPPDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'PragmaticPlayBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getPPJackpotWonDetailUrl(rootUrl, commonParams, transId, custId, betTypeId, winlostDate) {
        let url = rootUrl + 'JackpotWonBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'bettype', betTypeId);
        url = QueryString.setParam(url, 'date', winlostDate);

        return url;
    }

    static getSabaClubJackpotWonDetailUrl(rootUrl, commonParams, transId, custId, betTypeId, winlostDate) {
        let url = rootUrl + 'SabaClubJackpotWonBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'bettype', betTypeId);
        url = QueryString.setParam(url, 'date', winlostDate);

        return url;
    }

    static getSGJackpotWonDetailUrl(rootUrl, commonParams, transId, custId, betTypeId, winlostDate) {
        let url = rootUrl + 'JackpotWonBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'bettype', betTypeId);
        url = QueryString.setParam(url, 'date', winlostDate);

        return url;
    }

    static getVideoPokerGameDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'CardClubBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getSGDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'SGBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getJokerDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'JokerBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getSexyGamingDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'SexyGamingBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getSAGamingDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'SAGamingBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getSBCDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'SabaClubBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getBGGDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'BGGBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getPGSDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'PGSoftBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getHabaneroDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'HabaneroBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getWMDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'WMBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getVGamingDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'VGamingBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getVGamingJackpotWonDetailUrl(rootUrl, commonParams, transId, custId, betTypeId, winlostDate) {
        let url = rootUrl + 'VGamingJackpotWonBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'bettype', betTypeId);
        url = QueryString.setParam(url, 'date', winlostDate);

        return url;
    }

    static getCGDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'CreativeGamingBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getAPDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'AdvantPlayBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getAPMDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'AdvantPlayMiniBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getFGGDetailUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'FGGBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getMaxGameDetaillUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'MaxGameBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getJiliDetaillUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'JiliBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getYeeBetDetaillUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'YeeBetBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }

    static getMGRNGDetaillUrl(rootUrl, commonParams, transId, winlostDate, custId, transType) {
        let url = rootUrl + 'MGRNGBetListDetail/Index';
        url = this._assignCommonParams(url, commonParams);
        url = QueryString.setParam(url, 'transId', transId);
        url = QueryString.setParam(url, 'date', winlostDate);
        url = QueryString.setParam(url, 'custId', custId);
        url = QueryString.setParam(url, 'transType', transType);

        return url;
    }
}