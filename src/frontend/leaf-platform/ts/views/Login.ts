import { Checkbox } from '../components/Checkbox';
import { Password } from '../components/Password';
import { Textbox } from '../components/Textbox';
import { InjectService } from '../dependencyInjection/InjectService';
import { InjectTemplate } from '../dependencyInjection/InjectTemplate';
import { ResolveComponent } from '../dependencyInjection/ResolveComponent';
import { EnvironmentService } from '../environments/EnvironmentService';
import { IView } from './IView';

export class Login implements IView {

  @InjectTemplate('Login')
  public template: any;

  @InjectService(EnvironmentService.environmentServiceKey)
  private env: EnvironmentService;

  @ResolveComponent('sid')
  private sid: Checkbox;

  @ResolveComponent('uid')
  private uid: Textbox;

  @ResolveComponent('pwd')
  private pwd: Password;

  public get templateData() {
    return this.env.login;
  }

  public load(): void {
    console.log(this.sid);
    this.uid.text = 'whistle';
    this.pwd.text = '1';
  }
}
