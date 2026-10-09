Issue 1 - Modelo de datos y base de datos

Tablas
- Marca (Id, Nombre)
- Repuesto (Id, Codigo, Nombre, Precio, Cantidad, MarcaId)
- Cliente (Id, Nombre, Documento, Telefono, Direccion)
- Venta (Id, Fecha, ClienteId, Total)
- DetalleVenta (Id, VentaId, RepuestoId, Cantidad, Precio)

Relaciones
- Una Marca tiene muchos Repuestos.
- Un Cliente tiene muchas Ventas.
- Una Venta tiene muchos DetalleVenta.
- Un Repuesto aparece en muchos DetalleVenta.
