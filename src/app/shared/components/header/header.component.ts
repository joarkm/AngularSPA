import { Component, OnInit } from '@angular/core';
import { MenuEntry } from '../../interfaces/menu-entry';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  menuIsCollapsed = true;
  iconPath = 'assets/images/icons/128x128/';
  // The concrete menu-entries the user has permissions to access
  userMenuEntries: MenuEntry[];

  menuEntries: MenuEntry[] = [
    {
      title: 'ADMIN',
      iconPath: this.iconPath + '053-user-35.png',
      menuName: 'ADMIN TOOLS',
      routeUrl: 'admin'
    },
    {
      title: 'MAIN',
      iconPath: '',
      menuName: 'MAIN',
      routeUrl: 'main'
    },
  ]

  constructor() { }

  ngOnInit() {
    // Do a permissions check
    this.userMenuEntries = this.menuEntries;
  }


}
