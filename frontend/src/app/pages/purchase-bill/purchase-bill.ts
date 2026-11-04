import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Message } from '../../shared/message/message';
import { AuthService, Location, PurchaseBillItemRequest } from '../../core/services/auth.service';

interface BillItem {
  itemName: string;
  batch: string;
  standardCost: number;
  standardPrice: number;
  margin: number;
  quantity: number;
  freeQty: number;
  discount: number;
  totalCost: number;
  totalSelling: number;
}

@Component({
  selector: 'app-purchase-bill',
  standalone: true,
  imports: [CommonModule, FormsModule, Message],
  templateUrl: './purchase-bill.html',
  styleUrl: './purchase-bill.css',
})
export class PurchaseBill implements OnInit {
  locations: Location[] = [];

  itemOptions = ['Mango', 'Apple', 'Banana', 'Orange', 'Grapes', 'Kiwi', 'Strawberry'];

  selectedItem = '';
  selectedBatch = '';

  standardCost = 100;
  standardPrice = 150;
  margin = 50;
  quantity = 1;
  freeQty = 0;
  discount = 0;

  billItems: BillItem[] = [];

  errorMessage = '';

  isSaving = false;
  successMessage = '';

  constructor(
    private authService: AuthService,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.loadLocations();
  }

  loadLocations(): void {
    this.authService.getLocations().subscribe({
      next: (data) => {
        this.locations = data;

        //console.log('Locations received:', this.locations);

        this.cdr.markForCheck();
      },
      error: () => {
        this.errorMessage = 'Unable to load locations.';

        this.cdr.markForCheck();
      },
    });
  }

  updateMargin(): void {
    this.margin = this.standardPrice - this.standardCost;
  }

  addItem(): void {
    this.errorMessage = '';

    if (!this.selectedItem || !this.selectedBatch) {
      this.errorMessage = 'Please select an item and batch.';
      return;
    }

    if (
      this.quantity <= 0 ||
      this.standardCost < 0 ||
      this.standardPrice < 0 ||
      this.discount < 0 ||
      this.discount > 100 ||
      this.freeQty < 0
    ) {
      this.errorMessage = 'Please enter valid item values.';
      return;
    }

    const totalCost = this.standardCost * this.quantity * (1 - this.discount / 100);

    const totalSelling = this.standardPrice * this.quantity;

    const newItem: BillItem = {
      itemName: this.selectedItem,
      batch: this.selectedBatch,
      standardCost: this.standardCost,
      standardPrice: this.standardPrice,
      margin: this.margin,
      quantity: this.quantity,
      freeQty: this.freeQty,
      discount: this.discount,
      totalCost,
      totalSelling,
    };

    this.billItems.push(newItem);

    this.selectedItem = '';
    this.selectedBatch = '';
    this.quantity = 1;
    this.freeQty = 0;
    this.discount = 0;
  }

  get totalItems(): number {
    return this.billItems.length;
  }

  get totalQuantity(): number {
    return this.billItems.reduce((total, item) => total + item.quantity, 0);
  }

  savePurchaseBill(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (this.billItems.length === 0) {
      this.errorMessage = 'Please add at least one item before saving.';
      this.cdr.markForCheck();
      return;
    }

    const items: PurchaseBillItemRequest[] = this.billItems.map((item) => ({
      itemName: item.itemName,
      batchName: item.batch,
      standardCost: item.standardCost,
      standardPrice: item.standardPrice,
      margin: item.margin,
      quantity: item.quantity,
      freeQuantity: item.freeQty,
      discount: item.discount,
    }));

    this.isSaving = true;
    this.cdr.markForCheck();

    //console.time('Purchase bill save API time');

    this.authService.savePurchaseBill(items).subscribe({
      next: (response) => {
        //console.timeEnd('Purchase bill save API time');

        this.isSaving = false;
        this.successMessage = `${response.message} Bill ID: ${response.billId}`;

        // Clear the form table after a successful save.
        this.billItems = [];

        //console.log('Purchase bill saved successfully:', response);

        this.cdr.markForCheck();
      },

      error: (error) => {
        //console.timeEnd('Purchase bill save API time');

        this.isSaving = false;
        this.errorMessage = error.error?.message || 'Unable to save the purchase bill.';

        //console.error('Purchase bill save error:', error);

        this.cdr.markForCheck();
      },
    });
  }
}
