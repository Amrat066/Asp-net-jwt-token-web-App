import { Routes } from '@angular/router';
import { LoginComponent } from './component/auth/login.component';
import { RegisterComponent } from './component/auth/register.component';
import { ItemMasterComponent } from './component/item-master/item-master.component';
import { authGuard } from './Services/auth.guard';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'itemMaster', component: ItemMasterComponent, canActivate: [authGuard] },
    { path: '**', redirectTo: 'login' }
];
