const webpack = require('webpack');
const commonsPlugin = new webpack.optimize.CommonsChunkPlugin('common.js');

module.exports = {
    entry: {
        tumblrDesktop: "./Scripts/tumblr/lv.tumblr.index.desktop.js",
        tumblrMobile: "./Scripts/tumblr/lv.tumblr.index.mobile.js",
        commentDesktop: "./Scripts/comment/lv.comment.index.desktop.js",
        commentMobile: "./Scripts/comment/lv.comment.index.mobile.js",
        favoriteDesktop: "./Scripts/favorite/lv.favorite.index.desktop.js",
        favoriteMobile: "./Scripts/favorite/lv.favorite.index.mobile.js"
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