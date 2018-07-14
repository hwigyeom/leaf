import { ApplicationContext } from '../index';
import { HttpService } from '../network/HttpService';

export function InjectHttpService(target: any, key: string) {
  let http: HttpService;

  if (delete target[key]) {
    Object.defineProperty(target, key, {
      get: () => {
        if (http) {
          return http;
        }
        const context: ApplicationContext = window[ApplicationContext.appName];

        if (!context) {
          throw new Error('## Leaf Core: Does not found application context instance');
        }

        return http = context.dependencies.resolveService(HttpService.httpServiceKey) as HttpService;
      }
    });
  }
}
