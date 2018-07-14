import * as del from 'del';
import * as fs from 'fs-extra';
import * as OnBuildPlugin from 'on-build-webpack';
import * as path from 'path';
import * as webpack from 'webpack';
import getBaseConfig from './webpack.config.base';

const config = (env, argv): webpack.Configuration => {
  const baseConfig = getBaseConfig(argv.mode);

  baseConfig.plugins.push(new OnBuildPlugin(() => {
    const source = path.resolve(__dirname, '../_dist');
    const destination = path.resolve(__dirname, '../../../backend/Leaf.Server/Statics');
    // 테마 CSS 작성을 위해 생성된 js 파일 삭제
    del.sync([path.join(source, 'js/leaf.*.js*')]);
    // 백엔드 서버 정적리소스로 배포
    del.sync([path.join(destination, '/**/*')], {force: true});
    fs.copySync(source, destination);
  }));

  return baseConfig;
};

export default config;
