var alias = require('../config/alias');
var pathConfig = require('../config/path');
var pluginGenerator = require('../config/plugin');
const UglifyJsPlugin = require("uglifyjs-webpack-plugin");

function generateConfig() {
    var jsPath = pathConfig.scriptPaths.build;
    var cssPath = pathConfig.cssPaths.build;
    var commonConfigs = {
        resolve: {
            alias: alias,
            extensions: ['.js', '.jsx']
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
            [`${cssPath}/betlist-detail-systemparlay`]: pathConfig.cssPaths.betlist_detail_systemparlay
        },
        output: {
            path: pathConfig.projectRoot,
            filename: "[name].js",
            chunkFilename: jsPath + '/[name].chunk.js',
        },
        module: {
            rules: [{
                test: /\.(js|jsx)$/,
                loader: ["babel-loader"],
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
        node: {
            fs: 'empty'
        },
        externals: [
            { './jszip': 'jszip' },
            { './ods': 'ods' },
            { './cptable': 'var cptable' }
        ],
        optimization: {
            minimizer: [
                new UglifyJsPlugin({
                    sourceMap: true
                })
            ],
            splitChunks: {
                minSize: 30000,
                maxSize: 0,
                maxAsyncRequests: 5,
                maxInitialRequests: 3,
                name: false,
                cacheGroups: {
                    default: false,
                    vendor: {
                        test: /[\\/]node_modules[\\/]((?!(axios)).*)[\\/]((?!(process)).*)[\\/]/,
                        name: 'lib/vendor',
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