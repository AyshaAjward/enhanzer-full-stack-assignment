import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { PurchaseBill } from './pages/purchase-bill/purchase-bill';
import { Dashboard } from './pages/dashboard/dashboard';
import { AuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: Login },
  {
    path: 'purchase-bill',
    component: PurchaseBill,
    canActivate: [AuthGuard],
  },
  {
    path: 'dashboard',
    component: Dashboard,
    canActivate: [AuthGuard],
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' },
];
