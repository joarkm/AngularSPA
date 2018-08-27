import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './base.service';

@Injectable()
export class AdminService extends BaseService {

  private adminBaseUri = `${this.baseUri}/admin`;
  private accountsOverviewUri = `${this.adminBaseUri}/accounts/overview`;

  constructor(
    private http: HttpClient
  ) { 
    super();
  }

  getAccountsOverview() {
    return this.http
    .get(this.accountsOverviewUri)
    .catch(this.handleError);
  }

}
