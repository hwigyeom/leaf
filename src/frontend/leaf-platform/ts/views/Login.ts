import { IView } from './IView';

export class Login implements IView {
  public async initialize(): Promise<void> {
    console.log('login view');
    return undefined;
  }
}
