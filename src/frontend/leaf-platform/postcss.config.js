module.exports = {
  plugins: [
    require('postcss-import'),
    require('autoprefixer'),
    require('postcss-move-props-to-bg-image-query'),
    require('cssnano'),
  ],
};
