import { Component, OnDestroy, Renderer2 } from '@angular/core';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.css']
})
export class NotFoundComponent implements OnDestroy {
  constructor(private renderer: Renderer2) {
    this.renderer.setStyle(document.body, 'background-color', '#ffffff');
  }

  ngOnDestroy(): void {
    this.renderer.removeStyle(document.body, 'background-color');
  }
}
