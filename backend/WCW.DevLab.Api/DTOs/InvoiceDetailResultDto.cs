namespace WCW.DevLab.Api.DTOs
{
    public class InvoiceDetailResultDto
    {
        public InvoiceHeaderDto Encabezado { get; set; }
        public List<InvoiceItemDto> Detalles { get; set; }
    }

    public class InvoiceHeaderDto
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

    public class InvoiceItemDto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int CantidadDeProducto { get; set; }
        public decimal PrecioUnitarioProducto { get; set; }
        public decimal SubtotalProducto { get; set; }
        public string Notas { get; set; }
        public string ImagenProducto { get; set; }
        public string Ext { get; set; }
    }

}
