const path = require('path');
const alias = require('../config/alias');

var pathConfig = require('../config/path');
var pluginGenerator = require('../config/plugin');
const TerserPlugin = require("terser-webpack-plugin");

function generateConfig() {
    var jsPath = pathConfig.scriptPaths.build;
    var cssPath = pathConfig.cssPaths.build;
    var commonConfigs = {
        resolve: {
            alias: alias,
            extensions: ['.js', '.jsx'],
            fallback: {
                fs: false
            }
        },
        entry: {
            /*js*/
            [`${jsPath}/event-builder`]: pathConfig.scriptPaths.event_builder, //independent entry
            [`${jsPath}/betlist-detail`]: pathConfig.scriptPaths.betlist_detail,

            // /*css*/
            [`${cssPath}/simple-main`]: pathConfig.cssPaths.simple_main,
            [`${cssPath}/result`]: pathConfig.cssPaths.result,
            [`${cssPath}/main`]: pathConfig.cssPaths.main,
            [`${cssPath}/betlist-detail`]: pathConfig.cssPaths.betlist_detail,
            [`${cssPath}/betlist-detail-cashout`]: pathConfig.cssPaths.betlist_detail_cashout,
            [`${cssPath}/betlist-detail-systemparlay`]: pathConfig.cssPaths.betlist_detail_systemparlay,
            [`${cssPath}/betlist-detail-hybridparlay`]: pathConfig.cssPaths.betlist_detail_hybridparlay
        },
        output: {
            path: pathConfig.projectRoot,
            filename: "[name].js",
            chunkFilename: jsPath + '/[name].chunk.js',
        },
        module: {
            rules: [{
                test: /\.(js|jsx)$/,
                use: "babel-loader",
                exclude: /node_modules/
            }]
        },
        plugins: pluginGenerator(),
        stats: {
            assets: false,
            colors: true,
            version: false,
            hash: false,
            timings: true,
            chunks: false,
            chunkModules: false,
            children: true
        },
        externals: [
            { './jszip': 'jszip' },
            { './ods': 'ods' },
            { './cptable': 'var cptable' }
        ],
        optimization: {
            minimizer: [
                new TerserPlugin({
                    parallel: true,
                    extractComments: false
                })
            ],
            splitChunks: {
                minSize: 30000,
                maxAsyncRequests: 5,
                maxInitialRequests: 3,
                name: false,
                cacheGroups: {
                    default: false,
                    vendor: {
                        test: /[\\/]node_modules[\\/]((?!(axios)).*)[\\/]((?!(process)).*)[\\/]/,
                        name: `${pathConfig.scriptPaths.build}/lib/vendor`,
                        filename: "[name].chunk.js",
                        chunks: 'all',
                        minChunks: 1
                    }
                }
            }
        }
    };

    return commonConfigs;
}

module.exports = generateConfig;