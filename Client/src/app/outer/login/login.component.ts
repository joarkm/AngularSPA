// Based on https://github.com/mmacneil/AngularASPNETCore2WebApiAuth/blob/master/src/src/app/account/login-form/login-form.component.ts

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Credentials } from 'app/shared/interfaces/credentials.interface';
import { AuthService } from 'app/shared/services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ROLES } from '../../shared/constants/roles';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  login_title = 'LOGIN';

  brandNew: boolean;
  errors: string;
  isRequesting: boolean;
  submitted = false;
  credentials: Credentials = { userName: '', password: '' };

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private jwtHelper: JwtHelperService
  ) {}

  private logError(error: any) {
    this.errors = error;
    this.credentials.password = '';
  }

  async login({ value, valid }: { value: Credentials; valid: boolean }) {
    this.submitted = true;
    this.errors = '';
    const model = value;
    
    if (valid) {
      this.isRequesting = true;

      await this.authService
        .login(model)
        .toPromise()
        .then(
        jwt => {
          const jwt_decoded = this.jwtHelper.decodeToken(JSON.stringify(jwt));
          console.log(jwt_decoded);
          this.handleRedirection();
        },
        error => this.logError(error));
      if (this.errors) {
        this.isRequesting = false;
      }
    }
  }

  private handleRedirection(): void {
    // Check token for authentication and redirect to appropriate site
    console.log({token: this.jwtHelper.tokenGetter()});
    console.log({decoded_token: this.jwtHelper.decodeToken(this.jwtHelper.tokenGetter())});

    const jwt = this.jwtHelper.decodeToken(this.jwtHelper.tokenGetter());
    const role: string = jwt.rol;
    if (role === ROLES.SUPER_ADMIN || role === ROLES.ADMIN) {
      this.router.navigateByUrl('/admin');
    } else if (role === ROLES.REGULAR_USER) {
      this.router.navigateByUrl('/main');
    } else {
      this.router.navigateByUrl('/main');
    }
  }

  ngOnInit() {
    // subscribe to router event
    this.activatedRoute.queryParams.subscribe((param: any) => {
      this.brandNew = param['brandNew'];
      // this.credentials.userName = param['userName'];
    });
  }
}
