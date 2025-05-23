export interface InvoiceDetailDto {
  idProducto: number;
  cantidadDeProducto: number;
  precioUnitarioProducto: number;
  subtotalProducto: number;
  notas: string;
}

export interface CreateInvoiceRequest {
  fechaEmisionFactura: Date;
  idCliente: number;
  numeroFactura: number;
  numeroTotalArticulos: number;
  subTotalFacturas: number;
  totalImpuestos: number;
  totalFactura: number;
  detalles: InvoiceDetailDto[];
}
