# Resumen descriptivo de proyectos — Solución Pagapoco

**Pagapoco** es una plataforma de marketplace / clasificados orientada a la compraventa de repuestos y accesorios para vehículos. La solución sigue una arquitectura en capas compuesta por 6 proyectos .NET más una carpeta de recursos de base de datos.

---

## Arquitectura general

```
Pagapoco.Web.MVC          ← Frontend (consume la API vía HTTP)
        │
Pagapoco.API              ← Backend REST (punto de entrada de la API)
        │
Pagapoco.Service          ← Lógica de negocio (servicios e interfaces)
        │
Pagapoco.Infraestructure  ← Acceso a datos (EF Core + SQL Server)
        │
Pagapoco.Core.Entidades   ← Dominio (entidades puras, sin dependencias)

Pagapoco.Database         ← Scripts SQL para crear la base de datos
pagapoco                  ← Proyecto inicial/plantilla (no se usa en producción)
```

---

## 1. `Pagapoco.Core.Entidades` — Capa de Dominio

| Propiedad | Detalle |
|-----------|---------|
| **Tipo** | Biblioteca de clases (.NET 9) |
| **Namespace** | `Pagapoco.Core.Entities` |
| **Dependencias externas** | `System.Linq.Dynamic.Core` |
| **Dependencias internas** | Ninguna |

### Descripción
Es el núcleo del modelo de datos. Define las **entidades del dominio** sin dependencias de infraestructura, siguiendo el principio de inversión de dependencias.

### Entidades
| Clase | Tabla SQL | Descripción |
|-------|-----------|-------------|
| `User` | `Users` | Usuario de la plataforma. Almacena email, nombre, teléfono, ciudad, hash y salt de contraseña, y el flag de borrado lógico `IsDeleted`. |
| `Publication` | `Publications` | Anuncio/publicación de un repuesto. Incluye título, descripción, precio, ciudad, tipo, datos específicos del repuesto (marca, modelo, color, condición, compatibilidad) y el estado `IsPaused`. |
| `Image` | `Images` | Imagen asociada a una publicación. Almacena URL, texto alternativo y orden de visualización. |

### Relaciones
- Un `User` tiene muchas `Publications` (1:N, cascade delete).
- Una `Publication` tiene muchas `Images` (1:N, cascade delete).

---

## 2. `Pagapoco.Infraestructure` — Capa de Infraestructura / Acceso a Datos

| Propiedad | Detalle |
|-----------|---------|
| **Tipo** | Biblioteca de clases (.NET 9) |
| **Namespace** | `Pagapoco.Infrastructure.Data` |
| **Dependencias externas** | `Microsoft.EntityFrameworkCore.SqlServer 9.0.6`, `Microsoft.EntityFrameworkCore.Tools 9.0.6`, `Microsoft.EntityFrameworkCore.Design 9.0.6` |
| **Dependencias internas** | `Pagapoco.Core.Entidades` |

### Descripción
Implementa el acceso a datos mediante **Entity Framework Core** con SQL Server. Configura el contexto de la base de datos y las reglas del modelo relacional.

### Clases principales
| Clase | Descripción |
|-------|-------------|
| `AppDbContext` | DbContext de EF Core. Expone los `DbSet<T>` para `Users`, `Publications` e `Images`. Configura las relaciones, restricciones (email único) y valores por defecto (`IsDeleted = false`, `IsPaused = false`) mediante Fluent API en `OnModelCreating`. |

---

## 3. `Pagapoco.Service` — Capa de Aplicación / Lógica de Negocio

| Propiedad | Detalle |
|-----------|---------|
| **Tipo** | Biblioteca de clases (.NET 9) |
| **Namespace** | `Pagapoco.Application.Services` / `Pagapoco.Services.Interfaces` |
| **Dependencias externas** | `System.Linq.Dynamic.Core` |
| **Dependencias internas** | `Pagapoco.Core.Entidades`, `Pagapoco.Infraestructure` |

### Descripción
Contiene toda la **lógica de negocio** de la aplicación. Define las interfaces de los servicios y sus implementaciones concretas.

