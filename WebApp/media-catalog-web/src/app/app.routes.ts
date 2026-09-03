import { Routes } from '@angular/router';
import { Login } from './features/auth/pages/login/login';
import { Dashboard } from './features/dashboard/pages/dashboard/dashboard';
import { NotFound } from './shared/components/not-found/not-found';


export const routes: Routes = [
    { path: '', redirectTo: 'auth/login', pathMatch: 'full' },
    { path: 'auth/login', component: Login },
    { path: 'dashboard/:section', component: Dashboard },
    { path: 'dashboard', redirectTo: 'dashboard/home', pathMatch: 'full' },
    { path: '**', component: NotFound },
];
