import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AccountAppComponent } from './account.app.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { canActivate, canDeactivate } from './services/account.guard';

const routes: Routes = [
    {
        path: '', component: AccountAppComponent,
        children: [
            { path: 'register', component: RegisterComponent, canActivate: [canActivate], canDeactivate: [canDeactivate] },
            { path: 'login', component: LoginComponent, canActivate: [canActivate] }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AccountRoutingModule { }
