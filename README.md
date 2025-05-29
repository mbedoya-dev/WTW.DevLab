# Proyecto de Facturación WTW.DevLab

Este proyecto es una aplicación web de facturación desarrollada como parte de una prueba técnica. El objetivo es demostrar la implementación de una arquitectura estándar de aplicación web utilizando el stack de tecnologías de Microsoft. La aplicación permite la creación y consulta de facturas, interactuando con un backend en ASP.NET Core y una base de datos SQL Server.

## 📁 Estructura del Proyecto

El proyecto se organiza en tres componentes principales: backend, base de datos y frontend.

```
WTW.DevLab/
├── backend/                  # API REST en ASP.NET Core
│   ├── Controllers/          # Endpoints para Facturas, Productos y Clientes
│   ├── DTOs/                 # Modelos usados para transporte de datos
│   ├── Program.cs            # Configuración principal de la API
│   ├── appsettings.json      # Cadena de conexión y configuración
│
├── db/                       # Scripts de base de datos
│   ├── stored_procedures/    # Procedimientos almacenados SQL Server
│   ├── schema.sql            # Script para crear tablas base
│
└── frontend/                 # Aplicación Angular 19 (en desarrollo)
```

# Proyecto de Facturación WTW.DevLab

Este proyecto es una aplicación web de facturación desarrollada como parte de una prueba técnica. [cite: 1] El objetivo es demostrar la implementación de una arquitectura estándar de aplicación web utilizando el stack de tecnologías de Microsoft. [cite: 1] La aplicación permite la creación y consulta de facturas, interactuando con un backend en ASP.NET Core y una base de datos SQL Server.

## ✨ Características Implementadas y Tecnologías

### Backend (ASP.NET Core)

* **API RESTful:** Se han desarrollado endpoints para la gestión de:
    * Clientes (`ClientController.cs`)
    * Productos (`ProductController.cs`)
    * Facturas (`InvoiceController.cs`)
* **Acceso a Datos:**
    * Se utiliza **ADO.NET** directo (`SqlConnection`, `SqlCommand`) para la interacción con la base de datos.
    * Esta elección cumple con la restricción explícita de la prueba técnica de **no utilizar Entity Framework**. [cite: 9]
* **Lógica de Base de Datos Centralizada:**
    * Todas las operaciones de base de datos (consultas, inserciones, etc.) están encapsuladas en **procedimientos almacenados** en SQL Server[cite: 9], como:
        * `sp_GetAllClients` (`mbedoya-dev/wtw.devlab/WTW.DevLab-4116ce8d9aa845ec66f47a73d7688f62e854d76c/db/stored_procedures/sp_GetAllClients.sql`)
        * `sp_GetAllProducts` (`mbedoya-dev/wtw.devlab/WTW.DevLab-4116ce8d9aa845ec66f47a73d7688f62e854d76c/db/stored_procedures/sp_GetAllProducts.sql`)
        * `sp_GetProductById` (`mbedoya-dev/wtw.devlab/WTW.DevLab-4116ce8d9aa845ec66f47a73d7688f62e854d76c/db/stored_procedures/sp_GetProductById.sql`)
        * `sp_CreateInvoice` (`mbedoya-dev/wtw.devlab/WTW.DevLab-4116ce8d9aa845ec66f47a73d7688f62e854d76c/db/stored_procedures/sp_CreateInvoice.sql`)
        * `sp_SearchInvoiceByClient` (`mbedoya-dev/wtw.devlab/WTW.DevLab-4116ce8d9aa845ec66f47a73d7688f62e854d76c/db/stored_procedures/sp_SearchInvoiceByClient.sql`)
        * `sp_SearchInvoiceByNumber` (`mbedoya-dev/wtw.devlab/WTW.DevLab-4116ce8d9aa845ec66f47a73d7688f62e854d76c/db/stored_procedures/sp_SearchInvoiceByNumber.sql`)
    * El procedimiento `sp_CreateInvoice` utiliza **Table Valued Parameters (TVP)** (`DetalleFacturaType`) para la inserción eficiente de múltiples detalles de factura y maneja **transacciones** para asegurar la atomicidad de la operación.
* **DTOs (Data Transfer Objects):** Se emplean para una comunicación clara y estructurada entre el frontend y el backend (ej. `CreateInvoiceRequest.cs`, `ProductDto.cs`, `ClientDto.cs`).
* **Documentación API:** Se ha integrado Swagger para la visualización y prueba de los endpoints.
* **Configuración:**
    * La cadena de conexión a la base de datos se gestiona a través de `appsettings.json`.
    * CORS está configurado en `Program.cs` para permitir solicitudes desde `http://localhost:4200`, facilitando el desarrollo local con Angular.

### Base de Datos (SQL Server)

