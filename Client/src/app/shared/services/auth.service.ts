import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Credentials } from '../interfaces/credentials.interface';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/throw';
import { debug } from 'util';

@Injectable()
export class AuthService {

  private _loggedInSource = new BehaviorSubject<boolean>(false);
  loggedIn$ = this._loggedInSource.asObservable();
  private loggedIn = false;
  isAdmin: boolean;

  constructor(
    private http: HttpClient
  ) { }

  login(credentials: Credentials): Observable<boolean> {
    // const uri = 'http://localhost:5000/api/auth/login';
    const uri = 'api/auth/login';
    return this.http
    .post<boolean>(uri, credentials)
    .map(res => {
      this.loggedIn = true;
      this._loggedInSource.next(true);
      this.isAdmin = res;
      return res;
    })
    .catch(this.handleError);
  }

  logout() {
    this.loggedIn = false;
    this._loggedInSource.next(false);
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

}
