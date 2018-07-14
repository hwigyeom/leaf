import * as CopyPlugin from 'copy-webpack-plugin';
import * as ExtractTextPlugin from 'extract-text-webpack-plugin';
import * as HtmlPlugin from 'html-webpack-plugin';
import * as path from 'path';
import * as StyleLintPlugin from 'stylelint-webpack-plugin';
import * as webpack from 'webpack';
import * as CleanupPlugin from 'webpack-cleanup-plugin';

const pkg = require('../package.json');
const jqPkg = require('../node_modules/jquery/package.json');

// 스타일시트
const sassExtractPlugin = new ExtractTextPlugin({filename: 'css/[name].css'});
const styleLint = new StyleLintPlugin();

// 이미지 및 라이브러리 복사
const copyPlugin = new CopyPlugin([
  {from: '**/*', to: 'images/', context: 'images/'},
  {from: '**/*', to: 'js/', context: '_lib/kendo/'},
  {from: 'jquery.*', to: 'js/', ignore: ['*.slim.*'], context: 'node_modules/jquery/dist'}
], {debug: 'info'});

// HTML 문자열 교체
const htmlPlugins = [
  new HtmlPlugin({
    target: 'index.html',
    template: 'ts/templates/index.ejs',
    replacement: {
      version: pkg.version,
      kendo: pkg.kendo,
      jquery: jqPkg.version
    },
    inject: false,
    minify: false
  }),
];

const banner = `Leaf front-end application container v${pkg.version}
Copyright 2018 E&SYS co.,ltd All right reserved.`;

const bannerPlugin = new webpack.BannerPlugin({
  banner,
  entryOnly: true,
  test: /\.js$/
});

const cssBanner = `Leaf front-end framework stylesheet v${pkg.version}
Copyright 2018 E&SYS co.,ltd All right reserved.`;

const cssBannerPlugin = new webpack.BannerPlugin({
  banner: cssBanner,
  entryOnly: true,
  test: /\.css$/
});

// 기본 설정
const getBaseConfig = (mode: string): webpack.Configuration => {
  return {
    context: path.resolve(__dirname, '../'),
    entry: {
      'leaf': './ts/index.ts',
      'leaf.blue': './scss/themes/leaf.blue.scss',
    },
    output: {
      path: path.resolve(__dirname, '../_dist'),
      filename: 'js/[name].js',
      libraryTarget: 'umd',
      library: '[name]',
      umdNamedDefine: true,
      publicPath: '/',
    },

    resolve: {
      extensions: ['.ts', '.js', '.json', '.scss', '.html', '.css', '.ejs', '.svg'],
    },

    devtool: mode === 'development' ? 'inline-source-map' : 'source-map',

    module: {
      rules: [
        {
          enforce: 'pre',
          test: /\.tsx?$/,
          use: [
            {loader: 'tslint-loader', options: {emitErrors: true, typeCheck: false}},
          ],
          exclude: /node_modules/,
        },

        {
          test: /\.tsx?$/,
          loader: 'ts-loader',
          exclude: [/node_modules/, /_build/, /_proxy/, /_dist/]
        },

        {
          test: /\.scss$/,
          use: sassExtractPlugin.extract({
            fallback: 'style-loader',
            use: [
              {loader: 'css-loader', options: {sourceMap: mode === 'development', importLoaders: 1}},
              {loader: 'postcss-loader', options: {sourceMap: true}},
              {loader: 'resolve-url-loader', options: {keepQuery: true}},
              {loader: 'svg-transform-loader/encode-query'},
              {loader: 'sass-loader', options: {sourceMap: true}},
            ],
          }),
        },

        {
          test: /\.html$/,
          use: {
            loader: 'html-loader', options: {minimize: true},
          },
        },

        {
          test: /\.ejs$/,
          use: {
            loader: 'ejs-loader', options: {minimize: true},
          },
        },

        {
          test: /\.svg(\?.*)?$/,
          use: [
            {loader: 'url-loader'},
            {loader: 'svg-transform-loader'},
          ],
        },

        {
          test: /\.(png|jpe?g|gif)(\?.*)?$/,
          loader: 'url-loader',
          options: {
            limit: 10000,
            name: 'images/[name].[ext]',
          },
        },

        {
          test: /\.(woff2?|eot|ttf|otf)(\?.*)?$/,
          loader: 'url-loader',
          options: {
            limit: 10000,
            name: 'fonts/[name].[ext]',
          },
        },
      ],
    },

    plugins: [
      new CleanupPlugin(),
      bannerPlugin,
      styleLint,
      sassExtractPlugin,
      cssBannerPlugin,
      ...htmlPlugins,
      copyPlugin,
    ],
  };
};

export default getBaseConfig;
