import { InjectContext } from '../dependencyInjection/InjectContext';
import { EnvironmentService } from '../environments/EnvironmentService';
import { ApplicationContext } from '../index';
import * as debug from '../utils/debugUtils';

export enum HttpMethod {
  GET = 'GET',
  POST = 'POST',
  PUT = 'PUT',
  DELETE = 'DELETE'
}

export class HttpService {
  public static readonly httpServiceKey: string = '_http_';

  @InjectContext
  private context: ApplicationContext;

  constructor() {
    debug.log('## Leaf Core: HttpService 초기화 ##');

    $.ajaxSetup({
      beforeSend: (jqXHR: JQueryXHR, settings: JQueryAjaxSettings) => {
        // 인증 토큰을 헤더에 추가
        const token = this.getAuthToken();
        if (token) {
          jqXHR.setRequestHeader('Authorization', `Bearer ${token}`);
        }
      }
    });
  }

  public async data<T>(url: string, method: HttpMethod = HttpMethod.GET, settings?: JQueryAjaxSettings): Promise<T> {
    return new Promise<T>((resolve, reject) => {
      const defaultSettings: JQueryAjaxSettings = {
        dataType: 'json',
        cache: false
      };

      settings = $.extend(true, {}, defaultSettings, settings || {});
      settings.method = method;

      debug.info('## Leaf Core: HTTP Request for data - Start ##', method, url, settings);

      $.ajax(url, settings)
        .done((data: any, textStatus: string, jqXHR: JQueryXHR) => {
          debug.info('## Leaf Core: HTTP Request for data - Success ##', method, url, textStatus, jqXHR);
          if (data) {
            debug.table(data);
          }
          resolve(data as T);
        })
        .fail((jqXHR: JQueryXHR, textStatus: string, errorThrown: string) => {
          debug.info('## Leaf Core: HTTP Request for data - Fail ##', method, url, jqXHR, textStatus, errorThrown);
          reject({method, url, jqXHR, textStatus, errorThrown});
        });
    });
  }

  private getAuthToken(): string {
    if (this.context) {
      const envService = this.context.dependencies.resolveService(EnvironmentService.environmentServiceKey);
    }
    return;
  }
}