* **Motor:** SQL Server. [cite: 2]
* **Nombre de la Base de Datos:** `LabDev` (según `appsettings.json` y `Proyecto de Testing.docx` [cite: 2]).
* **Esquema:** Definido en `db/schema.sql`, incluye tablas como `CatProductos`, `TblClientes`, `TblFacturas`, `TblDetallesFactura`.
* **Credenciales de Desarrollo:** Usuario: `developer`, Contraseña: `abc123ABC`. [cite: 2]

### Frontend (Angular 19)

* **Framework:** Angular versión 19 (`mbedoya-dev/wtw.devlab/WTW.DevLab-4116ce8d9aa845ec66f47a73d7688f62e854d76c/frontend/WCW.DevLab.Web/package.json`).
* **Funcionalidad Principal (Creación de Facturas):**
    * Se ha implementado el componente `CreateInvoiceComponent` (`mbedoya-dev/wtw.devlab/WTW.DevLab-4116ce8d9aa845ec66f47a73d7688f62e854d76c/frontend/WCW.DevLab.Web/src/app/invoices/create-invoice/create-invoice.component.ts`) siguiendo el diseño guía y las reglas de negocio especificadas en el documento de la prueba técnica. [cite: 5]
    * Utiliza **Angular Material** para los componentes de la interfaz de usuario (`mbedoya-dev/wtw.devlab/WTW.DevLab-4116ce8d9aa845ec66f47a73d7688f62e854d76c/frontend/WCW.DevLab.Web/angular.json`, `mbedoya-dev/wtw.devlab/WTW.DevLab-4116ce8d9aa845ec66f47a73d7688f62e854d76c/frontend/WCW.DevLab.Web/src/app/invoices/create-invoice/create-invoice.component.ts`).
    * Emplea **Formularios Reactivos de Angular** (`FormGroup`, `FormArray`) para la gestión de datos de la factura y sus detalles.
    * Los servicios (`ClientService.ts`, `ProductService.ts`[cite: 7], `InvoiceService.ts` [cite: 9]) se encargan de la comunicación HTTP con la API backend.
    * Se definen modelos de datos TypeScript para una correcta tipificación (`product.model.ts`, `invoice.model.ts`, `client.model.ts`).
    * **Cumplimiento de Reglas de Negocio (Creación):** [cite: 5, 6]
        * El botón "Nuevo" limpia el formulario.
        * Los combos de cliente y producto se cargan dinámicamente desde la API.
        * Al seleccionar un producto, se actualizan automáticamente el precio unitario y la imagen.
        * Al cambiar la cantidad, se recalcula el subtotal del producto.
        * El subtotal general, impuestos (19%) y total de la factura se calculan en el cliente.
        * El botón "Guardar" envía los datos al backend.
* **Funcionalidad Pendiente (Búsqueda de Facturas):** La pantalla de búsqueda de facturas, detallada en el documento de la prueba técnica, no fue implementada. [cite: 7]

## 🚀 Configuración y Ejecución

### Prerrequisitos

* .NET SDK (versión compatible con ASP.NET Core usado en el backend)
* SQL Server
* Node.js y npm (para el frontend Angular)
* Angular CLI (globalmente o vía npx)

### Base de Datos

1.  Asegúrese de tener una instancia de SQL Server accesible.
2.  Cree una base de datos llamada `LabDev`.
3.  Ejecute el script `db/schema.sql` para crear las tablas.
4.  Ejecute todos los scripts de la carpeta `db/stored_procedures/` para crear los procedimientos almacenados.
5.  Verifique que la cadena de conexión en `backend/WCW.DevLab.Api/appsettings.json` coincida con su configuración de SQL Server y las credenciales proporcionadas (`User Id=developer;Password=abc123ABC;`). [cite: 2]

### Backend

1.  Navegue al directorio `backend/WCW.DevLab.Api/`.
2.  Restaure las dependencias: `dotnet restore`
3.  Ejecute la aplicación: `dotnet run`
    * Por defecto, la API estará disponible en `https://localhost:7165` y `http://localhost:5196`.
    * Swagger UI estará disponible en `/swagger`.

### Frontend

1.  Navegue al directorio `frontend/WCW.DevLab.Web/`.
2.  Instale las dependencias: `npm install`
3.  Inicie el servidor de desarrollo: `ng serve` (`mbedoya-dev/wtw.devlab/WTW.DevLab-4116ce8d9aa845ec66f47a73d7688f62e854d76c/frontend/WCW.DevLab.Web/package.json`)
    * La aplicación estará disponible en `http://localhost:4200/`.
    * La aplicación se recargará automáticamente si cambia alguno de los archivos 

## 🛠️ Herramientas Utilizadas

* Visual Studio Code 
* SQL Server Management Studio 
* Angular CLI
