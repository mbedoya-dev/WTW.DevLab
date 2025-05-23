namespace WCW.DevLab.Api.DTOs
{
    public class InvoiceSummaryDto
    {
        public int Id { get; set; }
        public int NumeroFactura { get; set; }
        public DateTime FechaEmisionFactura { get; set; }
        public int NumeroTotalArticulos { get; set; }
        public decimal SubTotalFacturas { get; set; }
        public decimal TotalImpuestos { get; set; }
        public decimal TotalFactura { get; set; }
        public string RazonSocial { get; set; }
        public string RFC { get; set; }
    }
}
