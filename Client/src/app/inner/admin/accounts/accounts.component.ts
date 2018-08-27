import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { AdminService } from '../../../shared/services/admin.service';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.css']
})
export class AccountsComponent implements OnInit, OnDestroy {
  

  private accountOverviewSubscription: Subscription;
  private labels: string[] = [];
  private values: string[] = [];
  userDetails: any;
  userLabels: string[] = [];
  userValues: string[] = [];
  roleLabels: string[] = [];
  roleValues: string[] = [];

  constructor(
    private adminService: AdminService
  ) { }

  ngOnInit() {
    this.accountOverviewSubscription = this.adminService
    .getAccountsOverview()
    .subscribe(model => {
      this.userDetails = model;
      this.labels = Object.keys(model);
        this.labels.forEach(label => {
          this.values.push(model[label]);
        });
      this.filterRoleFields(model);
      this.setUserDetails();
      this.formatDetails();
    });
  }

  ngOnDestroy() {
    this.accountOverviewSubscription.unsubscribe();
  }

  private filterRoleFields(details: any) {
    this.roleLabels = this.labels.filter(label => label.match(/role/));
    this.roleLabels.forEach(roleLabel => this.roleValues.push(details[roleLabel]));
  }

  private setUserDetails() {
    // Set difference userLabels \ roleLabels
    this.userLabels = this.labels
      .filter(label => !this.roleLabels
        .some(roleLabel => roleLabel === label));
    
    // Set difference userValues \ roleValues
    this.userValues = this.values
      .filter(value => !this.roleValues
        .some(roleValue => roleValue === value));
  }

  private formatDetails(): void {
    const camel2title = (camelCase: string): string => {
      return camelCase
      .replace(/([A-Z])/g, (match) => ` ${match}`)
      .replace(/^./, (match) => match.toUpperCase()); 
    }

    const dash2camel = (dashCase: string): string => {
      let words = dashCase.split(/-|_/);
      let camelCase = words[0];
      for (let index = 1; index < words.length; index++) {
        const word = words[index];
        camelCase += firstToUpper(word);
      }
      return camelCase;
    }

    const firstToUpper = (input: string): string => {
      return input
      .replace(/^./, (match) => match.toUpperCase()); 
    }

    const removeDash = (input: string): string => {
      return input
      .replace(/-|_/g, ' ');
    }

    for (let index = 0; index < this.userLabels.length; index++) {
      const label = this.userLabels[index];
      this.userLabels[index] = camel2title(label);
    }

    for (let index = 0; index < this.roleLabels.length; index++) {
      const label = this.roleLabels[index];
      this.roleLabels[index] = camel2title(label);
    }
    for (let index = 0; index < this.roleValues.length; index++) {
      let value = this.roleValues[index];
      value = dash2camel(value);
      value = camel2title(value);
      this.roleValues[index] = value;
    }
  }

}
