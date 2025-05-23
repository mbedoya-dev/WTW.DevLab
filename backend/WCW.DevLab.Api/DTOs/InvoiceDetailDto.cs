namespace WCW.DevLab.Api.DTOs
{
    public class InvoiceDetailDto
    {
        public required int IdProducto { get; set; }
        public required int CantidadDeProducto { get; set; }
        public required decimal PrecioUnitarioProducto { get; set; }
        public required decimal SubtotalProducto { get; set; }
        public required string Notas { get; set; }
    }
}
