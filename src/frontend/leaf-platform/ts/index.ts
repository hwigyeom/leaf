import '../scss/leaf.scss';
import { AuthenticationService } from './authorization/AuthenticationService';
import { dependencies } from './dependencies';
import { DependencyManager, InstanceType } from './dependencyInjection/DependencyManager';
import { EnvironmentService } from './environments/EnvironmentService';
import { HttpService } from './network/HttpService';
import { UrlService } from './network/UrlService';
import * as debug from './utils/debugUtils';
import { ApplicationContainer } from './views/ApplicationContainer';
import { ViewLoader } from './views/ViewLoader';

export * from './exports';

export class ApplicationContext {
  public static appName = '_leaf_app_';

  public readonly dependencies: DependencyManager;

  private _container: ApplicationContainer;

  constructor() {
    this.dependencies = new DependencyManager();
  }

  public get container(): ApplicationContainer {
    return this._container;
  }

  public async run(): Promise<void> {
    debug.log('## Leaf Core: Application 시작 ##');

    // 필수 서비스들을 생성하고 등록
    // HttpService
    const httpService = new HttpService();
    this.dependencies.registerService(HttpService.httpServiceKey, httpService, InstanceType.Instance);

    // UrlService
    const urlService = new UrlService();
    this.dependencies.registerService(UrlService.urlServiceKey, urlService, InstanceType.Instance);

    // EnvironmentService
    const envService = new EnvironmentService();
    this.dependencies.registerService(EnvironmentService.environmentServiceKey, envService, InstanceType.Instance);

    // AuthenticationService
    const authService = new AuthenticationService();
    this.dependencies
      .registerService(AuthenticationService.authenticationServiceKey, authService, InstanceType.Instance);

    // 뷰 로더
    const viewLoader = new ViewLoader();
    this.dependencies.registerService(ViewLoader.viewLoaderKey, viewLoader, InstanceType.Instance);

    // dependencies 에 등록된 종속성 항목 등록
    // 서비스 등록 // TODO: 종속성 주입 설정에 대한 고도화
    for (const key of Object.keys(dependencies.services)) {
      this.dependencies.registerService(key, dependencies.services[key], InstanceType.Singleton);
    }
    // 뷰 등록
    for (const key of Object.keys(dependencies.views)) {
      this.dependencies.registerView(key, dependencies.views[key]);
    }
    // 템플릿 등록
    for (const key of Object.keys(dependencies.templates)) {
      this.dependencies.registerTemplate(key, dependencies.templates[key]);
    }

    // 어플리케이션 컨테이너 설정
    this._container = new ApplicationContainer();
    await this._container.initialize();
  }
}

const context = new ApplicationContext();

// 플랫폼 객체를 윈도우 객체에 등록
Object.defineProperty(window, ApplicationContext.appName, {
  value: context
});

export { context };

declare const VERSION: string;
export const version = VERSION;
