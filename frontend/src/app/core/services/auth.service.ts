import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface LoginResponse {
  message: string;
  userCode: string;
  locations: {
    Location_Code: string;
    Location_Name: string;
  }[];
}

export interface Location {
  location_Code: string;
  location_Name: string;
}

export interface PurchaseBillItemRequest {
  itemName: string;
  batchName: string;
  standardCost: number;
  standardPrice: number;
  margin: number;
  quantity: number;
  freeQuantity: number;
  discount: number;
}

export interface SavePurchaseBillResponse {
  message: string;
  billId: number;
  totalItems: number;
  totalQuantity: number;
}

export interface PurchaseOrder {
  id: number;
  netAmount: number;
  itemCount: number;
}

export interface PurchaseOrderItem {
  purchaseOrderId: number;
  itemName: string;
  quantity: number;
}

export interface ItemQuantity {
  itemName: string;
  quantity: number;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = 'http://localhost:5286/api';

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${this.apiUrl}/Auth/login`,
      { email, password },
      { withCredentials: true },
    );
  }

  getLocations(): Observable<Location[]> {
    return this.http.get<Location[]>(`${this.apiUrl}/Locations`, { withCredentials: true });
  }

  savePurchaseBill(items: PurchaseBillItemRequest[]) {
    return this.http.post<SavePurchaseBillResponse>(
      `${this.apiUrl}/PurchaseBills`,
      { items },
      { withCredentials: true },
    );
  }

  getLatestOrders(): Observable<PurchaseOrder[]> {
    return this.http.get<PurchaseOrder[]>(`${this.apiUrl}/PurchaseBills/latest`, {
      withCredentials: true,
    });
  }

  getOldestItems(): Observable<PurchaseOrderItem[]> {
    return this.http.get<PurchaseOrderItem[]>(`${this.apiUrl}/PurchaseBills/oldest-items`, {
      withCredentials: true,
    });
  }

  getItemQuantities(): Observable<ItemQuantity[]> {
    return this.http.get<ItemQuantity[]>(`${this.apiUrl}/PurchaseBills/item-quantities`, {
      withCredentials: true,
    });
  }

  checkSession(): Observable<{ authenticated: boolean }> {
    return this.http.get<{ authenticated: boolean }>(`${this.apiUrl}/Auth/session`, {
      withCredentials: true,
    });
  }

  logout(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.apiUrl}/Auth/logout`,
      {},
      { withCredentials: true },
    );
  }
}
