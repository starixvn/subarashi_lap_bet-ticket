import UrlDetailBuilder from '../../lib/UrlDetailBuilder';
import DataService from '../../lib/DataService';

export default function ViewDetailEventBuilder() {
    window.showMP = (transId) => {
        let divEvent = document.querySelector('#divEvent_' + transId);
        divEvent.classList.add('mx-parlay'); //to make line-through for void-parlay-ticket
        const url = UrlDetailBuilder.getMixParlayDetailUrl(this.rootUrl, this._getLanguage());
        _showMixParlayDetail.call(this, url, transId, divEvent, this.isHideSaba);
    };

    window.showCombMP = (transId, index) => {
        let divEvent = document.querySelector('#divEvent_' + transId + '' + index);
        divEvent.classList.add('mx-parlay'); //to make line-through for void-parlay-ticket
        const url = UrlDetailBuilder.getMixParlayDetailUrl(this.rootUrl, this._getLanguage());
        _showMixParlayDetail.call(this, url, transId, divEvent, this.isHideSaba);
    };

    // expand choice content
    window.showNewCombMP = (...args) => {
        _showNewParlayDetail.apply(this, [this.rootUrl, this.isHideSaba, ...args]);
    };

    // expand choice content
    window.showSingleMatchParlayDetail = (...args) => {
        _showSingleMatchParlayDetail.apply(this, [this.rootUrl, ...args]);
    };

    // show detail popup (in choice column)
    window.showCombMPDetail = (transId, refNo, betId, custId, betTeam) => {
        const url = UrlDetailBuilder.getParlayDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, refNo, betId, custId, betTeam);
        this._openPopup(url, betTeam);
    };

    window.ViewCashOutHistory = (transId, refNo, betType, betdaqId) => {
        const { url, isCashOut } = UrlDetailBuilder.getCashOutHistoryPopupSetting(this.rootUrl, _getCommonParams.call(this), transId, refNo, betType, betdaqId);
        const title = this._getResource(isCashOut ? 'CashOutDetails' : 'BoughtDetails');
        this._openPopup(url, title);
    }

    window.ViewNewCashOutHistory = (transId, custId, winlostDate, currency) => {
        const url = UrlDetailBuilder.getNewCashOutHistoryPopupSetting(this.rootUrl, _getCommonParams.call(this), transId, custId, winlostDate, currency);
        const title = this._getResource('CashOutHistory');
        this._openPopup(url, title);
    };

    window.ShowBonusDetails = (refCode, transId) => {
        const url = UrlDetailBuilder.getBonusDetailUrl(this.rootUrl, _getCommonParams.call(this), refCode, transId);
        const title = this._getResource('TransactionDetail');
        this._openPopup(url, title);
    }

    window.showAGCasinoDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getAGCasinoDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('AGCasino') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);
        this._openPopup(url, title);
    }

    window.showRnGCasinoDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getRnGCasinoDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductCS') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showVoidBridgeDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getVoidBridgeDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductVB') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showTodayRunningBetsDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getAllBetDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductAB') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showPPDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getPPDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductPP') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showPPJackpotWonDetails = (transId, custId, betTypeId, winlostDate) => {
        const url = UrlDetailBuilder.getPPJackpotWonDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, custId, betTypeId, winlostDate);
        const title = this._getResource('ProductPP') + ' - ' +
            this._getResource('JackpotWonDetails');

        this._openPopup(url, title);
    }

    window.showSabaClubJackpotWonDetails = (transId, custId, betTypeId, winlostDate) => {
        const url = UrlDetailBuilder.getSabaClubJackpotWonDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, custId, betTypeId, winlostDate);
        const title = this._getResource('ProductSBC') + ' - ' +
            this._getResource('JackpotWonDetails');

        this._openPopup(url, title);
    }

    window.showSGJackpotWonDetails = (transId, custId, betTypeId, winlostDate) => {
        const url = UrlDetailBuilder.getSGJackpotWonDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, custId, betTypeId, winlostDate);
        const title = this._getResource('ProductSG') + ' - ' +
            this._getResource('JackpotWonDetails');

        this._openPopup(url, title);
    }

    window.showVGamingJackpotWonDetails = (transId, custId, betTypeId, winlostDate) => {
        const url = UrlDetailBuilder.getVGamingJackpotWonDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, custId, betTypeId, winlostDate);
        const title = this._getResource('ProductVGA') + ' - ' +
            this._getResource('JackpotWonDetails');

        this._openPopup(url, title);
    }

    window.showVideoPokerGameDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getVideoPokerGameDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductCC') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showSGDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getSGDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductSG') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showJokerBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getJokerDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductJK') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showSexyGamingBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getSexyGamingDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductSXG') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showSAGamingDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getSAGamingDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductSAG') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showSabaClubDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getSBCDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductSBC') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showPGSoftDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getPGSDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductPGS') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showHabaneroBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getHabaneroDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductHB') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showWMBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getWMDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductWM') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showBGBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getBGGDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductBGG') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showVGamingDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getVGamingDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductVGA') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showCGBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getCGDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductCG') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showAdvantPlayBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getAPDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductAP') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showAdvantPlayMiniBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getAPMDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductAPM') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showFGGBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getFGGDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductFGGL') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showMaxGameBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getMaxGameDetaillUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductMXG') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showJiliBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getJiliDetaillUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductJIG') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showYeeBetBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getYeeBetDetaillUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductYB') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showMGRNGCasinoBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getMGRNGDetaillUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductMGRNG') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showSealEntertainmentBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getSealEntertainmentDetaillUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductSEA') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showWELiveCasinoBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getWELiveCasinoDetaillUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductWE') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showWELiveCasinoBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getWELiveCasinoDetaillUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductWE') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showLive22BetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getLive22DetaillUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductL22') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showUUSlotsBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getUUSlotsDetaillUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductUUSL') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showFastSpinBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getFastSpinDetaillUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductFS') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showFACHAIBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getFACHAIDetaillUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductFAC') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showNextspinBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getNextspinDetaillUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductNS') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showONLiveCasinoBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getONCasinoDetaillUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductON') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showDetail = (id) => {
        var e = document.getElementById(id);
        if (e.style.display == 'block') {
            e.style.display = 'none';
        }
        else {
            e.style.display = 'block';
        }
    }
}

