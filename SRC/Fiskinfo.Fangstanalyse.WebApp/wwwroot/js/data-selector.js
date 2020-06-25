const DISABLED = 'disabled';
const SELECTED = 'selected';
const HEADERS = {
    'accept': 'text/csv',   // TODO: API gives date in another format if requesting csv, requesting json until fixed.
    "Accept-encoding": "br"
};

class DataSelector {

    constructor(options) {
        const { domId, years } = this._options = options;
        this._years = years;
        this._months = ['Jan.', 'Feb.', 'Mars', 'Apr.', 'Mai', 'Juni', 'Juli', 'Aug.', 'Sep.', 'Okt.', 'Nov.', 'Des.'];
        this._dataSelectorElement = null;
        this._yearsElement = null;
        this._monthsElement = null;
        this._updateButton = null;
        this._selectedMonths = {};
        this._selectedYears = {};
        this._downloadCount = false;
        this._catchData = {};
        this._inCatchDataSource = {};
        this._windData = {};
        this._inWindDataSource = {};
        this._dimension = null;
        this._initializeDataSelectorElements(domId);
        this._setupButtonEvents();
        this._addItems();
        this._eventCallbacks = {};
    }

    _initializeDataSelectorElements(domId) {
        this._dataSelectorElement = document.getElementById(domId);
        this._dataSelectorElement.classList.add("fa-data-selector");
        this._dataSelectorElement.innerHTML = `
            <div class="fa-data-selector-row">
                <div class="column-text year">År</div>
                <div class="column-colon">:</div>
                <div class="column-values years">
                </div>
            </div>
            <div class="fa-data-selector-row">
                <div class="month column-text">Måneder</div>
                <div class="column-colon">:</div>
                <div class="column-values months">
                </div>
            </div>
            <div class="fa-data-selector-row buttons">
                <div class="summation-string-container">
                    <div><span class="summation-years"></span></div>
                    <div><span class="summation-months"></span></div>
                </div>
                <button class="btn btn-primary update" disabled>
                    <span class="spinner-border spinner-border-sm status" role="status" aria-hidden="true" style=""></span>
                    <div class="search-button-text">Oppdater</div>
                </button>
                <button class="btn btn-primary status-small" disabled>
                    <span class="spinner-border spinner-border-sm status" role="status" aria-hidden="true" style=""></span>
                </button>
                <button class="btn btn-primary visibility">
                    <i class="fas fa-caret-up up"></i>
                    <i class="fas fa-caret-down down"></i>
                </button>
            </div>
        `;
        this._yearsElement = this._dataSelectorElement.querySelector('.column-values.years');
        this._monthsElement = this._dataSelectorElement.querySelector('.column-values.months');
        this._updateButton = this._dataSelectorElement.querySelector('.btn-primary.update');
        this._summationYearsElement = this._dataSelectorElement.querySelector('.summation-years');
        this._summationMonthsElement = this._dataSelectorElement.querySelector('.summation-months');
    }

    _setupButtonEvents() {
        this._dataSelectorElement
            .querySelector('.fa-data-selector-row .visibility')
            .addEventListener('click', () => {
                const minimized = !this._dataSelectorElement.classList.contains('minimized');
                this._dataSelectorElement.classList.toggle("minimized", minimized);
            });
        this._dataSelectorElement
            .querySelector('.fa-data-selector-row .update')
            .addEventListener('click', this._updateData.bind(this));

    }

    _addItems() {
        this._years.forEach(value => this._addItem(this._yearsElement, value, (year, selected) => {
            this._selectedYears[year] = selected;
        }));
        this._months.forEach(value => this._addItem(this._monthsElement, value, (month, selected) => {
            this._selectedMonths[month] = selected
        }));
    }

    _addItem(parentElement, value, callback) {
        const itemElement = document.createElement('div');
        itemElement.classList.add('item');
        itemElement.innerText = value;
        itemElement.addEventListener('click', () => {
            if (this._downloading()) return;
            const selected = !itemElement.classList.contains(SELECTED);
            if (!parentElement.classList.contains(DISABLED) || !selected) {
                itemElement.classList.toggle(SELECTED, selected);
                callback(value, selected)
            }
            this._updateState();
        });
        parentElement.appendChild(itemElement);
    }

