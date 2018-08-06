import { IComponentOptions } from './IComponentOptions';

export abstract class ComponentBase {
  public abstract async initialize(): Promise<void>;

  public abstract setOptions(options: IComponentOptions): void;
}
