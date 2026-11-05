# Stockmaster-API

## Decisiones de diseño

### Borrado lógico de productos

- Los productos no se eliminan: se desactivan (`Activo = false`) para conservar el historial de alertas y pedidos.
- Un filtro global (`HasQueryFilter`) oculta los productos inactivos en todas las consultas. Las alertas de productos inactivos también se ocultan.
- Desactivar un producto que no existe o que ya está inactivo responde **404 Not Found**: para la aplicación, un producto inactivo no existe.
- El nombre de un producto inactivo se puede reutilizar al crear uno nuevo.
- Un índice único parcial (`WHERE "Activo" = true`) garantiza en la base que no haya dos productos activos con el mismo nombre, incluso con peticiones simultáneas.
