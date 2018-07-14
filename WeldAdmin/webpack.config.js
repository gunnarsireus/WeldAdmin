var webpack = require('webpack');
var path = require('path');
var WebpackNotifierPlugin = require('webpack-notifier');

module.exports = {
    mode: 'development',
    resolve: {
        alias: {
            'react': path.join(__dirname, 'node_modules', 'react')
        }
    },
    context: path.join(__dirname, 'App'),
    entry: './main.js',
    output: {
        path: path.join(__dirname, 'Build'),
        filename: '[name].bundle.js'
    },
    plugins: [
      new WebpackNotifierPlugin(),
      new webpack.ProvidePlugin({
          $: "jquery",
          jQuery: "jquery",
          jquery: "jquery"
      }),
      new webpack.DefinePlugin({
          'process.env': {
              NODE_ENV: JSON.stringify('production')
          }
      })
    ],
    module: {
        rules: [
            { test: /\.css$/, loader: "style-loader!css-loader" },
            { test: /\.jpe?g$|\.gif$|\.png$|\.svg$|\.woff$|\.woff2$|\.ttf$|\.eot$/, loader: "url-loader" },
            { test: /\.html$/, loader: "html-loader" },
            { test: /\.js$/, loader: "babel-loader" },
            { test: /\.jsx$/, loader: "babel-loader" }
        ]
    }
};
