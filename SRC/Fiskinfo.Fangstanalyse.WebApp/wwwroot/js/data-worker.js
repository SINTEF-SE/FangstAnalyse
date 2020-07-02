const CATCH = "catch";
const WIND = "wind";
const HEADERS = {
    'accept': 'text/csv',
    "Accept-encoding": "br"
};

const baseURL = {};
const dataLookup = {
    [CATCH]: [],
    [WIND]: []
};
const queuedData = {
    [CATCH]: [],
    [WIND]: []
};
const failedDownloads = new Set();
let keys;
let keysStreamed = {
    [CATCH]: {},
    [WIND]: {}
};
let chunkSize;



function initialize(options) {
    setInterval(() => streamChunksIfAny(chunkSize), 10);
    baseURL[CATCH] = options.catchUrl;
    baseURL[WIND] = options.windUrl;
    chunkSize = options.chunkSize;
}

function sendDone() {
    postMessage({
        action: "done",
    });
}

function markDataAsDownloaded(type, key, data) {
    dataLookup[type][key] = data;
}

function getDataUrl(type, key) {
    const [year, month] = key.split("-");
    let url = baseURL[type].replace('{{year}}', year);
    url = url.replace('{{month}}', month);
    return url;
}

function isDataDownloaded(type, key) {
    return !!dataLookup[type][key];
}

function downloadAndQueueAllDataForStreaming(keys) {
    for (const key of keys) {
        downloadAndQueueDataForStreaming(key);
    }
}

function downloadAndQueueDataForStreaming(key) {
    const year = key.split("-")[0];
    getData(CATCH, key)
        .then(data => queuedData[CATCH].push({ rows: parseData(CATCH, data), key }))
        .catch(() => failedDownloads.add(key));

    if (year > 2013) {
        getData(WIND, key)
            .then(data => queuedData[WIND].push({ rows: parseData(WIND, data), key }))
            .catch(() => failedDownloads.add(key));
    }
}

function getData(type, key, failed) {
    return new Promise((resolved, rejected) => {
        if (isDataDownloaded(type, key)) {
            const data = dataLookup[type][key];
            resolved(data);
        } else {
            const url = getDataUrl(type, key);
            fetch(url, {
                headers: HEADERS
            })
                .then(data => data.text())
                .then(data => {
                    markDataAsDownloaded(type, key, data);
                    if (data.length > 0) {
                        resolved(data);
                    }
                });
        }
    });
}

function parseData(type, data) {
    if (!!data) {
        data = data.split("\r\n");
        const headers = data.shift();
        if (type === CATCH) {
            return data
                .map(line => {
                    return line.split(",").map((value, index) => {
                        return index === 0 || index === 7 || index === 8 ? parseFloat(value) : value;
                    })
                })
                .filter(d => d[3] != null);
        } else if (type === WIND) {
            return data
                .map(line => {
                    return line.split(",").map((value, index) => {
                        return index > 0 ? parseFloat(value) : value;
                    })
                })
                .filter(d => d[0] != null);
        }
    }
}

function hasChunks() {
    return queuedData[CATCH].length > 0 || queuedData[WIND].length > 0;
}

function hasFailedData() {
    return failedDownloads.size > 0;
}

function checkIfStreamedAll() {
    return keys.every(key => keysStreamed[CATCH][key] && (keysStreamed[WIND][key] || key.split("-")[0] <= "2013"));
}

function streamChunksIfAny(chunkSize) {
    if (hasChunks()) {
        streamChunks(CATCH, chunkSize);
        streamChunks(WIND, chunkSize);
    }
    if (hasFailedData()) {
        const failedKeys = Array.from(failedDownloads);
        downloadAndQueueAllDataForStreaming(failedKeys);
        failedDownloads.clear();
    }
}

function streamChunks(type, chunkSize) {
    let chunk = [];
    let data = queuedData[type].shift();
    while (!!data && Array.isArray(data.rows)) {
        chunk = chunk.concat(data.rows.slice(0, chunkSize - chunk.length));
        data.rows = data.rows.slice(chunk.length);
        if (chunk.length === chunkSize || (data.rows.length === 0 && chunk.length > 0)) {
            streamChunk(data.key, type, chunk);
            chunk = [];
        }
        if (data.rows.length === 0) {
            keysStreamed[type][data.key] = true;
            data = queuedData[type].shift();
            if (checkIfStreamedAll()) {
                sendDone();
            }
        }
    }
}

function streamChunk(key, type, chunk) {
    postMessage({
        action: `chunk-${type}`,
        chunk: chunk,
        key: key
    });
}

onmessage = function(e) {
    switch (e.data.action) {
        case "initialize":
            initialize(e.data);
            break;
        case "fetch":
            keys = e.data.keys;
            if (keys.length === 0) {
                sendDone();
            } else {
                keysStreamed = {
                    [CATCH]: {},
                    [WIND]: {}
                };
                downloadAndQueueAllDataForStreaming(keys);
            }
            break;
        default:
            console.warn(`Unknown action "${e.data.action}" in data-worker`);
    }
};