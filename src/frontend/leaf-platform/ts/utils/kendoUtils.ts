import * as debug from './debugUtils';

export function fixKendoCalendar() {
  const calendar = kendo['calendar'];
  let monthView: any;
  if (Array.isArray(calendar.views)) {
    for (const view of calendar.views) {
      if (view.name === 'month') {
        monthView = view;
        break;
      }
    }

    if (monthView) {
      monthView.title = (date, min, max, culture) => {
        return kendo.toString(date, 'Y', culture);
      };
    }
  }
}

export async function setKendoCulture(culture: string): Promise<void> {
  return new Promise<void>((resolve) => {
    const $old = $('script[src][data-role=kendo-culture]');
    const oldCulture = $old.data('culture');

    if (oldCulture !== culture) {
      const script = document.createElement('script');
      const $script = $(script);

      $('script[data-role=kendo]').after($script);

      $script.on('load', () => {
        debug.log('## Leaf Core: Kendo UI 지역화 리소스 로드 ##');
        kendo.culture(culture);
        resolve();
      });
      $script.attr('src', `/js/cultures/kendo.culture.${culture}.min.js?v${kendo.version}`);
      $script.attr('data-role', 'kendo-culture');
      $script.attr('data-culture', culture);
      $script.attr('charset', 'UTF-8');

      if ($old.length > 0) {
        $old.remove();
      }
    }
  });
}
