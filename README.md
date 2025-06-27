# DAEAPF
📘 DOCUMENTACIÓN DEL BACKEND (DAEAPF)
✅ Objetivo
Organizar y exponer una API REST para:

Manejo de usuarios (registro, login, actualización, cambio de contraseña)

Manejo de negocios (CRUD)

Manejo de productos por negocio (CRUD)

🗂️ Estructura de Capas
✅ Domain
➡️ Modelos de base de datos (Entidades): Usuario, Negocio, Producto, Estados, etc.
➡️ Representa las tablas.

✅ Infrastructure
➡️ DbContext (Entity Framework).
➡️ Configuración de la conexión a la base de datos.
➡️ Hashing de contraseñas.

✅ Application
➡️ DTOs (Data Transfer Objects) → Evitan referencias circulares y controlan datos expuestos.
➡️ Servicios (lógica de negocio).
➡️ Interfaces para inyección de dependencias.

✅ Controllers (API)
➡️ Exponen los endpoints.
➡️ Son delgados → solo validan, autorizan y llaman a servicios.

✅ Diseño de Seguridad
Autenticación con JWT.

Autorización con roles:

admin

negocio

cliente

Usuarios autenticados reciben un JWT al iniciar sesión.

Validación de claims (ID, Role) en cada request protegido.

✅ Flujo General
1️⃣ Controller recibe el request.
2️⃣ Valida modelo y autorización.
3️⃣ Extrae usuarioID y Role del JWT.
4️⃣ Llama al Servicio correspondiente con DTOs.
5️⃣ Servicio consulta el DbContext, aplica reglas de negocio.
6️⃣ Servicio devuelve DTO de respuesta → Controller → Frontend.

✅ DTOs (Data Transfer Objects)
🧑‍💻 Usuarios
✅ RegisterUserDto

Para crear cuenta (correo, contraseña, rol, etc).

✅ LoginUserDto

Para iniciar sesión.

✅ UpdateUserDto

Actualizar datos básicos (nombre, correo, rol).

✅ ChangePasswordDto

Cambiar contraseña actual de forma segura.

✅ UserResponseDto

Datos limpios que se exponen al frontend.

🏪 Negocios
✅ CreateNegocioDto

Crear negocio (nombre, descripción, dirección, categoría).

✅ UpdateNegocioDto

Editar negocio.

✅ NegocioResponseDto

Resultado con info completa, sin referencias cíclicas.

✅ NegocioFilterDto

Filtros para búsqueda (nombre, categoría, estado).

Incluye paginación opcional.

Búsqueda flexible (case-insensitive).

🧃 Productos
✅ CreateProductDto

Para añadir producto (nombre, descripción, precio, etc).

✅ UpdateProductDto

Para editar producto existente.

✅ ProductResponseDto

Formato de respuesta al frontend.

✅ Servicios
Interfaz + Implementación

Separa la lógica de negocio del Controller.

Facilita testing y mantenimiento.

Valida permisos del usuario (dueño o admin).

Hace el trabajo con EF Core.

✅ AuthService

Registro y login.

Generación de JWT.

✅ UsuarioService

Obtener, actualizar, borrar usuario.

Cambiar contraseña.

✅ NegocioService

CRUD completo con filtros flexibles.

Validación de dueño del negocio.

✅ ProductoService

CRUD vinculado al negocio.

Validación de dueño del negocio.

✅ Controllers
✅ AuthController

/api/auth/register

/api/auth/login

✅ UsuarioController

/api/usuario

GET all (admin)

GET by id

PUT by id (actualizar datos)

PUT /{id}/password (cambiar contraseña)

DELETE (admin)

✅ NegocioController

/api/negocio

GET con filtros (paginación, búsqueda flexible)

GET by id

POST (dueño o admin)

PUT (dueño o admin)

DELETE (dueño o admin)

✅ ProductoController

/api/producto/negocio/{negocioId} (listar productos de un negocio)

POST /api/producto (dueño o admin)

PUT /api/producto/{id} (dueño o admin)

DELETE /api/producto/{id} (dueño o admin)

✅ Características Técnicas Importantes
EF Core para acceso a datos.

Inyección de dependencias configurada en Program.cs/Startup.cs.

DTOs para seguridad y performance:

Evitan referencias infinitas (ciclos en serialización).

Controlan datos expuestos al frontend.

Filtros avanzados en negocio:

Búsqueda flexible en nombre y dirección.

Filtrado por categoría y estado.

Paginación.

Validación de permisos en servicios:

Admin puede todo.

Dueño solo su negocio/producto.

✅ Ventajas de este diseño
⭐ Separación de responsabilidades.
⭐ Escalable y mantenible.
⭐ Control de seguridad y permisos claro.
⭐ Estructura limpia para el equipo.
⭐ Evita errores de referencias circulares.

✅ Para contribuir o seguir desarrollando
➡️ Respetar la separación de capas.
➡️ Crear DTOs nuevos si se agregan endpoints.
➡️ Agregar validación de ModelState en Controllers.
➡️ Delegar validaciones complejas en los servicios.
➡️ Mantener la inyección de dependencias actualizada.

Si quieres, puedo preparar también:
✅ Un README.md más corto para Git.
✅ Diagrama de arquitectura simplificado.

¡Avísame si quieres eso! 🚀