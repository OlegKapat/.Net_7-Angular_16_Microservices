import { Injectable } from '@angular/core';
import { Constants } from './constants';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ReplaySubject } from 'rxjs';
import { User, UserManager, UserManagerSettings } from 'oidc-client';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private currentUserSource = new ReplaySubject<any>(1);
  currentUser$ = this.currentUserSource.asObservable();
  private manager = new UserManager(getClientSettings());
  private user!: User | null;
  token = "";
  access_token = "";

  constructor(private http: HttpClient, private router: Router) {
    this.manager.getUser().then(user => {
      this.user = user;
      this.currentUserSource.next(this.isAuthenticated());
    });
  }

  isAuthenticated(): boolean {
    return this.user != null && !this.user.expired;
  }

  login() {
    return this.manager.signinRedirect();
  }

  async signout() {
    await this.manager.signoutRedirect();
  }

  get authorizationHeaderValue(): string {
    console.log(this.token);
    console.log(this.access_token);
    return `${this.token} ${this.access_token}`;
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }
  public finishLogin = (): Promise<User> => {
    return this.manager.signinRedirectCallback()
    .then(user => {
      this.currentUserSource.next(this.checkUser(user));
      this.token = user.token_type;
      this.access_token = user.access_token;
      return user;
    })
  }

  public finishLogout = () => {
    this.user = null;
    return this.manager.signoutRedirectCallback();
  }

  private checkUser = (user : User): boolean => {
    console.log('inside check user');
    console.log(user);
    return !!user && !user.expired;
  }

}

export function getClientSettings(): UserManagerSettings {
  return {
    includeIdTokenInSilentRenew: true,
    automaticSilentRenew: true,
    silent_redirect_uri: `${Constants.clientRoot}/assets/silent-callback.html`,
    authority: Constants.idpAuthority,
    client_id: Constants.clientId,
    redirect_uri: `${Constants.clientRoot}/signin-callback`,
    scope: "openid profile eshoppinggateway",
    response_type: "code",
    post_logout_redirect_uri: `${Constants.clientRoot}/signout-callback`
  };
}
