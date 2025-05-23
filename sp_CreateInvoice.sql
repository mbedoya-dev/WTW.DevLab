--sp_CreateInvoice

CREATE TYPE DetalleFacturaType AS TABLE
(
    IdProducto INT,
    CantidadDeProducto INT,
    PrecioUnitarioProducto DECIMAL(18, 2),
    SubtotalProducto DECIMAL(18, 2),
    Notas VARCHAR(200)
);
GO

CREATE PROCEDURE sp_CreateInvoice
    @FechaEmisionFactura DATETIME,
    @IdCliente INT,
    @NumeroFactura INT,
    @NumeroTotalArticulos INT,
    @SubTotalFacturas DECIMAL(18,2),
    @TotalImpuestos DECIMAL(18,2),
    @TotalFactura DECIMAL(18,2),
    @Detalles DetalleFacturaType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRAN;

        INSERT INTO TblFacturas (
            FechaEmisionFactura, IdCliente, NumeroFactura, NumeroTotalArticulos,
            SubTotalFacturas, TotalImpuestos, TotalFactura
        )
        VALUES (
            @FechaEmisionFactura, @IdCliente, @NumeroFactura, @NumeroTotalArticulos,
            @SubTotalFacturas, @TotalImpuestos, @TotalFactura
        );

        DECLARE @FacturaId INT = SCOPE_IDENTITY();

        INSERT INTO TblDetallesFactura (
            IdFactura, IdProducto, CantidadDeProducto, PrecioUnitarioProducto,
            SubtotalProducto, Notas
        )
        SELECT
            @FacturaId, IdProducto, CantidadDeProducto, PrecioUnitarioProducto,
            SubtotalProducto, Notas
        FROM @Detalles;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END
GO
