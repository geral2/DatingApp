import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(private auth: AuthService) {
   }

  ngOnInit() {
  }

  login() {
            this.auth.login(this.model).subscribe(next => {
                    console.log('Logged Suscefully!');
            }, error => {
              console.log('logged failed!');
            });
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !!token;

  }

  logout() {
      localStorage.removeItem('token');
  }

}
