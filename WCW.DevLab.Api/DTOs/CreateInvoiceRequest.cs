namespace WCW.DevLab.Api.DTOs
{
    public class CreateInvoiceRequest
    {
        public required DateTime FechaEmisionFactura { get; set; }
        public required int IdCliente { get; set; }
        public required int NumeroFactura { get; set; }
        public required int NumeroTotalArticulos { get; set; }
        public required decimal SubTotalFacturas { get; set; }
        public required decimal TotalImpuestos { get; set; }
        public required decimal TotalFactura { get; set; }
        public required List<InvoiceDetailDto> Detalles { get; set; }
    }
}
