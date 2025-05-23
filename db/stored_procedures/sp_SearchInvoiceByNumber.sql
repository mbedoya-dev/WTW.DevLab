--sp_SearchInvoiceByNumber

CREATE PROCEDURE sp_SearchInvoiceByNumber
    @NumeroFactura INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Encabezado
    SELECT 
        f.Id, f.NumeroFactura, f.FechaEmisionFactura, 
        f.NumeroTotalArticulos, f.SubTotalFacturas, 
        f.TotalImpuestos, f.TotalFactura,
        c.RazonSocial, c.RFC
    FROM TblFacturas f
    INNER JOIN TblClientes c ON c.Id = f.IdCliente
    WHERE f.NumeroFactura = @NumeroFactura;

    -- Detalle
    SELECT 
        d.IdProducto, p.NombreProducto, 
        d.CantidadDeProducto, d.PrecioUnitarioProducto, 
        d.SubtotalProducto, d.Notas,
        p.ImagenProducto, p.ext
    FROM TblDetallesFactura d
    INNER JOIN TblFacturas f ON f.Id = d.IdFactura
    INNER JOIN CatProductos p ON p.Id = d.IdProducto
    WHERE f.NumeroFactura = @NumeroFactura;
END
GO
