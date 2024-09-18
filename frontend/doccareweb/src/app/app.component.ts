import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(private router: Router) {}

  title = 'doccareweb';

  showOnlyComponentRoute(): boolean {
    const specialRoutes = ['/login', '/register'];
    return specialRoutes.includes(this.router.url);
  }
}
