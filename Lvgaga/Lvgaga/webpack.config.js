const webpack = require('webpack');
const commonsPlugin = new webpack.optimize.CommonsChunkPlugin('common.js');

module.exports = {
    entry: {
        tumblrDesktop: "./Scripts/tumblr/desktop/index.js",
        tumblrMobile: "./Scripts/tumblr/mobile/index.js",
        commentDesktop: "./Scripts/comment/desktop/index.js",
        commentMobile: "./Scripts/comment/mobile/index.js",
        favoriteDesktop: "./Scripts/favorite/desktop/index.js",
        favoriteMobile: "./Scripts/favorite/mobile/index.js"
    },
    output: {
        path: "./Scripts/dist",
        filename: "[name].entry.js"
    },
    module: {
        loaders: [
            { test: /\.jsx?$/, exclude: /node_modules/, loader: "babel-loader" }
        ]
    }
};