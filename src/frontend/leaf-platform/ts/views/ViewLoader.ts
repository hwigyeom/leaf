import { ComponentFactory } from '../components/ComponentFactory';
import { IView } from './IView';

export class ViewLoader {
  public static viewLoaderKey = '_viewLoader_';

  public async loadView(view: IView, target: Element, embedded: boolean = false): Promise<void> {

    const $target = $(target);

    view['_container_'] = $target;

    if (embedded) {
      if (typeof view.template === 'function') {
        const templateData = view.templateData || {};
        $target.html(view.template.call(view, templateData));
      } else {
        $target.html(view.template);
      }
    }

    if (typeof view.preInit === 'function') {
      await view.preInit.call(view);
    }

    await ComponentFactory.convertComponents(target);

    if (typeof view.initialize === 'function') {
      await view.initialize.call(view);
    }

    if (typeof view.load === 'function') {
      await view.load.call(view);
    }
  }

  public async unloadView(view: IView): Promise<void> {
    return new Promise<void>((resolve) => {
      const $container = view['_container_'] as JQuery;
      $container.find('.leaf-component');
      resolve();
    });
  }
}