### Interfaces y servicios
| Interfaz | Implementación | Responsabilidad |
|----------|---------------|-----------------|
| `IUserService` | `UserService` | Registro de usuarios (con PBKDF2-SHA256, 100 000 iteraciones), login con verificación de contraseña, actualización de perfil, borrado lógico/físico y consulta de publicaciones del usuario. |
| `IPublicationService` | `PublicationService` | CRUD completo de publicaciones, paginación, búsqueda por ciudad/tipo, filtrado por atributos de repuesto (marca, modelo, color, condición, compatibilidad), y gestión del estado pausado/activo. |
| `IImageService` | `ImageService` | Gestión de imágenes asociadas a publicaciones: agregar, eliminar, actualizar y listar imágenes. *(Varios métodos de la interfaz actualizada están pendientes de implementación — `NotImplementedException`.)* |

---

## 4. `Pagapoco.API` — Capa de Presentación (API REST)

| Propiedad | Detalle |
|-----------|---------|
| **Tipo** | ASP.NET Core Web API (.NET 9) |
| **Namespace** | `Pagapoco.API.Controllers` |
| **Dependencias externas** | `Microsoft.AspNetCore.Authentication.JwtBearer 9.0.6`, `Swashbuckle.AspNetCore 9.0.1`, `Microsoft.AspNetCore.OpenApi 9.0.6`, `System.Linq.Dynamic.Core` |
| **Dependencias internas** | `Pagapoco.Core.Entidades`, `Pagapoco.Infraestructure`, `Pagapoco.Service` |

### Descripción
Es el **punto de entrada principal** de la aplicación backend. Expone una API RESTful con autenticación JWT, documentación Swagger interactiva y CORS habilitado para cualquier origen.

### Controladores y endpoints

#### `UserController` — `/api/user`
| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| POST | `/register` | Público | Registra un nuevo usuario |
| POST | `/login` | Público | Autentica al usuario y devuelve un token JWT + userId |
| PUT | `/{userId}` | JWT | Actualiza nombre, teléfono y ciudad del usuario |
| DELETE | `/{userId}` | JWT | Elimina (lógico o físico) al usuario autenticado |
| GET | `/{userId}/publications` | JWT | Devuelve las publicaciones del usuario |
| GET | `/{userId}` | JWT | Obtiene los datos del perfil del usuario |

#### `PublicationsController` — `/api/publications`
| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| GET | `/paged` | Público | Lista publicaciones con paginación |
| GET | `/` | Público | Devuelve todas las publicaciones activas |
| GET | `/search` | Público | Busca por ciudad y/o tipo |
| GET | `/{id}` | Público | Detalle de una publicación (con imágenes opcionales) |
| GET | `/filter` | Público | Filtra por atributos del repuesto |
| GET | `/user/{userId}` | JWT | Publicaciones de un usuario específico |
| POST | `/` | JWT | Crea una nueva publicación |
| PUT | `/{publicationId}` | JWT | Actualiza título, descripción y precio |
| DELETE | `/{publicationId}` | JWT | Elimina (lógico o físico) una publicación |
| POST | `/{publicationId}/pause` | JWT | Pausa una publicación |
| POST | `/{publicationId}/activate` | JWT | Activa una publicación pausada |

#### `ImagesController` — `/api/images`
| Método | Ruta | Auth | Descripción |
|--------|------|------|-------------|
| GET | `/publication/{publicationId}` | Público | Obtiene las imágenes de una publicación |
| POST | `/publication/{publicationId}/add` | JWT | Agrega imágenes a una publicación |
| DELETE | `/{imageId}` | JWT | Elimina una imagen |
| PUT | `/{imageId}` | JWT | Actualiza URL, alt text u orden de una imagen |

### DTOs
Carpeta `Dtos/` con objetos de transferencia para: `UserRegisterDto`, `UserLoginDto`, `UserUpdateDto`, `UserDto`, `PublicationCreateDto`, `PublicationUpdateDto`, `PublicationReadDto`, `ImageCreateDto`, `ImageUpdateDto`, `ImageReadDto`.

