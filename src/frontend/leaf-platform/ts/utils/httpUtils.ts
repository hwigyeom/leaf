/**
 * jQuery 전역 AJAX 전송 설정을 수행합니다.
 * @param {string} tokenStorageKey 인증토큰이 저장되어있는 세션스토리지 키입니다.
 */
export function setupAjax(tokenStorageKey: string) {
  $.ajaxSetup({
    beforeSend: (jqXHR: JQueryXHR, settings: JQueryAjaxSettings) => {
      if (tokenStorageKey) {
        const authToken = sessionStorage.getItem(tokenStorageKey);

        if (authToken) {
          jqXHR.setRequestHeader('Authorization', `Bearer ${authToken}`);
        }
      }
    }
  });
}

/**
 * jQuery 를 이용하여 데이터를 요청합니다.
 * @param {string} url 전송 URL
 * @param {JQueryAjaxSettings} settings AJAX 전송 설정
 * @return {Promise<any | void>}
 */
export function requestData(url: string, settings: JQueryAjaxSettings): Promise<any | void> {
  return new Promise<any | void>((resolve, reject) => {
    $.ajax(url, settings)
      .done((data: any, textStatus: string, jqXHR: JQueryXHR) => {
        if ($.isPlainObject(data)) {
          resolve(data);
        }
      })
      .fail((jqXHR: JQueryXHR, textStatus, errorThrown) => {
        const error = jqXHR.responseJSON;
        reject(error);
      });
  });
}
