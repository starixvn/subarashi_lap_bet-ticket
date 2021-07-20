var merge = require('webpack-merge');
var utils = require('./utils');
var baseWebpackConfigGenerator = require('./webpack.base.conf');
var CleanWebpackPlugin = require('clean-webpack-plugin');

var pathConfig = require('../config/path');

var baseConfigs = baseWebpackConfigGenerator();

var cssRootPath = pathConfig.cssPaths.build;
var jsRootPath = pathConfig.scriptPaths.build;

module.exports = merge(baseConfigs,
    utils.setDefinePlugin('process.env.NODE_ENV', 'development'),
    {
        mode: 'development',
        devtool: 'source-map',
        watch: true,
        plugins: [
            new CleanWebpackPlugin([`${jsRootPath}/`], {
                root: process.cwd()
            }),
            new CleanWebpackPlugin([`${cssRootPath}/*.css`, `${cssRootPath}/*.map`, `${cssRootPath}/*.js`], {
                root: process.cwd()
            }),
            new CleanWebpackPlugin([`${cssRootPath}/pages/*/*.css`, `${cssRootPath}/pages/*/*.js`, `${cssRootPath}/pages/*/*.map`], {
                root: process.cwd()
            })
        ]
    },

    utils.extractCSS(false)
);
