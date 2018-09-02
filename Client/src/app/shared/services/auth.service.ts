import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Credentials } from '../interfaces/credentials.interface';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/throw';
import { debug } from 'util';
import { BaseService } from './base.service';
import { JwtResponse } from 'app/shared/interfaces/jwt-response';
import { LOCALSTORAGE } from '../constants/localstorage';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ROLES } from '../constants/roles';

@Injectable()
export class AuthService extends BaseService {

  private _loggedInSource = new BehaviorSubject<boolean>(false);
  loggedIn$: Observable<boolean> = this._loggedInSource.asObservable();
  private loggedIn = false;

  private authUrl = [this.baseUri, 'auth'].join('/');
  private loginUrl = [this.authUrl, 'login'].join('/');
  private refreshUrl = [this.authUrl, 'token', 'refresh'].join('/');

  constructor(
    private http: HttpClient,
    private jwtHelper: JwtHelperService
  ) {
    super();
   }

  login(credentials: Credentials): Observable<JwtResponse> {
    return this.http
    .post<JwtResponse>(this.loginUrl, credentials)
    .map(jwt => {
      this.loggedIn = true;
      // Broadcast login state change
      this._loggedInSource.next(true);
      // Store received jwt in localstorage
      localStorage.setItem(LOCALSTORAGE.JWT, JSON.stringify(jwt));
    })
    .catch(this.handleError);
  }

  logout() {
    this.loggedIn = false;
    // Broadcast login state change
    this._loggedInSource.next(false);
    // Remove jwt from localstorage
    localStorage.removeItem(LOCALSTORAGE.JWT);
  }

  isLoggedIn(): boolean {
    return this.loggedIn;
  }

  protected /*override*/ handleError(error: HttpErrorResponse | any) {
    let errMsg: any;
    debug;
    if (error instanceof HttpErrorResponse) {
      const ERROR = error as HttpErrorResponse;
      if (ERROR.status >= 500) {
        errMsg = `${ERROR.status} ${ERROR.statusText}`;
      } else if (ERROR.status >= 400) {
        errMsg = 'Login failed';
      } else if (ERROR.error.error instanceof SyntaxError) {
        errMsg = 'Application error';
      } else {
        errMsg = error.message ? error.message : error.toString();
      }
      return Observable.throw(errMsg);
    }
  }

  refreshToken(): Observable<JwtResponse> {
    const jwt = this.jwtHelper.tokenGetter();
    return this.http
    .post<JwtResponse>(this.refreshUrl, jwt)
    .map(jwt => {
      this.loggedIn = true;
      // Broadcast login state change
      this._loggedInSource.next(true);
      // Store received jwt in localstorage
      localStorage.setItem(LOCALSTORAGE.JWT, JSON.stringify(jwt));
    })
    .catch(this.handleError);
  }

  isAdmin(): boolean {
    const jwt = this.jwtHelper.decodeToken(this.jwtHelper.tokenGetter());
    const role: string = jwt.rol;
    return role === ROLES.SUPER_ADMIN || role === ROLES.ADMIN;
  }
  
}
