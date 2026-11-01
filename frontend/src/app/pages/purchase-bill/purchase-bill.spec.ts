import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseBill } from './purchase-bill';

describe('PurchaseBill', () => {
  let component: PurchaseBill;
  let fixture: ComponentFixture<PurchaseBill>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PurchaseBill],
    }).compileComponents();

    fixture = TestBed.createComponent(PurchaseBill);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
