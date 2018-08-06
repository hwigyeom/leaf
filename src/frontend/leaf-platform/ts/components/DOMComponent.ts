import { ComponentBase } from './ComponentBase';
import { IComponentOptions } from './IComponentOptions';

export abstract class DOMComponent extends ComponentBase {
  public abstract get $element(): JQuery<Element>;

  public abstract get $container(): JQuery<Element>;

  public abstract parseOptions(dummy: Element): IComponentOptions;
}
