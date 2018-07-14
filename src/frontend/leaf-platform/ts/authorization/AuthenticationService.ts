import * as jwtDecode from 'jwt-decode';
import { InjectService } from '../dependencyInjection/InjectService';
import { EnvironmentService } from '../environments/EnvironmentService';

export class AuthenticationService {
  public static authenticationServiceKey: string = '_authentication_';

  @InjectService(EnvironmentService.environmentServiceKey)
  private env: EnvironmentService;

  public get isAuthenticated(): boolean {
    return !!this.getAuthToken();
  }

  private getAuthToken() {
    const authTokenString = sessionStorage.getItem(this.env.global.keys.token);

    if (!authTokenString) {
      return;
    }
    const authToken = jwtDecode(authTokenString);
    return authToken;
  }
}
