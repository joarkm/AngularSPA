// Based on https://github.com/mmacneil/AngularASPNETCore2WebApiAuth/blob/master/src/src/app/account/login-form/login-form.component.ts

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Credentials } from 'app/shared/interfaces/credentials.interface';
import { AuthService } from 'app/shared/services/auth.service';

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
  ) {}

  private logError(error: any) {
    this.errors = error;
    this.credentials.password = '';
  }

  async login({ value, valid }: { value: Credentials; valid: boolean }) {
    this.submitted = true;
    this.errors = '';
    if (valid) {
      this.isRequesting = true;

      await this.authService
        .login(value)
        .toPromise()
        .then(isAdmin => {
          if (isAdmin) {
            this.router.navigateByUrl('/admin');
          } else {
            this.router.navigateByUrl('/main');
          }
        }, error => this.logError(error));
      if (this.errors) {
        this.isRequesting = false;
      }
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