    _updateData() {
        const removeKeys = this._getKeysToRemoveFromDataSource();
        this._removeFromCatchDataSource(removeKeys);
        this._removeFromWindDataSource(removeKeys);
        if (this._downloading()) return;
        this._monthsElement.classList.add('disabled');
        this._yearsElement.classList.add('disabled');
        this._updateButton.setAttribute("disabled", "true");
        this._downloadCount = this._getDownloadCount();
        this._dataSelectorElement.classList.add('downloading');
        let currentDate = new Date();

        this.trigger("update", {
            years: this._getSelectedYears(),
            months: Object.keys(this._selectedMonths).map(key => this._months.indexOf(key) + 1)
        });

        for (const year of this._getSelectedYears()) {
            for (let month of this._getSelectedMonths()) {
                month = this._months.indexOf(month) + 1;

                // Ignore requests for future data.
                if(year === currentDate.getFullYear() && month > currentDate.getMonth()) {
                    this._downloadCount -= 1;
                    continue;
                }

                this._getCatchData(year, month, data => {
                    this._addToCatchDataSourceIfNecessary(year, month, data);
                    this._downloadCount -= 1;
                    if (!this._downloading()) {
                        this._doneDownloading();
                    }
                });

                if (year > 2013) {
                    this._getWindData(year, month, data => {
                        this._addToWindDataSourceIfNecessary(year, month, data);
                    });
                }
            }
        }
    }

    _getKeysToRemoveFromDataSource() {
        const selectedLookup = this._getSelectedLookup();
        return Object.keys(this._inCatchDataSource)
            .filter(key => !selectedLookup[key]);
    }

    _getSelectedLookup() {
        const selectedLookup = {};
        for (const year of this._getSelectedYears()) {
            for (let month of this._getSelectedMonths()) {
                month = this._months.indexOf(month) + 1;
                selectedLookup[`${year}-${month}`] = true;
            }
        }
        return selectedLookup;
    }

    _doneDownloading() {
        this._monthsElement.classList.remove('disabled');
        this._yearsElement.classList.remove('disabled');
        this._dataSelectorElement.classList.remove('downloading');
        this._updateState();
    }

    _getDownloadCount() {
        return this._getSelectedYears().length*this._getSelectedMonths().length;
    }

    _markCatchDataAsDownloaded(year, month, data) {
        const key = `${year}-${month}`;
        this._catchData[key] = data;
    }

    _markWindDataAsDownloaded(year, month, data) {
        const key = `${year}-${month}`;
        this._windData[key] = data;
    }

    _isCatchDataDownloaded(year, month) {
        const key = `${year}-${month}`;
        return !!this._catchData[key];
    }

    _isWindDataDownloaded(year, month) {
        const key = `${year}-${month}`;
        return !!this._windData[key];
    }

    _removeFromCatchDataSource(removeKeys) {
        removeKeys.forEach(key => this._inCatchDataSource[key] =false);
        this._removeFromDataSource(this._options.catchDataSource, removeKeys);
    }

    _removeFromWindDataSource(removeKeys) {
        removeKeys.forEach(key => this._inWindDataSource[key] =false);
        this._removeFromDataSource(this._options.windDataSource, removeKeys);
    }

    _removeFromDataSource(dataSource, removeKeys) {
        dataSource
            .getDataContainer()
            .getCrossfilter()
            .remove(d => {
                const date = new Date(d[4]);
                const key = `${date.getFullYear()}-${date.getMonth() + 1}`;
                return removeKeys.indexOf(key) >= 0;
            });
    }

    _addToCatchDataSourceIfNecessary(year, month, data) {
        const key = `${year}-${month}`;
        if (!this._inCatchDataSource[key]) {
            this._options.catchDataSource.addData(data);
            this._inCatchDataSource[key] = true;
        }
    }

    _addToWindDataSourceIfNecessary(year, month, data) {
        const key = `${year}-${month}`;

        if (!this._inWindDataSource[key]) {
            this._options.windDataSource.addData(data);
            this._inWindDataSource[key] = true;
        }
    }

    _getCatchData(year, month, callback) {
        if (this._isCatchDataDownloaded(year, month)) {
            const data = this._catchData[`${year}-${month}`];
            callback(data);
        } else {
            const url = this._getCatchDataUrl(year, month);

            fetch(url, {
                headers: HEADERS
            })
                .then(data => data.text())
                .then(text => {
                    this._markCatchDataAsDownloaded(year, month, text);

                    if (text.length > 0) {
                        callback(text);
                    }
                });
        }
    }

