import { Routes } from '@angular/router';
import { CreateInvoiceComponent } from './invoices/create-invoice/create-invoice.component';

export const routes: Routes = [
  { path: 'nueva-factura', component: CreateInvoiceComponent },
  { path: '', redirectTo: 'nueva-factura', pathMatch: 'full' }
];