function _showMixParlayDetail(url, transId, divEvent, isHideSaba) {
    if (divEvent.style.display === 'none') {
        this._beforeShow();
        DataService.postData(url, { transId, isHideSaba }, this._getAccessToken())
            .then((result) => {
                const choice = UrlDetailBuilder.buildChoiceMixParlayDetail(result.data, divEvent);
                divEvent.innerHTML = choice;
                divEvent.style.display = 'block';
                this._afterShow();
            }).catch((error) => {
                this._afterShow();
                console.log(error);
            });
    } else {
        divEvent.style.display = 'none';
        divEvent.innerHTML = '';
    }
}

function _showNewParlayDetail(rootUrl, isHideSaba, transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam, custId) {
    let divEvent = document.querySelector('#divEvent_' + transId + '' + transIndex);
    let nameDiv = document.querySelector('#combinationOfferName_' + transId + transIndex);

    divEvent.classList.add('mx-parlay'); //to make line-through for void-parlay-ticket

    if (divEvent.style.display == 'none') {
        const url = UrlDetailBuilder.getChoiceParlayDetailUrl(rootUrl, this._getLanguage());
        this._beforeShow();
        DataService.postData(url, { transId, handicap1, handicap2, betcheck, betteam, custId, isHideSaba }, this._getAccessToken())
            .then((result) => {
                const choice = UrlDetailBuilder.buildChoiceParlayDetail(result.data);
                divEvent.innerHTML = choice;
                divEvent.style.display = 'block';
                nameDiv.style.display = 'none';

                this._afterShow();
            }).catch((error) => {
                this._afterShow();
                console.log(error);
            });
    }
    else {
        divEvent.style.display = 'none';
        divEvent.innerHTML = '';
        nameDiv.style.display = 'block';
    }
}

function _showSingleMatchParlayDetail(rootUrl, transId, custId, status, transIndex) {
    const divEvent = document.querySelector('#divEvent_' + transId + '' + transIndex);

    if (divEvent.style.display == 'none') {
        const url = UrlDetailBuilder.getSingleMatchParlayDetailUrl(rootUrl, this._getLanguage(), transId);
        this._beforeShow();
        DataService.postData(url, { transId, custId, status }, this._getAccessToken())
            .then((result) => {
                const selection = UrlDetailBuilder.buildSingleMatchParlayDetail(result.data);

                divEvent.innerHTML = selection;
                divEvent.style.display = 'block';

                this._afterShow();
            }).catch(function (error) {
                console.log(error);
            });
    } else {
        divEvent.style.display = 'none';
        divEvent.innerHTML = '';
    }
}

function _getCommonParams() {
    return {
        baseCurrency: this._getBaseCurrency(),
        language: this._getLanguage(),
        accessToken: this._getAccessToken()
    }
}