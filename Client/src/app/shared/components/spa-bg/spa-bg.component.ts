import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-spa-bg',
  templateUrl: './spa-bg.component.html',
  styleUrls: ['./spa-bg.component.css']
})
export class SPABgComponent implements OnInit {
  iconPath = 'assets/images/icons/128x128/white/';

  @Input() icon: string;

  @Input() header: string;

  constructor() { }

  ngOnInit() {
  }

}
