import { Component, OnDestroy, Renderer2 } from '@angular/core';

@Component({
  selector: 'app-service-unavailable',
  templateUrl: './service-unavailable.component.html',
  styleUrls: ['./service-unavailable.component.css']
})
export class ServiceUnavailableComponent implements OnDestroy {
  constructor(private renderer: Renderer2) {
    this.renderer.setStyle(document.body, 'background-color', '#ffffff');
  }

  ngOnDestroy(): void {
    this.renderer.removeStyle(document.body, 'background-color');
  }
}
