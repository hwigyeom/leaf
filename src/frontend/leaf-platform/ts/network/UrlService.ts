import * as debug from '../utils/debugUtils';

export class UrlService {
  public static readonly urlServiceKey: string = '_url_';

  constructor() {
    debug.log('## Leaf Core: UrlService 초기화 ##');
  }

  /**
   * 현재 상태가 디버그 모드인지 여부
   */
  public static get isDebugMode(): boolean {
    return /[?&][Dd]ebug=true/.test(window.location.search);
  }
}
