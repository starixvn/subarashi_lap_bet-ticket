var webpack = require('webpack');
var WebpackBuildNotifierPlugin = require('webpack-build-notifier');

let _buildNumber = 1;
function generatePlugins() {
    var commonPlugins = [
        new webpack.IgnorePlugin({
            resourceRegExp: /^\.\/locale$/,
            contextRegExp: /moment$/
        }),
        new WebpackBuildNotifierPlugin({
            title: "React Webpack Build",
            suppressSuccess: true
        }),
        function (compiler) {
            compiler.hooks.done.tap("BuildNotifierPlugin", (stats) => {
                setTimeout(() => {
                    console.log('\n ////////// Build version: #' + (_buildNumber++) + ' (' + new Date().toLocaleString() + ').//////////\n');
                }, 150);
            });
        }
    ];

    return commonPlugins;
}

module.exports = generatePlugins;