---

## 5. `Pagapoco.Web.MVC` — Frontend Web (ASP.NET Core MVC)

| Propiedad | Detalle |
|-----------|---------|
| **Tipo** | ASP.NET Core MVC (.NET 9) |
| **Namespace** | `Pagapoco.Web.MVC.Controllers` / `Pagapoco.Web.MVC.Models` |
| **Dependencias externas** | `System.Linq.Dynamic.Core` |
| **Dependencias internas** | `Pagapoco.API` *(referencia de proyecto, aunque consume la API vía HTTP)* |

### Descripción
Interfaz web visual para el usuario final. Es una aplicación **ASP.NET Core MVC** que consume la API REST de `Pagapoco.API` mediante `HttpClient`. Utiliza sesiones y cookies HTTP-only para gestionar la autenticación con JWT.

### Controladores
| Controlador | Vistas | Responsabilidad |
|-------------|--------|-----------------|
| `HomeController` | `Index`, `Privacy`, `Error` | Página principal y gestión de errores |
| `UserController` | `Login`, `Register`, `Edit` | Registro, inicio y cierre de sesión, edición y eliminación del perfil de usuario |
| `PublicationController` | `Index`, `Details`, `MyPublications`, `Filter` | Listado, detalle, filtrado de publicaciones y vista de publicaciones del usuario autenticado |

### Modelos de vista (ViewModels)
`UserLoginViewModel`, `UserRegisterViewModel`, `UserViewModel`, `UserEditViewModel`, `PublicationViewModel`, `ImageViewModel`, `UserPublicationViewModel`, `ErrorViewModel`.

---

## 6. `Pagapoco.Database` — Scripts de Base de Datos

| Propiedad | Detalle |
|-----------|---------|
| **Tipo** | Recurso SQL (sin proyecto .NET) |
| **Archivo** | `creationSQLServer` |

### Descripción
Contiene el **script SQL de creación** de la base de datos `PagapocoDB` en SQL Server. Define las tres tablas principales (`Users`, `Publications`, `Images`) con sus columnas, restricciones y claves foráneas en cascada. Es la referencia canónica del esquema de base de datos.

---

## 7. `pagapoco` — Proyecto Inicial / Plantilla

| Propiedad | Detalle |
|-----------|---------|
| **Tipo** | ASP.NET Core Web API (.NET 8) |
| **Estado** | Plantilla generada automáticamente, no integrada en la solución principal |

### Descripción
Es el proyecto **scaffolded** generado originalmente por la plantilla de .NET al iniciar el desarrollo. Contiene el controlador de ejemplo `WeatherForecastController` y no está conectado al resto de la solución. Sirve como referencia histórica del punto de partida del proyecto.

---

## Resumen de dependencias entre proyectos

```
pagapoco              (standalone, plantilla inicial)

Pagapoco.Core.Entidades
    └── (sin dependencias internas)

Pagapoco.Infraestructure
    └── Pagapoco.Core.Entidades

Pagapoco.Service
    ├── Pagapoco.Core.Entidades
    └── Pagapoco.Infraestructure

Pagapoco.API
    ├── Pagapoco.Core.Entidades
    ├── Pagapoco.Infraestructure
    └── Pagapoco.Service

Pagapoco.Web.MVC
    └── Pagapoco.API  (+ consume API vía HTTP en runtime)
```

---

## Stack tecnológico

| Tecnología | Uso |
|-----------|-----|
| **.NET 9** | Todos los proyectos principales |
| **ASP.NET Core Web API** | `Pagapoco.API` |
| **ASP.NET Core MVC** | `Pagapoco.Web.MVC` |
| **Entity Framework Core 9** | ORM para acceso a datos |
| **SQL Server** | Base de datos relacional |
| **JWT (JwtBearer)** | Autenticación y autorización |
| **Swagger / Swashbuckle** | Documentación interactiva de la API |
| **PBKDF2-SHA256** | Hash seguro de contraseñas |
| **HttpClient** | Comunicación entre MVC y API |
| **System.Linq.Dynamic.Core** | Consultas LINQ dinámicas |
