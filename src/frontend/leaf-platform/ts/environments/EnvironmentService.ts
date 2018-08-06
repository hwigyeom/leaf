import { InjectHttpService } from '../dependencyInjection/InjectHttpService';
import { HttpMethod, HttpService } from '../network/HttpService';
import { IHttpService } from '../network/IHttpService';
import * as debug from '../utils/debugUtils';
import { IGlobalEnvironments } from './IGlobalEnvironments';
import { ILoginEnvironments } from './ILoginEnvironments';

const GLOBAL_ENV_URL = '/env/global';
const LOGIN_ENV_URL = 'env/login';

interface IEnvironmentsItem<T> {
  loaded: boolean;
  env?: T;
}

export class EnvironmentService implements IHttpService {
  public static readonly environmentServiceKey: string = '_environments_';

  private static readonly defaultGlobalEnv: IGlobalEnvironments = {
    culture: 'ko',
    title: 'Leaf Application Framework',
    subTitle: 'Example',
    theme: 'blue',
    keys: {
      token: 'leaf:auth:token',
      sid: 'leaf:auth:sid'
    }
  };

  private static readonly defaultLoginEnv: ILoginEnvironments = {
    ci: '/images/login-ci.svg',
    ciTitle: '',
    banner: '/images/login-banner.png',
    product: '/images/login-solution.svg',
    productTitle: '',
    background: '/images/login-bg.png'
  };

  @InjectHttpService
  public http: HttpService;

  private readonly _global: IEnvironmentsItem<IGlobalEnvironments>;
  private readonly _login: IEnvironmentsItem<ILoginEnvironments>;

  constructor() {
    this._global = {loaded: false};
    this._login = {loaded: false};
    debug.log('## Leaf Core: EnvironmentService 초기화 ##');
  }

  public get global() {
    if (!this._global.loaded) {
      throw new Error('## Leaf Core: 글로벌 환경 변수가 아직 로드되지 않았습니다.');
    }

    return this._global.env;
  }

  public get login() {
    if (!this._login.loaded) {
      throw new Error(`## Leaf Core: 로그인 환경 변수가 아직 로드되지 않았습니다.`);
    }

    return this._login.env;
  }

  public async loadGlobalEnvironments(): Promise<void> {
    try {
      const serverEnv = await this.http.data<IGlobalEnvironments>(GLOBAL_ENV_URL, HttpMethod.GET);

      this._global.loaded = true;
      this._global.env = $.extend({}, true, EnvironmentService.defaultGlobalEnv, serverEnv);
    } catch (e) {
      debug.error(e);
      if (!this._global.loaded) {
        this._global.loaded = true;
        this._global.env = EnvironmentService.defaultGlobalEnv;
      }
    }
  }

  public async loadLoginEnvironments(): Promise<void> {
    try {
      const serverEnv = await this.http.data<ILoginEnvironments>(LOGIN_ENV_URL, HttpMethod.GET);

      this._login.loaded = true;
      this._login.env = $.extend({}, true, EnvironmentService.defaultLoginEnv, serverEnv);
    } catch (e) {
      debug.error(e);
      if (!this._login.loaded) {
        this._login.loaded = true;
        this._login.env = EnvironmentService.defaultLoginEnv;
      }
    }
  }
}
