import { InjectTemplate } from '../dependencyInjection/InjectTemplate';
import { IView } from './IView';

export class Main implements IView {

  @InjectTemplate('Login')
  public template: any;

  public async initialize(): Promise<void> {
    return new Promise<void>((resolve) => {
      console.log('main view');
      resolve();
    });
  }
}
