var path = require('path');

var projectRoot = path.resolve(__dirname, '../../../');

var cssPaths = {
    build: "wwwroot/css",    
    simple_main: "./ClientApp/src/css/simple-main.scss",
    result: "./ClientApp/src/css/pages/result/index.scss",

    main: "./ClientApp/src/css/main.scss",
    betlist_detail: "./ClientApp/src/css/pages/betlist-detail/index.scss",
    betlist_detail_cashout: "./ClientApp/src/css/pages/betlist-detail/cashout.scss",
    betlist_detail_colossus: "./ClientApp/src/css/pages/betlist-detail/colossus.scss",
    betlist_detail_systemparlay: "./ClientApp/src/css/pages/betlist-detail/systemparlay.scss"
};

var scriptPaths = {
    build: "wwwroot/js",
    
    event_builder: "./ClientApp/src/js/pages/event-builder/index.js",
    betlist_detail: "./ClientApp/src/js/pages/betlist-detail/components/index.jsx"

};
module.exports = {
    projectRoot: projectRoot,
    cssPaths: cssPaths,
    scriptPaths: scriptPaths
};