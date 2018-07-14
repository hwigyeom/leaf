import { ApplicationContext } from '../index';
import { IView } from '../views/IView';

export function InjectView(key: string) {
  return (target: any, propertyKey: string) => {
    let instance: IView;

    if (delete target[propertyKey]) {
      Object.defineProperty(target, propertyKey, {
        get: () => {
          if (instance) {
            return instance;
          }

          const context: ApplicationContext = window[ApplicationContext.appName];

          if (!context) {
            throw new Error('## Leaf Core: Does not found application context instance');
          }

          return instance = context.dependencies.resolveView(key);
        },
        configurable: true
      });
    }
  };
}
