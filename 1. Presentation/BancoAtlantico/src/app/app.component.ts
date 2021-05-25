import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from './_services/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'BancoAtlantico';

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService
  ) {
    console.log("init app");
    this.authenticationService.login();

    this.router.navigate(['/atmlist']);
  }
}
