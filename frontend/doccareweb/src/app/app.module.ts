import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';

import { AccountModule } from './features/account/account.module';
import { FooterComponent } from './features/navigation/components/footer/footer.component';
import { HeaderComponent } from './features/navigation/components/header/header.component';
import { HomeComponent } from './features/navigation/components/home/home.component';
import { LogoComponent } from './features/navigation/components/logo/logo.component';
import { MenuLoginComponent } from './features/navigation/components/menu-login/menu-login.component';
import { SidebarComponent } from './features/navigation/components/sidebar/sidebar.component';

import { LocalStorageUtils } from './utils/localstorage';
import { SharedModule } from './shared/shared.module';
import { ErrorInterceptor } from './services/error.handler.service';
import { ServiceUnavailableComponent } from './features/navigation/components/service-unavailable/service-unavailable.component';
import { NotFoundComponent } from './features/navigation/components/not-found/not-found.component';
import { NavigationService } from './services/navigation.service';

export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
];


@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    MenuLoginComponent,
    SidebarComponent,
    FooterComponent,
    LogoComponent,
    HomeComponent,
    ServiceUnavailableComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({ toastClass: 'ngx-toastr custom-toast' }),
    NgxSpinnerModule.forRoot(),
    ModalModule.forRoot(),
    AccountModule,
    SharedModule
  ],
  providers: [LocalStorageUtils, BsModalService, httpInterceptorProviders, NavigationService],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule { }
