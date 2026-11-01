import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { PurchaseBill } from './pages/purchase-bill/purchase-bill';
import { AuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: Login },
  {
    path: 'purchase-bill',
    component: PurchaseBill,
    canActivate: [AuthGuard],
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' },
];
