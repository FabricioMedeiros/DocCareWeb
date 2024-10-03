import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NavigationService {
  private allowedNavigation = false;

  allowNavigation() {
    this.allowedNavigation = true;
  }

  isNavigationAllowed(): boolean {
    return this.allowedNavigation;
  }

  resetNavigation() {
    this.allowedNavigation = false;
  }
}
