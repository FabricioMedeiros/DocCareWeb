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
import { BsDatepickerModule, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { defineLocale } from 'ngx-bootstrap/chronos'; 
import { ptBrLocale } from 'ngx-bootstrap/locale';

import { AccountModule } from './features/account/account.module';
import { FooterComponent } from './features/navigation/components/footer/footer.component';
import { HeaderComponent } from './features/navigation/components/header/header.component';
import { HomeComponent } from './features/navigation/components/home/home.component';
import { LogoComponent } from './features/navigation/components/logo/logo.component';
import { MenuLoginComponent } from './features/navigation/components/menu-login/menu-login.component';
import { SidebarComponent } from './features/navigation/components/sidebar/sidebar.component';
import { NotFoundComponent } from './features/navigation/components/not-found/not-found.component';
import { ServiceUnavailableComponent } from './features/navigation/components/service-unavailable/service-unavailable.component';

import { LocalStorageUtils } from './core/utils/localstorage';
import { SharedModule } from './shared/shared.module';

import { NavigationService } from './core/services/navigation.service';
import { ErrorInterceptor } from './core/interceptors/error.handler.service';

export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
];

defineLocale('pt-br', ptBrLocale);

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
    ToastrModule.forRoot({ toastClass: 'ngx-toastr custom-toast', preventDuplicates: true }),
    NgxSpinnerModule.forRoot(),
    ModalModule.forRoot(),
    BsDatepickerModule.forRoot(),
    BsDropdownModule.forRoot(),
    TooltipModule.forRoot(),
    PopoverModule.forRoot(),
    AccountModule,
    SharedModule
  ],
  providers: [LocalStorageUtils, BsModalService, httpInterceptorProviders, 
    NavigationService
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule { 
  constructor(private localeService: BsLocaleService) { 
    this.localeService.use('pt-br');
  }
}
