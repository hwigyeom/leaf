export interface IView {
  template: any;
  templateData?: {};
  preInit?: () => Promise<void> | void;
  initialize?: () => Promise<void> | void;
  load?: () => Promise<void> | void;
}
