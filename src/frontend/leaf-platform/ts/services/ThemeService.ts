export class ThemeService {
  public async changeTheme(theme: string, version?: string): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      const $oldTheme = $('head style[data-role=leaf-theme]');
      const oldTheme = $oldTheme.data('theme');

      if (!oldTheme || oldTheme !== theme) {
        // 테마가 아직 로드되지 않았거나 변경이 일어난 경우
        const style = document.createElement('style');
        style.textContent = `@import "/css/leaf.${theme}.css"`;
        style.setAttribute('data-role', 'leaf-theme');
        style.setAttribute('data-theme', theme);

        const interval = setInterval(() => {
          try {
            if (style.sheet['cssRules']) {
              $oldTheme.remove();
              clearInterval(interval);
              resolve();
            }
          } catch {
          }

        }, 10);

        if ($oldTheme.length === 0) {
          $('head > link:last-child').after(style);
        } else {
          $oldTheme.after(style);
        }
      } else {
        resolve();
      }
    });
  }
}
