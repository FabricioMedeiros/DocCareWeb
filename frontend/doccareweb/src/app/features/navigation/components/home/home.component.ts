import { Component, Renderer2, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnDestroy {
  constructor(private renderer: Renderer2) {
    this.renderer.setStyle(document.body, 'background-color', '#ffffff');
  }

  ngOnDestroy(): void {
    this.renderer.removeStyle(document.body, 'background-color');
  }
}
