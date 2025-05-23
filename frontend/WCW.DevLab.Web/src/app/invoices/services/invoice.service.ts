import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateInvoiceRequest } from '../models/invoice.model';

@Injectable({ providedIn: 'root' })
export class InvoiceService {
  private http = inject(HttpClient);
  private baseUrl = 'https://localhost:7165/api/Invoice';

  createInvoice(payload: CreateInvoiceRequest): Observable<void> {
    return this.http.post<void>(this.baseUrl, payload);
  }

  getInvoicesByClient(id: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/client/${id}`);
  }

  getInvoiceByNumber(numero: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/number/${numero}`);
  }
}
