import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  RegisterMode: Boolean = true;

  constructor() { }

  ngOnInit() {
  }

  registerToogle() {
    this.RegisterMode = false;
  }

  RegisterModeHomeEvent(_RegisterMode: boolean) {
    this.RegisterMode = _RegisterMode;
  }

}
