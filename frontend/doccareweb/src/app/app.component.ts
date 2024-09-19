import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'doccareweb';
  showOnlyComponentRoute: boolean = false;

  constructor(private router: Router) {
    this.router.events.pipe(
      filter((event): event is NavigationEnd => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      this.updateShowOnlyComponentRoute(event.urlAfterRedirects);
    });
  }

  private updateShowOnlyComponentRoute(url: string): void {
    const specialRoutes = ['/account/login', '/account/register'];
    this.showOnlyComponentRoute = specialRoutes.includes(url);
  }
}
