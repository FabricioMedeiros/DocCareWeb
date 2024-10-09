import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

import { LocalStorageUtils } from '../utils/localstorage';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private localStorageUtils: LocalStorageUtils, private router: Router) {}

  isAuthenticated(): boolean {
    const token = this.localStorageUtils.getTokenUser();
    return !!token;
  }

  redirectToHome(): void {
    this.router.navigate(['/home']);
  }

  redirectToLogin(returnUrl: string): void {
    this.router.navigate(['/account/login'], { queryParams: { returnUrl } });
  }
}
