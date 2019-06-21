import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  @Output() RegisterModeEvent = new EventEmitter();

  constructor(private auth: AuthService) { }

  ngOnInit() {
  }

  register() {
      this.auth.register(this.model).subscribe(() => {
          console.log('registered!');
      }, error => {
        console.log(error);
      });
  }

  cancel() {
    this.RegisterModeEvent.emit(true);
  }
}
