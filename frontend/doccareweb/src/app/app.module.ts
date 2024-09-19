import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { HeaderComponent } from './features/navigation/header/header.component';
import { SidebarComponent } from './features/navigation/sidebar/sidebar.component';
import { FooterComponent } from './features/navigation/footer/footer.component';
import { LogoComponent } from './features/navigation/logo/logo.component';
import { HomeComponent } from './features/navigation/home/home.component';
import { MenuLoginComponent } from './features/navigation/menu-login/menu-login.component';

import { LocalStorageUtils } from './utils/localstorage';
import { AccountModule } from './features/account/account.module';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    MenuLoginComponent,
    SidebarComponent,
    FooterComponent,
    LogoComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(), 
    AccountModule   
  ],
  providers: [LocalStorageUtils],
  bootstrap: [AppComponent]
})
export class AppModule { }
