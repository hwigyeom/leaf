import { AuthenticationService } from '../authorization/AuthenticationService';
import { InjectContext } from '../dependencyInjection/InjectContext';
import { InjectService } from '../dependencyInjection/InjectService';
import { IGlobalEnvironments } from '../environments/IGlobalEnvironments';
import { ApplicationContext } from '../index';
import { ThemeService } from '../services/ThemeService';
import * as kendoUtils from '../utils/kendoUtils';
import { IView } from './IView';
import { Login } from './Login';
import { Main } from './Main';

export class ApplicationContainer implements IView {
  public static appContainerKey = '_container_';
  public static appContainerElement = 'app';

  private readonly _env: IGlobalEnvironments;
  private readonly _$element: JQuery;
  @InjectContext
  private context: ApplicationContext;
  @InjectService(AuthenticationService.authenticationServiceKey)
  private authService: AuthenticationService;
  @InjectService('ThemeService')
  private themeService: ThemeService;

  constructor(globalEnv: IGlobalEnvironments) {
    this._env = globalEnv;
    this._$element = $(`#${ApplicationContainer.appContainerElement}`);
  }

  public get env(): IGlobalEnvironments {
    return this._env;
  }

  public get $element(): JQuery {
    return this._$element;
  }

  private _login: Login;

  // Main View
  private get login(): Login {
    return this._login = this._login || this.context.dependencies.resolveView('Login');
  }

  private _main: Main;

  // Main View
  private get main(): Main {
    return this._main = this._main || this.context.dependencies.resolveView('Main');
  }

  public async initialize(): Promise<void> {
    const $html = this.$element.parents('html');

    // TODO: 지원 브라우저 체크

    // 로딩 시작
    this.showGlobalSpinner();

    // 언어설정
    if (this.env.culture) {
      $html.attr('lang', this.env.culture);
    }

    $html.find('head > meta').after($(`<title>${this.env.title}</title>`));

    // 색상 테마 로드
    await this.themeService.changeTheme(this.env.theme);

    // kendo culture 로드 및 설정
    await kendoUtils.setKendoCulture(this.env.culture);
    kendoUtils.fixKendoCalendar();

    // 인증 정보 확인 후 페이지 이동
    if (this.authService.isAuthenticated) {
      // 메인 화면 출력
      await this.main.initialize();
    } else {
      // 로그인 화면 출력
      await this.login.initialize();
    }

    // 로딩 제거
    // this.removeGlobalSpinner();
  }

  public showGlobalSpinner(): void {
    let $loading = this._$element.next('.loading');
    if (!$loading.length) {
      $loading = $('<div class="loading"></div>');
      this._$element.after($loading);
    }
  }

  public removeGlobalSpinner(): void {
    this._$element.next('.loading').remove();
  }
}
