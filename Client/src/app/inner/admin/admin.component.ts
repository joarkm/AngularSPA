import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  iconPath: string;

  constructor() { 
    this.iconPath = 'assets/images/icons/128x128';
  }

  ngOnInit() {
  }

}
