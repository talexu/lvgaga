module.exports = {
    entry: {
        tumblrDesktop: "./Scripts/tumblr/lv.tumblr.index.desktop.js",
        commentDesktop: "./Scripts/comment/lv.comment.index.desktop.js"
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