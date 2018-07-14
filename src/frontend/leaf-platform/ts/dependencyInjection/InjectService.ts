import { ApplicationContext } from '../index';

export function InjectService(key: string) {
  return (target: any, propertyKey: string) => {
    let service: any;

    if (delete target[propertyKey]) {
      Object.defineProperty(target, propertyKey, {
        get: () => {
          if (service) {
            return service;
          }

          const context: ApplicationContext = window[ApplicationContext.appName];

          if (!context) {
            throw new Error('## Leaf Core: Does not found application context instance');
          }

          return service = context.dependencies.resolveService(key);
        },
        configurable: true
      });
    }
  };
}