    _getCatchDataUrl(year, month) {
        let { catchDataUrl: url } = this._options;
        url = url.replace('{{year}}', year);
        url = url.replace('{{month}}', month);
        return url;
    }

    _getWindData(year, month, callback) {
        if (this._isWindDataDownloaded(year, month)) {
            const data = this._windData[`${year}-${month}`];
            callback(data);
        } else {
            const url = this._getWindDataUrl(year, month);

            fetch(url, {
                headers: HEADERS
            })
                .then(data => data.text())
                .then(text => {
                    this._markWindDataAsDownloaded(year, month, text);

                    if (text.length > 0) {
                        callback(text);
                    }

                    if (!this._downloading()) {
                        this._doneDownloading();
                    }
                });
        }

    }

    _getWindDataUrl(year, month) {
        let { windDataUrl: url } = this._options;
        url = url.replace('{{year}}', year);
        url = url.replace('{{month}}', month);
        return url;
    }

    _downloading() {
        return this._downloadCount > 0;
    }

    _shouldUpdate() {
        const selectedLookUp = this._getSelectedLookup();
        return Object.keys(selectedLookUp).filter(key => !this._inCatchDataSource[key]).length
            + this._getKeysToRemoveFromDataSource().length;
    }

    _updateState() {
        const changeCount = this._shouldUpdate();
        changeCount === 0
            ? this._updateButton.setAttribute("disabled", "true")
            : this._updateButton.removeAttribute("disabled");
        const monthsCount = this._getSelectedMonths().length;
        const yearsCount = this._getSelectedYears().length;
        let {downloadLimit} = this._options;
        if (typeof downloadLimit === 'number') {
            const limit = Math.max(downloadLimit, 1);
            this._monthsElement.classList.toggle('disabled', limit - monthsCount*yearsCount - yearsCount < 0);
            this._yearsElement.classList.toggle('disabled', limit - monthsCount*yearsCount - monthsCount < 0);
        }
        this._updateSummationString();
    }

    _updateSummationString() {
        let years = new Set();
        let months = new Set();
        Object.keys(this._inCatchDataSource).forEach(key => {
            const [year, month] = key.split('-');
            years.add(year);
            months.add(month);
        });
        years = Array.from(years);
        months = Array
            .from(months)
            .map(month => parseInt(month) - 1)
            .sort((a,b) => a - b)
            .map(month => this._months[month]);
        const totalCount = years.length*months.length;
        this._summationYearsElement.innerText = totalCount > 0 ? `År: ${this._createYearSummationString(years)}`: '';
        this._summationMonthsElement.innerText = totalCount > 0 ? `Måneder: ${months.join(', ')}` : '';
    }

    _createYearSummationString(
        years,
    ) {
        const yearsSorted = years.slice().map(numberString => parseInt(numberString)).sort();
        if (yearsSorted.length === 0) {
            return [];
        } else {
            const yearsSummation = [yearsSorted[0]];
            for (const value of yearsSorted.slice(1)) {
                const currentIndex = yearsSummation.length - 1;
                const currentValue = typeof yearsSummation[currentIndex] === 'number'
                    ? yearsSummation[currentIndex] : yearsSummation[currentIndex].slice(-1)[0];
                if (value - currentValue === 1) {
                    typeof yearsSummation[currentIndex] === 'number'
                        ? yearsSummation[currentIndex] = [yearsSummation[currentIndex], value]
                        : yearsSummation[currentIndex] = [yearsSummation[currentIndex][0], value]
                } else {
                    yearsSummation.push(value);
                }
            }
            return yearsSummation
                .map(value => typeof value === 'number' ? `${value}` : `${value[0]}-${value[1]}`).join(', ');
        }
    }

    _getSelectedYears() {
        return Object.keys(this._selectedYears).filter(key => !!this._selectedYears[key]);
    }

    _getSelectedMonths() {
        return Object.keys(this._selectedMonths).filter(key => !!this._selectedMonths[key]);
    }

    getHTMLElement() {
        return this._dataSelectorElement;
    }

    getID() {
        return "data-selector";
    }
    
    on(event, callback) {
        const callbacks = this._eventCallbacks[event] || [];
        callbacks.push(callback);
        this._eventCallbacks[event] = callbacks;
    }
    
    trigger(event, message) {
        const callbacks = this._eventCallbacks[event] || [];
        for (const callback of callbacks) {
            callback(message);
        }
    }

}