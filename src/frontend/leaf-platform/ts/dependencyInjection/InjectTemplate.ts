import { ApplicationContext } from '../index';

export function InjectTemplate(key: string) {
  return (target: any, propertyKey: string) => {
    let template: any;

    if (delete target[propertyKey]) {
      Object.defineProperty(target, propertyKey, {
        get: () => {
          if (typeof template !== 'undefined') {
            return template;
          }

          const context: ApplicationContext = window[ApplicationContext.appName];

          if (!context) {
            throw new Error('## Leaf Core: Does not found application context instance');
          }

          return template = context.dependencies.resolveTemplate(key);
        },
        configurable: true
      });
    }
  };
}
