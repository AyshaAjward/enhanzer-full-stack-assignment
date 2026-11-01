import { TestBed } from '@angular/core/testing';

import { PurchaseBill } from './purchase-bill';

describe('PurchaseBill', () => {
  let service: PurchaseBill;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PurchaseBill);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
