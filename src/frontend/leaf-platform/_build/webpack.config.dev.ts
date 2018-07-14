import * as path from 'path';
import * as webpack from 'webpack';
import getBaseConfig from './webpack.config.base';

const config = (env, argv): webpack.Configuration => {
  const baseConfig = getBaseConfig(argv.mode);

  baseConfig.devServer = {
    contentBase: path.resolve(__dirname, '../dist'),
    host: '0.0.0.0',
    port: 8800,
    disableHostCheck: true,
    watchOptions: {
      poll: true,
    },
    inline: true,
    proxy: [
      {
        context: ['/api', '/view', '/data', '/file', '/env'],
        target: 'http://localhost:5000',
      },
    ],
  };

  return baseConfig;
};

export default config;
