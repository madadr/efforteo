import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  title = 'Efforteo';

  constructor() { }

  ngOnInit() {
  }

  isAuthenticated() {
    // TODO: get value from service
    return false;
  }
}
