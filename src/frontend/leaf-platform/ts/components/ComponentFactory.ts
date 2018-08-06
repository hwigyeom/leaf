import { ComponentBase } from './ComponentBase';
import { convertibleDOMComponents } from './convertibleDOMComponents';
import { DOMComponent } from './DOMComponent';
import { IComponentOptions } from './IComponentOptions';

// jQuery 슈도 셀렉터를 확장
// @ts-ignore
$.extend($['expr'][':'], {
  leaf: (e) => {
    return /^leaf:/i.test(e.nodeName);
  }
});

export class ComponentFactory {
  public static async convertComponents(container: Element) {
    const $container = $(container);
    // noinspection CssInvalidPseudoSelector
    const $dummies = $container.find(':leaf');
    const convertItems: Array<{ dummy: Element, component: DOMComponent }> = [];

    $dummies.each((idx, dummy) => {
      const componentClass = ComponentFactory.getComponentClass(dummy);
      if (componentClass) {
        convertItems.push({
          dummy,
          component: ComponentFactory.createComponent(componentClass, dummy) as DOMComponent
        });
      }
    });

    for (const item of convertItems) {
      await item.component.initialize();
      $(item.dummy).after(item.component.$container).detach();
    }
  }

  public static createComponent<T extends ComponentBase>(constructor: new(options?: IComponentOptions) => T,
                                                         arg: Element | IComponentOptions): T {
    let options: IComponentOptions;
    let component: T;

    if ($.isPlainObject(arg)) {
      options = arg as IComponentOptions;
      component = new constructor(options);
    } else {
      component = new constructor();
      if (component instanceof DOMComponent) {
        options = (component as DOMComponent).parseOptions(arg as Element);
        component.setOptions(options);
      }
    }

    return component;
  }

  private static getComponentClass(element: Element): { new(...args: any[]): ComponentBase } {
    const tagName = element.tagName;
    const controlName = tagName.split(':')[1];
    return convertibleDOMComponents[controlName.toLowerCase()];
  }
}
