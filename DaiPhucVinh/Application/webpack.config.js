const path = require("path");
const webpack = require("webpack");
module.exports = {
  entry: "./app.js",
  output: {
    path: path.join(__dirname, "./../WebUI/asset
    s/js"),
    filename: "app.bundle.js",
    clean: true,
  },
  devtool: false,
  module: {
    rules: [
      {
        test: /\.js$/,
        exclude: /node_modules/,
        use: ["babel-loader"],
      },
      {
        test: /\.(css)$/,
        use: ["style-loader", "css-loader", "sass-loader"],
      },
      {
        test: /\.(scss)$/,
        use: ["style-loader", "css-loader", "sass-loader"],
      },
    ],
  },
  plugins: [
    new webpack.ProvidePlugin({
      $: "jquery",
      jQuery: "jquery",
      "window.jQuery": "jquery",
      Popper: ["popper.js", "default"],
    }),
  ],
};
