module.exports = {
    entry: {
        tumblr: "./Scripts/tumblr/desktop.jsx"
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