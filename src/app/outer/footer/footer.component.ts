import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit {

  iconPath: string;

  constructor() { }

  ngOnInit() {
    this.iconPath = 'assets/images/icons';
  }

}
