const { merge } = require('webpack-merge');
var utils = require('./utils');
var baseWebpackConfigGenerator = require('./webpack.base.conf');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
var pathConfig = require('../config/path');

var baseConfigs = baseWebpackConfigGenerator();

var cssRootPath = pathConfig.cssPaths.build;
var jsRootPath = pathConfig.scriptPaths.build;

module.exports = merge(baseConfigs,
    utils.setDefinePlugin('process.env.NODE_ENV', 'development'),
    {
        mode: 'development',
        devtool: 'source-map',
        watch: false,
        plugins: [
            new CleanWebpackPlugin({
                cleanOnceBeforeBuildPatterns: [
                    `${jsRootPath}/**/*`,
                    `${cssRootPath}/*.css`,
                    `${cssRootPath}/*.map`,
                    `${cssRootPath}/*.js`,
                    `${cssRootPath}/pages/*/*.css`,
                    `${cssRootPath}/pages/*/*.js`,
                    `${cssRootPath}/pages/*/*.map`
                ],
                dry: false,
                verbose: true,
                dangerouslyAllowCleanPatternsOutsideProject: false
            })
        ]
    },

    utils.extractCSS(false)
);