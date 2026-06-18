# Prueba Técnica CCL - Backend

Este proyecto es la solución de la API para la prueba técnica, construido con **.NET 9 (C#)** y **PostgreSQL**. Implementa autenticación por JWT y control de inventario usando Entity Framework Core.

## Requisitos Previos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) instalado.
- PostgreSQL instalado y corriendo en tu máquina local.

## Configuración de la Base de Datos

El proyecto está configurado para conectarse a una base de datos local llamada `inventario_ccl`. Las credenciales por defecto están en `appsettings.json`.
1. Asegúrate de tener una base de datos creada en Postgres llamada `inventario_ccl`.
2. Las tablas se generarán automáticamente mediante la migración y **los datos iniciales se sembrarán (crearán) solos** la primera vez que arranques la aplicación.

## Ejecución del Proyecto

1. Abre tu terminal en esta carpeta.
2. Restaura los paquetes y dependencias (si no lo has hecho):
   ```bash
   dotnet restore
   ```
3. Asegura que la migración se haya aplicado a tu BD:
   ```bash
   dotnet ef database update
   ```
4. Corre el proyecto:
   ```bash
   dotnet run
   ```
   El servidor iniciará en http://localhost:5200.

## Credenciales de Acceso (Hardcodeadas)
Tal como lo solicita el requerimiento técnico, el usuario existe fijamente en memoria y no en BD:
- **Usuario:** admin
- **Contraseña:** admin123
