--sp_GetAllClients

CREATE PROCEDURE sp_GetAllClients
AS
BEGIN
    SELECT Id, RazonSocial, RFC, FechaCreacion
    FROM TblClientes
    ORDER BY RazonSocial;
END
GO
