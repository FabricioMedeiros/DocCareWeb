import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AccountModule } from './features/account/account.module';
import { FooterComponent } from './features/navigation/components/footer/footer.component';
import { HeaderComponent } from './features/navigation/components/header/header.component';
import { HomeComponent } from './features/navigation/components/home/home.component';
import { LogoComponent } from './features/navigation/components/logo/logo.component';
import { MenuLoginComponent } from './features/navigation/components/menu-login/menu-login.component';
import { SidebarComponent } from './features/navigation/components/sidebar/sidebar.component';

import { LocalStorageUtils } from './utils/localstorage';
import { SharedModule } from './shared/shared.module';

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
    AccountModule,
    SharedModule  
  ],
  providers: [LocalStorageUtils],
  bootstrap: [AppComponent]
})
export class AppModule { }
