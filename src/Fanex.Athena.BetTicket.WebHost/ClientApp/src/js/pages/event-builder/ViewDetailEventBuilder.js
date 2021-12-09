import UrlDetailBuilder from '../../lib/UrlDetailBuilder';
import DataService from '../../lib/DataService';

export default function ViewDetailEventBuilder() {

    window.showMP = (transId) => {
        let divEvent = document.querySelector('#divEvent_' + transId);
        divEvent.classList.add('mx-parlay'); //to make line-through for void-parlay-ticket
        const url = UrlDetailBuilder.getMixParlayDetailUrl(this.rootUrl, this._getLanguage());
        _showMixParlayDetail.call(this, url, transId, divEvent);
    };

    window.showCombMP = (transId, index) => {
        let divEvent = document.querySelector('#divEvent_' + transId + '' + index);
        divEvent.classList.add('mx-parlay'); //to make line-through for void-parlay-ticket
        const url = UrlDetailBuilder.getMixParlayDetailUrl(this.rootUrl, this._getLanguage());
        _showMixParlayDetail.call(this, url, transId, divEvent);
    };

    // expand choice content
    window.showNewCombMP = (...args) => {
        _showNewParlayDetail.apply(this, [this.rootUrl, ...args]);
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

    window.showColossusBetsDetails = (refNo, winlostDate, transId, custId, betCheck) => {
        const url = UrlDetailBuilder.getColossusBetsDetailUrl(this.rootUrl, _getCommonParams.call(this), refNo, winlostDate, custId, betCheck);
        const title = this._getResource('TicketDetailOf').replace('{0}', refNo);

        this._openPopup(url, title);
    }

    window.showGlobalGaming = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getGlobalGamingDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductFW') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showPPDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getPPDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductPP') + ' - ' +
            this._getResource('DetailedBetlist') + ' - ' + this._formatDateString(winlostDate);

        this._openPopup(url, title);
    }

    window.showPPJackpotWonDetails = (transId, custId, betTypeId) => {
        const url = UrlDetailBuilder.getPPJackpotWonDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, custId, betTypeId);
        const title = this._getResource('ProductPP') + ' - ' +
            this._getResource('JackpotWonDetails');

        this._openPopup(url, title);
    }

    window.showSabaClubJackpotWonDetails = (transId, custId, betTypeId) => {
        const url = UrlDetailBuilder.getSabaClubJackpotWonDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, custId, betTypeId);
        const title = this._getResource('ProductSBC') + ' - ' +
            this._getResource('JackpotWonDetails');

        this._openPopup(url, title);
    }

    window.showVGamingJackpotWonDetails = (transId, custId, betTypeId) => {
        const url = UrlDetailBuilder.getVGamingJackpotWonDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, custId, betTypeId);
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

    window.showGDGBetDetails = (transId, winlostDate, custId, transType) => {
        const url = UrlDetailBuilder.getGDGDetailUrl(this.rootUrl, _getCommonParams.call(this), transId, winlostDate, custId, transType);
        const title = this._getResource('ProductGDG') + ' - ' +
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
}

function _showMixParlayDetail(url, transId, divEvent) {
    if (divEvent.style.display === 'none') {
        this._beforeShow();
        DataService.postData(url, { transId }, this._getAccessToken())
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

function _showNewParlayDetail(rootUrl, transId, transIndex, refNo, handicap1, handicap2, betcheck, betteam, custId) {
    let divEvent = document.querySelector('#divEvent_' + transId + '' + transIndex);
    let nameDiv = document.querySelector('#combinationOfferName_' + transId + transIndex);

    divEvent.classList.add('mx-parlay'); //to make line-through for void-parlay-ticket

    if (divEvent.style.display == 'none') {
        const url = UrlDetailBuilder.getChoiceParlayDetailUrl(rootUrl, this._getLanguage());
        this._beforeShow();
        DataService.postData(url, { transId, handicap1, handicap2, betcheck, betteam, custId }, this._getAccessToken())
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