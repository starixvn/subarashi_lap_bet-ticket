var webpack = require('webpack');
var WebpackBuildNotifierPlugin = require('webpack-build-notifier');
const RemoveEmptyScriptsPlugin = require('webpack-remove-empty-scripts');

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
        new RemoveEmptyScriptsPlugin(),
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