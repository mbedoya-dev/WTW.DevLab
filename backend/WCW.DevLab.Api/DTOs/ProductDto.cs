namespace WCW.DevLab.Api.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string ImagenProducto { get; set; }
        public string Ext { get; set; }
    }
}
