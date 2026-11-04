import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { forkJoin, timeout, finalize } from 'rxjs';
import {
  AuthService,
  PurchaseOrder,
  PurchaseOrderItem,
  ItemQuantity,
} from '../../core/services/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  latestOrders: PurchaseOrder[] = [];
  oldestItems: PurchaseOrderItem[] = [];
  itemQuantities: ItemQuantity[] = [];

  isLoading = false;
  errorMessage = '';

  chartColors: string[] = [
    '#3978d4',
    '#48a999',
    '#e9a23b',
    '#8b70c9',
    '#e47777',
    '#54a6c7',
    '#a4b85c',
    '#d18cba',
    '#718096',
    '#c18b54',
  ];

  constructor(
    private authService: AuthService,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    console.time('Dashboard total loading time');
    this.loadDashboard();
  }

  loadDashboard() {
    this.isLoading = true;
    this.errorMessage = '';

    this.cdr.markForCheck();

    forkJoin({
      orders: this.authService.getLatestOrders(),
      items: this.authService.getOldestItems(),
      quantities: this.authService.getItemQuantities(),
    })
      .pipe(
        timeout({ first: 15000 }),
        finalize(() => {
          this.isLoading = false;

          // Notify Angular that the view needs updating
          this.cdr.markForCheck();
        }),
      )
      .subscribe({
        next: (result) => {
          this.latestOrders = result.orders;
          this.oldestItems = result.items;
          this.itemQuantities = result.quantities;

          // console.log('Dashboard data received:', result);
          // console.log('Latest orders:', this.latestOrders.length);
          // console.log('Oldest items:', this.oldestItems.length);
          // console.log('Item quantities:', this.itemQuantities.length);

          // Notify Angular after receiving the API response
          this.cdr.markForCheck();
        },

        error: (error) => {
          console.error('Dashboard loading error:', error);

          this.errorMessage =
            error.name === 'TimeoutError'
              ? 'The dashboard is taking too long to load. Please try again.'
              : error.error?.message || 'Unable to load dashboard data.';

          this.cdr.markForCheck();
        },
      });
  }

  get totalQuantity(): number {
    return this.itemQuantities.reduce((total, item) => total + item.quantity, 0);
  }

  getItemPercentage(quantity: number): number {
    if (this.totalQuantity === 0) {
      return 0;
    }

    return (quantity / this.totalQuantity) * 100;
  }

  getItemColor(index: number): string {
    return this.chartColors[index % this.chartColors.length];
  }

  getDonutGradient(): string {
    if (this.totalQuantity === 0) {
      return 'conic-gradient(#e5e7eb 0% 100%)';
    }

    let currentPercentage = 0;

    const segments = this.itemQuantities.map((item, index) => {
      const start = currentPercentage;
      currentPercentage += this.getItemPercentage(item.quantity);

      return `${this.getItemColor(index)} ${start}% ${currentPercentage}%`;
    });

    return `conic-gradient(${segments.join(', ')})`;
  }
}
