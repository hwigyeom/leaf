import { DOMComponent } from './DOMComponent';
import { IComponentOptions } from './IComponentOptions';

export class Panel extends DOMComponent {

  public get $container(): JQuery<Element> {
    return undefined;
  }

  public get $element(): JQuery<Element> {
    return undefined;
  }

  public async initialize(): Promise<void> {
    return new Promise<void>((resolve) => {

      resolve();
    });
  }

  public parseOptions(dummy: Element): IComponentOptions {
    const options: IComponentOptions = {id: dummy.id};
    $.each(dummy.attributes, (idx, item) => {
      if (item.specified) {
        options[item.name] = item.value;
      }
    });
    return options;
  }

  public setOptions(options: IComponentOptions): void {
  }
}
