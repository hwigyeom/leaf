import { ApplicationContext } from '../index';

/**
 * 어플리케이션 컨텍스트를 주입하는 데코레이터입니다.
 */
export function InjectContext(target: any, propertyKey: string) {
  let context: ApplicationContext;

  if (delete target[propertyKey]) {
    Object.defineProperty(target, propertyKey, {
      get: (): ApplicationContext => {
        if (context) {
          return context;
        }

        context = window[ApplicationContext.appName] as ApplicationContext;
        if (!context) {
          throw new Error('## Leaf Core: Does not found application context instance');
        }
        return context;
      },
      configurable: true
    });
  }
}
