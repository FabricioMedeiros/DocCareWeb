import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './features/navigation/header/header.component';
import { SidebarComponent } from './features/navigation/sidebar/sidebar.component';
import { FooterComponent } from './features/navigation/footer/footer.component';
import { LogoComponent } from './features/navigation/logo/logo.component';
import { HomeComponent } from './features/navigation/home/home.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    SidebarComponent,
    FooterComponent,
    LogoComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
