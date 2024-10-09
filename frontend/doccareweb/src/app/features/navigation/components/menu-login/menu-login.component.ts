import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LocalStorageUtils } from 'src/app/core/utils/localstorage';

@Component({
  selector: 'app-menu-login',
  templateUrl: './menu-login.component.html',
  styleUrls: ['./menu-login.component.css']
})
export class MenuLoginComponent {

  token: string | null = "";
  user: any;
  email: string | null = "";
  localStorageUtils = new LocalStorageUtils();

  constructor(private router: Router) {  }

  isUserLoggedIn(): boolean {
    this.token = this.localStorageUtils.getTokenUser() ;
    this.user = this.localStorageUtils.getUser();
    this.email = this.localStorageUtils.getEmailUser();

    return this.token !== null;
  }

  logout() {
    this.localStorageUtils.clearLocalUserData();
    this.router.navigate(['/account/login']);
  }
}
