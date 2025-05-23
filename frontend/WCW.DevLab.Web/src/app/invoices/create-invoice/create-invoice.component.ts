import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';

import { ClientService } from '../services/client.service';
import { ProductService } from '../services/product.service';
import { InvoiceService } from '../services/invoice.service';

import { Client} from '../models/client.model';
import { Product } from '../models/product.model';
import { CreateInvoiceRequest, InvoiceDetailDto  } from '../models/invoice.model';
import { MatDivider } from '@angular/material/divider';

@Component({
  selector: 'app-create-invoice',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatCardModule,
    MatTableModule,
    MatIconModule,
    MatFormFieldModule,
    MatDivider
  ],
  templateUrl: './create-invoice.component.html',
  styleUrl: './create-invoice.component.css'
})
export class CreateInvoiceComponent implements OnInit {
  private fb = inject(FormBuilder);
  private clientService = inject(ClientService);
  private productService = inject(ProductService);
  private invoiceService = inject(InvoiceService);

  form!: FormGroup;
  clients: Client[] = [];
  products: Product[] = [];
  columns: string[] = ['producto', 'precio', 'cantidad', 'imagen', 'subtotal'];

  ngOnInit(): void {
    this.initForm();
    this.loadClients();
    this.loadProducts();
  }

  initForm(): void {
    this.form = this.fb.group({
      idCliente: [null, Validators.required],
      numeroFactura: [null, [Validators.required, Validators.min(1)]],
      fechaEmisionFactura: [new Date()],
      detalles: this.fb.array([]),
      subTotalFacturas: [{ value: 0, disabled: true }],
      totalImpuestos: [{ value: 0, disabled: true }],
      totalFactura: [{ value: 0, disabled: true }]
    });
  }

  get detalles(): FormArray {
    return this.form.get('detalles') as FormArray;
  }

  addDetalle(): void {
    const detalle = this.fb.group({
      idProducto: [null, Validators.required],
      cantidadDeProducto: [1, [Validators.required, Validators.min(1)]],
      precioUnitarioProducto: [{ value: 0, disabled: true }],
      subtotalProducto: [{ value: 0, disabled: true }],
      notas: ['']
    });

    detalle.get('idProducto')!.valueChanges.subscribe(productId => {
      const producto = this.products.find(p => p.id === + (productId? productId : 0));
      if (producto) {
        const cantidad = detalle.get('cantidadDeProducto')?.value || 1;
        detalle.patchValue({
          precioUnitarioProducto: producto.precioUnitario,
          subtotalProducto: producto.precioUnitario * cantidad
        });
        this.recalculateTotals();
      }
    });

    detalle.get('cantidadDeProducto')!.valueChanges.subscribe(cantidad => {
      const precio = detalle.get('precioUnitarioProducto')?.value || 0;
      detalle.patchValue({
        subtotalProducto: cantidad? cantidad : 0 * precio
      }, { emitEvent: false });
      this.recalculateTotals();
    });

    this.detalles.push(detalle);
  }

  recalculateTotals(): void {
    const subtotal = this.detalles.controls.reduce((acc, curr) =>
      acc + (curr.get('subtotalProducto')?.value || 0), 0
    );
    const impuestos = subtotal * 0.19;
    const total = subtotal + impuestos;

    this.form.patchValue({
      subTotalFacturas: subtotal,
      totalImpuestos: impuestos,
      totalFactura: total
    }, { emitEvent: false });
  }

  loadClients(): void {
    this.clientService.getClients().subscribe(data => this.clients = data);
  }

  loadProducts(): void {
    this.productService.getProducts().subscribe(data => this.products = data);
  }

  getImage(productId: number): string {
    return this.products.find(p => p.id === productId)?.imagenProducto || '';
  }

  nuevo(): void {
    this.form.reset();
    this.detalles.clear();
    this.form.patchValue({ fechaEmisionFactura: new Date() });
  }

  guardar(): void {
    if (this.form.valid) {
      const raw = this.form.getRawValue();
      const payload: CreateInvoiceRequest = {
        ...raw,
        numeroTotalArticulos: raw.detalles.reduce((sum: number, d: any) => sum + d.cantidadDeProducto, 0),
        detalles: raw.detalles as InvoiceDetailDto[]
      };
      this.invoiceService.createInvoice(payload).subscribe(() => {
        alert('Factura guardada correctamente');
        this.nuevo();
      });
    }
  }
}