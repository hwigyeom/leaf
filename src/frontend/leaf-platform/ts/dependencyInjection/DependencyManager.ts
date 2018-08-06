import { IView } from '../views/IView';

/**
 * 종속성 등록 형식
 */
export enum InstanceType {
  /**
   * 요청 시 마다 새로운 인스턴스를 생성하여 반환하도록 등록
   */
  Transient = 'Transient',
  /**
   * 인스턴스를 하나만 생성하여 같이 사용하도록 등록
   */
  Singleton = 'Singleton',
  /**
   * 이미 생성된 인스턴스를 등록
   */
  Instance = 'Instance'
}

/**
 * 종속성 등록 항목
 */
class DependencyItem<T> {
  public type: InstanceType;
  public dependency: T | { new(...args: any[]): T };
}

/**
 * 종속성 분류별 종속성을 관리하는 사전
 */
class DependencyDictionary<T> {

  public register(key: string, dependency: T | { new(...args: any[]): T },
                  type: InstanceType = InstanceType.Transient) {
    if (Object.getOwnPropertyDescriptor(this, key)) {
      throw new Error(`## Leaf Core: Depdency Manager - '${key} is already registered key.'`);
    } else {
      const item = new DependencyItem<T>();
      item.type = type;
      if (item.type === InstanceType.Singleton) {
        item.dependency = new (dependency as { new(...args: any[]): T })();
      } else {
        item.dependency = dependency;
      }
      Object.defineProperty(this, key, {value: item});
    }
  }

  public resolve(key: string): T {
    if (!Object.getOwnPropertyDescriptor(this, key)) {
      return;
    }
    const item = this[key] as DependencyItem<T>;
    let instance: T;
    switch (item.type) {
      case InstanceType.Transient:
        instance = new (item.dependency as { new(...args: any[]): T })();
        break;
      default:
        instance = item.dependency as T;
        break;
    }

    return instance;
  }
}

/**
 * 어플리케이션의 종속성을 관리합니다.
 */
export class DependencyManager {
  private readonly templates: DependencyDictionary<any>;
  private readonly views: DependencyDictionary<IView>;
  private readonly services: DependencyDictionary<any>;

  constructor() {
    this.templates = new DependencyDictionary<any>();
    this.views = new DependencyDictionary<IView>();
    this.services = new DependencyDictionary<any>();
  }

  // region Templates
  /**
   * 뷰 템플릿을 등록합니다.
   * @param {string} key 템플릿의 키
   * @param template 템플릿
   */
  public registerTemplate(key: string, template: any): void {
    this.templates.register(key, template, InstanceType.Instance);
  }

  /**
   * 지정한 키로 등록된 템플릿을 가져옵니다.
   * @param {string} key 템플릿의 키
   * @return {any} 템플릿
   */
  public resolveTemplate(key: string): any {
    return this.templates.resolve(key);
  }

  // endregion Template

  // region Views
  /**
   * 뷰 형식의 종속성을 등록합니다.
   * @param {string} key 뷰의 키
   * @param {{new(...args: any[]): IView}} view 뷰 클래스
   */
  public registerView(key: string, view: { new(...args: any[]): IView }): void {
    this.views.register(key, view);
  }

  /**
   * 지정된 키로 등록된 뷰 객체를 가져옵니다.
   * @param {string} key 뷰의 키
   * @return {IView} 뷰 객체
   */
  public resolveView(key: string): IView {
    return this.views.resolve(key);
  }

  // endregion Views

  // region Service
  /**
   * 서비스 형식의 종속성을 등록합니다.
   * @param {string} key 서비스의 키
   * @param {IHttpService | {new(...args: any[]): IHttpService} | any} service 서비스 형식이나 객체
   * @param {InstanceType} type 종속성 등록 형식
   */
  public registerService<T>(key: string, service: T | { new(...args: any[]): T },
                            type: InstanceType = InstanceType.Transient): void {
    this.services.register(key, service, type);
  }

  /**
   * 지정한 키로 등록된 서비스의 인스턴스를 가져옵니다.
   * @param {string} key 서비스의 키
   * @return {IHttpService} 서비스의 인스턴스
   */
  public resolveService<T>(key: string): any {
    return this.services.resolve(key) as T;
  }

  // endregion Service
}
