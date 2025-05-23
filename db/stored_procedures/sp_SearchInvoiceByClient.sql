--sp_SearchInvoiceByClient

CREATE PROCEDURE sp_SearchInvoiceByClient
    @IdCliente INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        f.Id, f.NumeroFactura, f.FechaEmisionFactura, 
        f.NumeroTotalArticulos, f.SubTotalFacturas, 
        f.TotalImpuestos, f.TotalFactura,
        c.RazonSocial, c.RFC
    FROM TblFacturas f
    INNER JOIN TblClientes c ON c.Id = f.IdCliente
    WHERE f.IdCliente = @IdCliente
    ORDER BY f.FechaEmisionFactura DESC;
END
GO
