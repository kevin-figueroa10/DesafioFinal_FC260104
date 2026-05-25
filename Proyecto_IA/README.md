# 🤖 Versión IA - Sistema de Gestión de Biblioteca Universitaria

## Descripción

Este es un **proyecto separado** que contiene la versión del **Sistema Integral de Gestión de Biblioteca Universitaria** generada completamente con **Inteligencia Artificial** (ChatGPT/Claude).

## 📦 Contenido

- **codigo_generado.cs** - 1,185 líneas de código C# generado por IA
- **Program.cs** - Punto de entrada de la aplicación
- **BibliotecaUniversitariaIA.csproj** - Configuración del proyecto
- **Data/** - Archivos de datos (libros.csv, usuarios.txt, prestamos.txt)

## 🚀 Cómo Ejecutar

### Opción 1: Desde PowerShell (Recomendado)

```powershell
cd "C:\Users\figue\source\repos\Proyecto_final\Proyecto_IA"
dotnet restore
dotnet run
```

### Opción 2: Desde Visual Studio

1. Abre Visual Studio Community 2026
2. Abre el archivo `BibliotecaUniversitariaIA.csproj`
3. Presiona F5 o Ctrl+F5 para ejecutar

### Opción 3: Compilar y luego ejecutar

```powershell
cd "C:\Users\figue\source\repos\Proyecto_final\Proyecto_IA"
dotnet build
.\bin\Debug\net10.0\BibliotecaUniversitariaIA.exe
```

## ✨ Características

- ✅ 3 módulos completos (Libros, Usuarios, Préstamos)
- ✅ Validaciones robustas de datos
- ✅ Persistencia en archivos (CSV y TXT)
- ✅ Interfaz profesional con tablas Unicode
- ✅ Generado completamente por IA sin modificaciones
- ✅ Código compilable y funcional

## 📋 Módulos Disponibles

### Módulo de Libros
- Registrar, buscar, actualizar, eliminar libros
- Máximo 10 libros en el sistema
- Validación de código (LIBxxxxx)

### Módulo de Usuarios
- Registrar, buscar, cambiar estado de usuarios
- Máximo 5 usuarios en el sistema
- Validación de carné (8 dígitos numéricos)
- Validación de email

### Módulo de Préstamos
- Registrar préstamo con validaciones
- Registrar devolución con cálculo de retraso
- Ver historial de préstamos
- Período de préstamo: 14 días

## 🔍 Datos de Prueba

El proyecto incluye datos precargados:
- **9 libros** de ejemplo
- **5 usuarios** de ejemplo
- **7 préstamos** históricos

## 🛠️ Requisitos

- .NET 10 SDK
- Visual Studio Community 2026 o superior (opcional)
- PowerShell

## 📚 Documentación

Para más información sobre el proyecto completo, ver:
- `../Proyecto_final/README.md` - Documentación completa
- `../Proyecto_final/prompt_utilizado.txt` - Prompt ingresado a IA

## 📝 Información del Estudiante

- **Carné**: FC260104
- **Nombre**: Kevin Isaac Figueroa Calderón
- **Repositorio**: https://github.com/kevin-figueroa10/DesafioFinal_FC260104

## ✅ Estado

- ✅ Compilación: Exitosa
- ✅ Ejecución: Completamente funcional
- ✅ Documentación: Completa

---

**Nota**: Este proyecto es una solución separada para la versión IA. El proyecto original está en `../Proyecto_final/`.
