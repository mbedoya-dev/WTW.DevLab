--sp_GetAllProducts

CREATE PROCEDURE sp_GetAllProducts
AS
BEGIN
    SELECT 
        Id, NombreProducto, PrecioUnitario, ImagenProducto, ext
    FROM CatProductos
    ORDER BY NombreProducto;
END
GO
