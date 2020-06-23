const BASE_CATCH_DATA_URL = "https://fangstanalyse.no:47003/optimizedcatchdata/getcatchdatainteroperable";
const BASE_WIND_URL = "https://fangstanalyse.no:47003/ClimateAndWeatherData/getwinddata";
const DETAILED_CATCH_DOWNLOAD_BASE_URL = "https://fangstanalyse.no:47003/optimizedcatchdata/getdetailedcatchdatainteroperable?";
const N = 36;
const MONTHS_DOWNLOAD_LIMIT = 36;
const months = ['Jan.', 'Feb.', 'Mars', 'Apr.', 'Mai', 'Juni', 'Juli', 'Aug.', 'Sep.', 'Okt.', 'Nov.', 'Des.'];
const monthsFull = ["Januar", "Februar", "Mars", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Desember"];
const artLookup = {"2511": "Krill", "414": "Annen hå", "61106": "Sild", "1013": "Mora", "1035": "Polartorsk", "102703": "Hyse", "1062": "Skolest", "1036": "Øyepål", "2899": "Annen tang og tare", "169520": "Blåstål/ Rødnebb", "435": "Svarthå", "2019": "Makrellstørje", "2354": "Piggvar", "102202": "Torsk", "2699": "Annet bløtdyr", "3211": "Laksestørje", "743": "Strømsild", "2317": "Sandflyndre", "2628": "Albuskjell", "2812": "Sukkertare", "1811": "Tobis og annen sil", "253420": "Kongekrabbe", "2919": "Annen marin fisk", "2528": "Hestereke", "599": "Annen skate og rokke", "6211": "Black oreo *", "1022": "Torsk", "1311": "Beryx - alfonsinos", "512": "Piggskate", "253210": "Taskekrabbe", "715": "Annen ørret", "2541": "Sjøkreps", "2999": "Annen marin fisk", "253220": "Taskekrabbe", "1321": "Orange roughy", "716": "Røye", "2318": "Lomre", "1099": "Annen torskefisk", "2512": "Raudåte", "2314": "Smørflyndre", "2619": "Kuskjell", "2629": "Annen kammusling", "2524": "Dypvannsreke", "2532": "Taskekrabbe", "2638": "Blekksprut uspes.", "2635": "Åtte-armet blekksprut uspes.", "1032": "Sei", "2626": "Haneskjell", "3511": "Månefisk", "1651": "Mulle", "1912": "Dolkfisk/trådstjert", "2211": "Knurr uspes.", "1024": "Blålange", "222110": "Rognkjeks (felles)", "1040": "Sølvtorsk", "1039": "Hvitting", "1696": "Gressgylt", "2534": "Kongekrabbe", "2621": "Stort kamskjell (Kamskjell)", "516": "Spisskate", "522": "Spinytail skate *", "742": "Vassild", "2827": "Sauetang", "1511": "Annen multe", "1695": "Blåstål/ Rødnebb", "61103": "Sild", "614": "Sardin", "2651": "Kongsnegl (Kongesnegl)", "1030": "Sypike", "1691": "Berggylt", "311": "Blåhai", "412": "Pigghå", "431": "Dypvannshå", "2311": "Kveite", "102701": "Hyse", "1623": "Havabbor", "6159": "Tannfisk - uspes.", "611": "Sild", "61104": "Sild", "2611": "Østers", "2341": "Tunge", "2212": "Knurr", "1713": "Blåsteinbit", "3111": "Havmus", "2890": "Grisetangdokke", "1692": "Annen leppefisk", "2205": "Blåkjeft", "2411": "Breiflabb", "1012": "Blå antimora", "2627": "Saueskjell", "2624": "O-skjell", "2650": "Strandsnegl", "2351": "Glassvar", "211": "Håbrann", "6329": "Patagonsk torsk - uspes", "2623": "Blåskjell", "553": "Hvitskate", "717": "Annen røye", "312": "Hågjel", "6299": "Oreo dories nei*", "615": "Brisling", "6411": "Pink cusk-eel*", "1052": "Silver hake *", "1693": "Bergnebb", "1711": "Gråsteinbit", "2537": "Strandkrabbe", "102704": "Hyse", "61101": "Sild", "2513": "Antarktisk krill", "1061": "Isgalt", "2631": "Vanlig ti-armet blekksprut uspes", "2548": "Krinakrabbe", "2542": "Hummer", "6155": "Humped rockcod*", "2820": "Grisetang", "2634": "Akkar", "524": "Sandskate", "1719": "Steinbiter", "222120": "Rognkjeks (felles)", "2612": "Stillehavsøsters", "2841": "Havsalat", "2349": "Annen tunge", "2315": "Gapeflyndre", "1211": "Trompetfisk", "2620": "Sandskjell", "528": "Burton-skate", "2011": "Stripet pelamide", "641": "Laksesild", "321": "Gråhai", "2832": "Søl", "2535": "Trollkrabbe", "1694": "Grønngylt", "1011": "Mora uspes.", "102201": "Torsk", "811": "Ål", "61105": "Sild", "2539": "Annen krabbe", "61107": "Sild", "434": "Stor svarthå", "713": "Ørret (oppdrett)", "2811": "Andre brunalger", "2643": "Andre pigghuder", "1025": "Skjellbrosme", "6169": "Isfisk - uspes.", "2051": "Sverdfisk", "1029": "Hvit lysing", "499": "Annen hai", "711": "Laks", "2622": "Harpeskjell", "2536": "Snøkrabbe", "2560": "Fjærerur", "3611": "Nordlig lysprikkfisk", "102204": "Torsk", "1421": "Villsvinfisk", "1023": "Lange", "2313": "Blåkveite", "511": "Storskate", "1027": "Hyse", "1611": "Hestmakrell", "2312": "Rødspette", "75101": "Lodde", "2640": "Kråkebolle", "61502": "Brisling", "2329": "Annen flyndre", "432": "Gråhå", "911": "Horngjel", "2201": "Uer uspes.", "2221": "Rognkjeks (felles)", "1051": "Lysing", "253410": "Kongekrabbe", "2352": "Slettvar", "2013": "Makrell", "2319": "Skrubbe", "2813": "Stortare", "2826": "Knapptang", "2203": "Snabeluer", "1712": "Flekksteinbit", "430": "Brunhå", "2637": "Annen ti-armet blekksprut", "75102": "Lodde", "520": "Kloskate", "6332": "Grenadiers nei*", "1021": "Brosme", "1411": "Sanktpetersfisk", "3115": "Brun havmus", "621": "Ansjos", "2214": "Rødknurr", "821": "Havål", "1038": "Kolmule", "212": "Brugde", "1034": "Lyr", "61501": "Brisling", "1671": "Havbrasme", "6152": "Antarktisk tannfisk", "741": "Strømsild/Vassild", "2202": "Uer (vanlig)"};
const styleLookup = generateStyleLookup();
const years = getYears();
const qualityLookup = {
    "10": "Ekstra",
    "11": "Prima",
    "12": "Superior",
    "20": "A",
    "21": "Blank",
    "30": "B",
    "31": "Sekunda",
    "32": "Afrika",
    "33": "Frostskadet",
    "34": "Gul",
    "35": "Produksjonsrogn",
    "36": "Knekt krabbe",
    "37": "Blaut krabbe",
    "40": "Skadd",
    "41": "Offal",
    "42": "Vrak",
    "99": "Uspesifisert"
};
const redskapLookup = {
    9: "Oppdrett/uspesifisert",
    8: "Andre redskap",
    7: "Harpun/kanon",
    6: "Snurrevad",
    5: "Trål",
    4: "Bur og ruser",
    3: "Krokredskap",
    2: "Garn",
    1: "Not"
};
const lengdeLookup = {
    5: "28m+",
    4: "21-27,9m",
    3: "15-20,9m",
    2: "11-14,9m",
    1: "Under 11m",
    0: "Ukjent"
};

let currentSelectedYears = "";
let currentSelectedMonths ="";
let nextSelectedYears = "";
let nextSelectedMonths = "";
let styleOpacity = {};
let fangstfeltFeatureLookup = {};

function getFangstFeltKey(value) {
    if (value.length > 4) {
        value = value.substr(value.length - 4);
    }
    return +value;
}

function generateStyleLookup() {
    const style = {};
    const scale = chroma.scale(Sintium.colorScheme.SPECTRAL.chromaColors);
    for (let i = 0; i <= N; i++) {
        const color = scale(i/N).rgba();
        color[3] = 0.5;
        style[i] = new ol.style.Style({
            fill: new ol.style.Fill({
                color: color
            }),
            text: new ol.style.Text({})
        });
    }
    style[0] = null;
    return style;
}

function getYears() {
    let tmp = [];

    for (var i = 2000; i <= new Date().getFullYear(); i++) {
        tmp.push(i);
    }

    return tmp;
}

function formattedDate(date) {
    const year = date.getFullYear();
    const month = months[date.getMonth()];
    return month + " " + year;
}

const lookupErrors = {};

function mapReducedDataToFeature(feature, fangstFeltKode) {
    const code = getFangstFeltKey(fangstFeltKode);
    const coordinates = fangstfeltFeatureLookup[code];
    if (!!coordinates) {
        feature.setGeometry(new ol.geom.Polygon(coordinates));
    } else {
        lookupErrors[code] = fangstFeltKode;
    }
    return feature;
}

function maxLength(string, maxLength) {
    if (string.length > maxLength) {
        return string.slice(0, maxLength) + "..";
    }
    return string;
}

function styleFunction(feature, key) {
    const styleKey = Math.ceil((styleOpacity[key] || 0) * N);

    if (styleLookup[styleKey] != null) {
        styleLookup[styleKey].getText().setText(feature.getFeatureKey());
    }

    return styleLookup[styleKey];
}

function onMapReduceLayerDataChange(data) {
    const values = Object.values(data)
        .filter(dataPoint => {
            const firstRecord = dataPoint.records[0];
            if (!firstRecord) return false;
            const key = getFangstFeltKey(firstRecord[2]);
            return !!fangstfeltFeatureLookup[key]
        }).map(dataPoint => dataPoint.sum.rundvekt);
    const max = Math.max(...values);
    let total = 0;
    styleOpacity = {};
    Object.keys(data)
        .forEach(function(key) {
            total += data[key].sum.rundvekt;
            styleOpacity[key] = data[key].sum.rundvekt/max;
        });
    legendControl.setMax(total);
}

function updateSearchButtonStatus() {
    const buttonHTMLElement = document.querySelector(".search-button-container button");
    if (!nextSelectedYears || !nextSelectedMonths || nextSelectedYears === currentSelectedYears && nextSelectedMonths === currentSelectedMonths) {
        buttonHTMLElement.setAttribute("disabled", "true");
    } else {
        buttonHTMLElement.removeAttribute("disabled")
    }
}

function setCurrentDates(nextSelectedYears, nextSelectedMonths) {
    currentSelectedYears = nextSelectedYears;
    currentSelectedMonths = nextSelectedMonths;
    updateSearchButtonStatus();
}

function createCoordinateLookup(json) {
    for (const feature of (new ol.format.GeoJSON()).readFeatures(json)) {
        const key = getFangstFeltKey(feature.get('lok'));
        fangstfeltFeatureLookup[key] = [feature.getGeometry().getCoordinates()[0].map(coord => ol.proj.fromLonLat(coord))];
    }
}

const catchDataSource = Sintium.dataSource({
    useThread: true,
    useCrossfilter: true,
    csvHasHeader: true
});

const windDataSource = Sintium.dataSource({
    useThread: true,
    useCrossfilter: true,
    csvHasHeader: true
});

const legendControl = Sintium.legendControl({
    colorScheme: Sintium.colorScheme.SPECTRAL,
    min: 0,
    max: 100000,
    size: 300,
    position: "top-left",
    flow: "vertical"
});

let test;

fetch("/data/fangstfelt.json")
    .then(result => result.json())
    .then(json => {
        createCoordinateLookup(json);

        catchDataSource.addColumn("year", record => {
            const date = record.get("dato");
            if (record.get("dato") !== undefined) {
                return date.substring(0, 4);
            } else {
                return null;
            }
        });

        const topElement = new TopElement();

        const playWidget = Sintium.playWidget({
            dataSource: catchDataSource,
            keyColumn: "dato",
            keyUnits: "distinct",
            dateIsText: true
        });

        const playWidgetControl = Sintium.widgetControl({
            widget: playWidget,
            position: "bottom-center",
            width: "90%",
            height: 80,
            customStyle: true
        });

        // CHART
        
        const temperatureChart = Sintium.boxPlot({
            domId: "box-plot",
            dataSource: catchDataSource,
            xAxis: function(record) {
                return Date.parse(record.get("dato"));
            },
            xUnits: "distinct",
            xAxisTickFormat: function (v) {
                const date = new Date(v);
                return formattedDate(date);
            },
            yAxis: "temperatur",
            brushOn: false,
            lines: 'both',
            centerBar: true,
            rotateLabels: true,
            padding: [0.5, 0.5],
            elasticY: true,
            margins: [10, 15, 50, 30],
            translateLabels: [-20, 20]
        });

        const airPressureChart = Sintium.boxPlot({
            domId: "air-pressure",
            dataSource: catchDataSource,
            xAxis: function(record) {
                return Date.parse(record.get("dato"));
            },
            xUnits: "distinct",
            xAxisTickFormat: function (v) {
                const date = new Date(v);
                return formattedDate(date);
            },
            yAxis: "lufttrykk",
            brushOn: false,
            lines: 'both',
            centerBar: true,
            rotateLabels: true,
            padding: [0.5, 0.5],
            elasticY: true,
            margins: [10, 15, 50, 30],
            translateLabels: [-20, 20],
        });

        const toolsChart = Sintium.pieChart({
            domId: "redskap",
            dataSource: catchDataSource,
            keyColumn: "redskapkode",
            valueColumn: "rundvekt",
            legend: true,
            labels: true,
            labelFunction: function (key) {
                if (!redskapLookup[key]) {
                    return null;
                }
                return maxLength(redskapLookup[key], 18);
            },
            cap: 11,
            othersLabel: "Andre",
            marginLeft: 110,
            titleFunction: function (d) {
                return `${redskapLookup[d.key]}: ${d.value} kg`;
            },
            colorScheme: Sintium.colorScheme.BASIC
        });

        const speciesChart = Sintium.pieChart({
            domId: "art",
            dataSource: catchDataSource,
            keyColumn: "art",
            valueColumn: "rundvekt",
            legend: true,
            labels: true,
            labelFunction: function(key) {
                if (!artLookup[key]) {
                    return null;
                }
                return maxLength(artLookup[key], 18);
            },
            cap: 11,
            othersLabel: "Andre",
            marginLeft: 110,
            titleFunction: function (d) {
                return `${artLookup[d.key]}: ${d.value} kg`;
            },
            colorScheme: Sintium.colorScheme.SPECTRAL
        });

        const vesselSizeChart = Sintium.barChart({
            domId: "lengde",
            dataSource: catchDataSource,
            xAxis: "lengdegruppe",
            xUnits: "distinct",
            xAxisTickFormat: function(v) {
                return lengdeLookup[v];
            },
            brushOn: false,
            centerBar: true,
            yAxis: "rundvekt",
            yTickFormatShort: true,
            elasticX: true,
            elasticY: true,
            useCanvas: false,
            stackColumn: "year",
            titleFunction: function (d) {
                let title = lengdeLookup[d.key];
                let keys = Object.keys(d.value);

                for (let i = 0, len = keys.length; i < len; i++) {
                    title += `\n${keys[i]}: ${d.value[keys[i]]} kg`;
                }

                return title;
            },
            clickable: true,
            legend: true,
            margins: [10, 20, 10, 80],
            colorScheme: Sintium.colorScheme.BASIC
        });

        const monthsChart = Sintium.barChart({
            domId: "months-chart",
            dataSource: catchDataSource,
            xAxis: function(record) {
                return +record.get("dato").substring(5, 7) - 1;
            },
            xAxisTickFormat: function (v) {
                return months[v];
            },
            xUnits: "distinct",
            yAxis: "rundvekt",
            yTickFormatShort: true,
            elasticX: true,
            elasticY: true,
            useCanvas: false,
            stackColumn: "year",
            titleFunction: function (d) {
                let title = `${d.key} ${d.layer}`;
                let keys = Object.keys(d.value);

                for (let i = 0, len = keys.length; i < len; i++) {
                    title += `\n${keys[i]}: ${d.value[keys[i]]} kg`;
                }

                return title;
            },
            brushOn: false,
            centerBar: true,
            legend: true,
            margins: [10, 20, 10, 80],
            colorScheme: Sintium.colorScheme.BASIC
        });

        const qualityChart = Sintium.barChart({
            domId: "kvalitet",
            dataSource: catchDataSource,
            xAxis: "dato",
            xAxisTickFormat: function (v) {
                const date = new Date(v);
                return formattedDate(date);
            },
            xUnits: "distinct",
            yAxis: "rundvekt",
            yTickFormatShort: true,
            elasticX: true,
            elasticY: true,
            useCanvas: false,
            stackColumn: "kvalitetkode",
            labelFunction: function (key) {
                if (!qualityLookup[key]) {
                    return null;
                }
                return maxLength(qualityLookup[key], 18);
            },
            titleFunction: function (d) {
                let title = formattedDate(new Date(d.key));
                let keys = Object.keys(d.value);
                for (let i = 0, len = keys.length; i < len; i++) {
                    title += `\n${qualityLookup[keys[i]]}: ${d.value[keys[i]]} kg`;
                }

                return title;
            },
            brushOn: false,
            centerBar: true,
            legend: true,
            margins: [10, 20, 10, 100],
            colorScheme: Sintium.colorScheme.BASIC
        });

        const timeLineBarChart = Sintium.barChart({
            domId: "timeline",
            dataSource: catchDataSource,
            xAxis: "dato",
            xUnits: "distinct",
            yAxis: "rundvekt",
            xAxisTickFormat: function (v) {
                const date = new Date(v);
                return formattedDate(date);
            },
            yTickFormatShort: true,
            brushOn: false,
            elasticX: true,
            elasticY: true,
            margins: [10, 15, 50, 30],
            rotateLabels: true,
            translateLabels: [-20, 20],
            useCanvas: false,
            titleFunction: function (d) {
                return `${formattedDate(new Date(d.key))}: ${d.value} kg`;
            },
            centerBar: true
        });

        const windRose = Sintium.windRose({
            domId: "wind-rose",
            dataSource: windDataSource,
            margins: [0, 30, 0, 0],
            buckets: [0, 3, 6, 9, 12, 15, Infinity],
            directionColumn: "wind_direction_10m",
            valueColumn: "wind_speed_10m",
            latColumn: "latitude",
            lonColumn: "longitude"
        });

        playWidget.on('start-play', function () {
            timeLineBarChart.lockY(true);
            monthsChart.lockYAt(playWidget.getMaxForColumns("rundvekt", ["year"]));
            vesselSizeChart.lockYAt(playWidget.getMaxForColumns("rundvekt", ["lengdegruppe", "year"]));
            qualityChart.lockY(true);
            airPressureChart.lockAxes(true);
            temperatureChart.lockAxes(true);
            topElement.show(true);
        });

        playWidget.on('stop-play', function () {
            timeLineBarChart.lockY(false);
            monthsChart.lockY(false);
            vesselSizeChart.lockY(false);
            qualityChart.lockY(false);
            airPressureChart.lockAxes(false);
            temperatureChart.lockAxes(false);
            topElement.show(false);
        });

        playWidget.on('playing', function (message) {
            const dateText = formattedDate(new Date(message.currentValue));
            topElement.text(dateText);
        });

            // WIDGET GRID
        const grid = Sintium.widgetGrid({ domId: "grid" })
            .addWidget(toolsChart, {
                title: "Redskap",
                x: 6,
                y: 0,
                width: 6,
                height: 4,
                closable: false,
            })
            .addWidget(speciesChart, {
                title: "Art",
                x: 0,
                y: 4,
                width: 6,
                height: 4,
                closable: false,
            })
            .addWidget(windRose, {
                title: "Vind",
                y: 4,
                x: 0,
                width: 6,
                height: 4,
                closable: false
            })
            .addWidget(vesselSizeChart, {
                title: "Fartøylengde",
                y: 4,
                x: 6,
                width: 6,
                height: 4,
                closable: false,
            })
            .addWidget(monthsChart, {
                title: "Måneder",
                y: 8,
                x: 0,
                width: 12,
                height: 4,
                closable: false,
            })
            .addWidget(qualityChart, {
                title: "Kvalitet",
                y: 13,
                x: 0,
                width: 12,
                height: 5,
                closable: false,
            })
            .addWidget(timeLineBarChart, {
                title: "Rundvekt",
                y: 19,
                X: 0,
                width: 12,
                height: 4,
                closable: false,
            })
            .addWidget(temperatureChart, {
                title: "Overflatetemperatur (C)",
                y: 24,
                x: 0,
                width: 12,
                height: 5,
                closable: false,
            })
            .addWidget(airPressureChart, {
                title: "Lufttrykk (hPa)",
                y: 30,
                x: 0,
                width: 12,
                height: 4,
                closable: false
            });

        const dataSelector = new DataSelector({
            domId: "data-selector",
            years: years,
            catchDataUrl: `${BASE_CATCH_DATA_URL}?year={{year}}&month={{month}}`,
            windDataUrl: `${BASE_WIND_URL}?years={{year}}&months={{month}}`,
            catchDataSource: catchDataSource,
            windDataSource: windDataSource,
            downloadLimit: MONTHS_DOWNLOAD_LIMIT
        });


        const widgetDrawer = Sintium.drawer({
            buttonControl: Sintium.buttonControl({
                normalIcon: 'chart-pie',
                flow: 'vertical',
                position: 'top-right'
            }),
            widgets: [dataSelector, grid],
            position: "right",
            openClass: "widget-drawer-open"
        });

        // MAP

        const mapReduceLayer = Sintium.mapReduceLayer({
            layerId: "map-reduce",
            dataSource: catchDataSource,
            visible: true,
            lazyLoad: false,
            groupByProperty: "fangstfelt",
            sum: ['rundvekt'],
            map: mapReducedDataToFeature,
            onDataChange: onMapReduceLayerDataChange,
            styleFunction: styleFunction
        });

        mapReduceLayer.addSelection({
            selector: "single",
            condition: "click",
            filter: true
        });

        mapReduceLayer.addSelection({
            selector: "box",
            condition: "ctrl click",
            filter: true
        });

        const seaMapLayer = Sintium.wmsLayer({
            layerId: "Sjøkart",
            url: "https://opencache.statkart.no/gatekeeper/gk/gk.open?",
            params: {
                LAYERS: 'sjokartraster',
                VERSION: '1.1.1'
            },
            visible: false
        });

        const seaMapLayerToggle = Sintium.buttonControl({
            normalIcon: 'fas fa-map',
            position: 'top-right',
            flow: 'vertical',
        });

        seaMapLayerToggle.onClick(() => {
            const visible = !seaMapLayerToggle.isActive();
            seaMapLayerToggle.setActive(visible);
            seaMapLayer.setVisible(visible);
            ga('send', {
                hitType: 'event',
                eventCategory: 'Map',
                eventAction: 'BaseLayerChanged',
                eventLabel: 'interaction',
                eventValue: 1
            });
        });
        
        function getDetailedDownloadUrl() {
            let downloadUrl = DETAILED_CATCH_DOWNLOAD_BASE_URL;
            const timeLineDimension = timeLineBarChart.getChart().dimension();
            let currentDateTimeFilters = !timeLineDimension ? [] : timeLineDimension
                .group()
                .top(Infinity)
                .filter(grouping => grouping.value > 0)
                .map(grouping => {
                    const date = new Date(grouping.key);
                    return `${date.getFullYear()}-${+date.getMonth() + 1}`;
                });
            const qualityDimension = qualityChart.getChart().dimension();
            let currentQualityFilters = new Set(
                !qualityDimension ? [] : qualityDimension
                    .group()
                    .top(Infinity)
                    .filter(grouping => grouping.value > 0)
                    .map(grouping => grouping.key.split(':-:')[1])
            );
            const vesselSizeDimension = vesselSizeChart.getChart().dimension();
            let currentLengthFilters = new Set(!vesselSizeDimension ? [] : vesselSizeDimension
                    .group()
                    .top(Infinity)
                    .filter(grouping => grouping.value > 0)
                    .map(grouping => grouping.key.split(':-:')[0])
            );
            let currentSpeciesFilters = speciesChart.getChart().filters();
            let currentToolFilters = toolsChart.getChart().filters();
            let currentCatchAreaDimension = mapReduceLayer.getDimension();
            let currentCatchAreaFilters = !currentCatchAreaDimension ? [] : currentCatchAreaDimension
                .group()
                .top(Infinity)
                .filter(grouping => grouping.value > 0)
                .map(grouping => grouping.key);

            if(currentDateTimeFilters.length > 0) {
                const dateTimeFilterString = currentDateTimeFilters.map(filter => filter.substring(0, 7)).join(',');
                downloadUrl += `&dates=${dateTimeFilterString}`
            }
            if(qualityChart.getChart().filters().length > 0 && currentQualityFilters.size > 0) {
                const qualityFilterString = Array.from(currentQualityFilters).join(',');
                downloadUrl += `&qualityCodes=${qualityFilterString}`;
            }
            if(vesselSizeChart.getChart().filters().length > 0 && currentLengthFilters.size > 0) {
                const lengthFilterString = Array.from(currentLengthFilters).join(',');
                downloadUrl += `&lengthCodes=${lengthFilterString}`;
            }
            if(currentSpeciesFilters.length > 0) {
                const speciesFilterString = currentSpeciesFilters.join(',');
                downloadUrl += `&speciesCodes=${speciesFilterString}`;
            }
            if(currentToolFilters.length > 0) {
                const toolsFilterString = currentToolFilters.join(',');
                downloadUrl += `&toolCodes=${toolsFilterString}`;
            }
            if(currentCatchAreaFilters.length > 0) {
                const catchAreaFilterString = currentCatchAreaFilters.join(',');
                downloadUrl += `&catchAreas=${catchAreaFilterString}`;
            }
            return downloadUrl;
        }

        function detailedCatchDataDownloadCallback(url) {
            const headers = {
                'accept': 'text/csv',   // TODO: API gives date in another format if requesting csv, requesting json until fixed.
                "Accept-encoding": "br"
            };
            fetch(url, {
                headers: headers
            })
                .then(data => data.text())
                .then(text => {
                    download(text, "fangstdata.csv", "text/csv")
                });
        }
        
        function getUrlParameters(dataArray, callback) {
            let parameters = "";

            dataArray.forEach(callback);

            return parameters;
        }

        const downloadControl = Sintium.ExternalDownloadControl({
            position: 'top-right',
            flow: 'vertical',
            downloadUrlFetcherCallback: getDetailedDownloadUrl,
            downloadCallback: detailedCatchDataDownloadCallback
        });

        const map = Sintium.map({
            domId: "map",
            layers: [mapReduceLayer, seaMapLayer],
            controls: [
                legendControl, 
                playWidgetControl, 
                seaMapLayerToggle, 
                downloadControl,
                Sintium.htmlControl({
                    html: topElement.element()
                })
            ],
            viewPortMode: Sintium.isMobile() ? 'all' : 'visible',
            keepCenter: !Sintium.isMobile(),
            onViewPortChange: (extent) => {
                mapReduceLayer.filterByExtent(extent);
                windRose.setRange(extent[1], extent[0], extent[3], extent[2]);
            },
            use: [widgetDrawer]
        });

        if (!Sintium.isMobile()) {
            setTimeout(() => {
                widgetDrawer.open();
            }, 500);
        }
        
        const catchDateDimension = catchDataSource.getDataContainer().getCrossfilter().dimension(d => {
            const date = new Date(d[4]);
            return `${date.getFullYear()}-${date.getMonth()}`;
        });
        const windDateDimension = windDataSource.getDataContainer().getCrossfilter().dimension(d => {
            const date = new Date(d[1]);
            return `${date.getFullYear()}-${date.getMonth()}`;
        });

        function refilter() {
            const lookup = {};
            const windLookup = {};

            catchDateDimension.group().top(Infinity)
                .filter(grouping => grouping.value > 0)
                .map(grouping => grouping.key)
                .forEach(key => lookup[key] = true);

            windDateDimension.group().top(Infinity)
                .filter(grouping => grouping.value > 0)
                .map(grouping => grouping.key)
                .forEach(key => windLookup[key] = true);

            if(compareObjects(windLookup, lookup)) {
                windDateDimension.filter(null);
            } else if(dataSelector._downloading()) {
                Object.keys(dataSelector._selectedYears).forEach(
                    year => Object.keys(dataSelector._selectedMonths).forEach(
                        month => lookup[`${year}-${month}`] = dataSelector._selectedYears[year]
                    ));

                windDateDimension.filter(key => !!lookup[key]);
            } else if(Object.keys(lookup).length > 0) {
                windDateDimension.filter(key => !!lookup[key]);
            }
        }

        catchDataSource.onDataFiltered(() => refilter());
        catchDataSource.onDataAdded(() => refilter());

        function compareObjects(o1, o2){
            for(let p in o1){
                if(o1.hasOwnProperty(p)){
                    if(o1[p] !== o2[p]){
                        return false;
                    }
                }
            }
            for(let p in o2){
                if(o2.hasOwnProperty(p)){
                    if(o1[p] !== o2[p]){
                        return false;
                    }
                }
            }
            return true;
        }
    });