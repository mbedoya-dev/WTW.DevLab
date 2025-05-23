--sp_GetProductById

CREATE PROCEDURE sp_GetProductById
    @IdProducto INT
AS
BEGIN
    SELECT 
        Id, NombreProducto, PrecioUnitario, ImagenProducto, ext
    FROM CatProductos
    WHERE Id = @IdProducto;
END
GO
