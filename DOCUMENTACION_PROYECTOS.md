# Documentación Completa - Biblioteca Universitaria

## 📋 Índice
1. [Descripción General](#descripción-general)
2. [Proyecto Final (Versión Manual)](#proyecto-final-versión-manual)
3. [Proyecto IA (Versión Generada por IA)](#proyecto-ia-versión-generada-por-ia)
4. [Cómo Ejecutar Ambas Versiones](#cómo-ejecutar-ambas-versiones)
5. [Comparación de Arquitecturas](#comparación-de-arquitecturas)
6. [Estructura de Carpetas](#estructura-de-carpetas)

---

## Descripción General

Este repositorio contiene **dos implementaciones** del sistema de **Gestión de Biblioteca Universitaria**, ambas desarrolladas en **C# .NET 10**:

1. **Proyecto Final**: Versión manual con arquitectura en capas (Models, Services, Data, UI, Utilidades)
2. **Proyecto IA**: Versión consolidada en un único archivo generada por Inteligencia Artificial

Ambas versiones implementan las mismas funcionalidades principales:
- ✅ Gestión de libros (crear, listar, actualizar, eliminar)
- ✅ Gestión de usuarios (registrar, listar, activar/desactivar)
- ✅ Gestión de préstamos (realizar préstamo, devolver libro, listar préstamos)
- ✅ Validaciones y control de datos
- ✅ Persistencia en archivos (CSV y TXT)

---

## Proyecto Final (Versión Manual)

### 📁 Ubicación
```
C:\Users\figue\source\repos\Proyecto_final\Proyecto_final\
```

### 🏗️ Arquitectura
Utiliza una **arquitectura en capas** profesional:

```
Proyecto_final/
├── Models/                 # Clases de dominio (Libro, Usuario, Prestamo)
├── Services/              # Lógica de negocio (ServicioLibros, ServicioUsuarios, ServicioPrestamos)
├── Data/                  # Acceso a datos (FileManager)
├── UI/                    # Interfaz de usuario (Menu)
├── Utilidades/            # Helpers y validadores (ValidadorHelper, etc)
├── Data_Files/            # Archivos de datos (libros.csv, usuarios.txt, prestamos.txt)
├── Program.cs             # Punto de entrada
└── Proyecto_final.csproj  # Configuración del proyecto
```

### 🔧 Características Principales

**Clases Principales:**
- `Libro`: Modelo de datos para libros con propiedades (ID, Título, Autor, etc)
- `Usuario`: Modelo de datos para usuarios con propiedades (Carné, Nombre, Carrera, etc)
- `Prestamo`: Modelo de datos para préstamos con propiedades (ID, Usuario, Libro, Fecha Préstamo, Fecha Devolución)

**Servicios:**
- `ServicioLibros`: Gestiona operaciones CRUD de libros
- `ServicioUsuarios`: Gestiona operaciones CRUD de usuarios
- `ServicioPrestamos`: Gestiona préstamos y devoluciones de libros

**Utilidades:**
- `ValidadorHelper`: Validaciones reutilizables para datos
- Métodos de formateo de tablas y presentación de datos

**Persistencia:**
- `FileManager`: Carga y guarda datos en archivos de texto (CSV y TXT)

### 💻 Cómo Ejecutar Proyecto Final

```powershell
# Navegar a la carpeta del proyecto
cd "C:\Users\figue\source\repos\Proyecto_final\Proyecto_final"

# Restaurar dependencias
dotnet restore

# Compilar
dotnet build

# Ejecutar
dotnet run
```

### 📊 Menú Principal
Cuando se ejecuta, muestra las siguientes opciones:
1. **Gestionar Libros** (Crear, Listar, Actualizar, Eliminar)
2. **Gestionar Usuarios** (Registrar, Listar, Activar/Desactivar)
3. **Gestionar Préstamos** (Realizar Préstamo, Devolver, Listar)
4. **Salir**

---

## Proyecto IA (Versión Generada por IA)

### 📁 Ubicación
```
C:\Users\figue\source\repos\Proyecto_final\Proyecto_IA\
```

### 🏗️ Arquitectura
Utiliza una **arquitectura consolidada en un único archivo** (`codigo_generado.cs`):

```
Proyecto_IA/
├── codigo_generado.cs           # Implementación completa en UN archivo
│   ├── Namespace: BibliotecaUniversitaria
│   ├── Clases de modelos (Libro, Usuario, Prestamo)
│   ├── Clases de servicios (ServicioLibros, ServicioUsuarios, ServicioPrestamos)
│   ├── Clase GestorArchivos (persistencia)
│   ├── Clase MenuPrincipal (UI)
│   └── Clase Program (punto de entrada global)
├── Program.cs                   # Bootstrap (inicializa servicios)
├── Data/                        # Archivos de datos
│   ├── libros.csv
│   ├── usuarios.txt
│   └── prestamos.txt
├── BibliotecaUniversitariaIA.csproj  # Configuración del proyecto
├── README.md                    # Instrucciones de ejecución
└── .gitignore                   # Archivo para ignorar en Git
```

### 🔧 Características Principales

**Diferencias con Proyecto Final:**
- ✨ **Consolidado en un archivo**: Todas las clases en `codigo_generado.cs`
- ✨ **Namespace único**: `BibliotecaUniversitaria`
- ✨ **Lógica más compacta**: Optimizada por IA
- ✨ **Misma funcionalidad**: Implementa exactamente lo mismo que la versión manual

**Clases Incluidas:**
1. **Modelos de datos**: `Libro`, `Usuario`, `Prestamo`
2. **Servicios**: `ServicioLibros`, `ServicioUsuarios`, `ServicioPrestamos`
3. **Gestor de archivos**: `GestorArchivos` (persistencia)
4. **Interfaz de usuario**: `MenuPrincipal`

### 💻 Cómo Ejecutar Proyecto IA

```powershell
# Navegar a la carpeta del proyecto IA
cd "C:\Users\figue\source\repos\Proyecto_final\Proyecto_IA"

# Restaurar dependencias
dotnet restore

# Compilar
dotnet build

# Ejecutar
dotnet run
```

### 📊 Interfaz
Presenta el mismo menú interactivo que el proyecto manual, con todas las opciones de gestión de libros, usuarios y préstamos.

---

## Cómo Ejecutar Ambas Versiones

### Opción 1: Ejecutar desde Visual Studio

1. Abrir `C:\Users\figue\source\repos\Proyecto_final\` en Visual Studio
2. En el **Explorador de Soluciones**, hacer clic derecho en el proyecto deseado
3. Seleccionar **Establecer como Proyecto de inicio**
4. Presionar `F5` o `Ctrl+F5` para ejecutar

### Opción 2: Ejecutar desde PowerShell

**Para Proyecto Final:**
```powershell
cd "C:\Users\figue\source\repos\Proyecto_final\Proyecto_final"
dotnet run
```

**Para Proyecto IA:**
```powershell
cd "C:\Users\figue\source\repos\Proyecto_final\Proyecto_IA"
dotnet run
```

### Opción 3: Compilar y ejecutar el ejecutable

**Proyecto Final:**
```powershell
cd "C:\Users\figue\source\repos\Proyecto_final\Proyecto_final"
dotnet build -c Release
# El ejecutable está en: bin/Release/net10.0/Proyecto_final.exe
.\bin\Release\net10.0\Proyecto_final.exe
```

**Proyecto IA:**
```powershell
cd "C:\Users\figue\source\repos\Proyecto_final\Proyecto_IA"
dotnet build -c Release
# El ejecutable está en: bin/Release/net10.0/BibliotecaUniversitariaIA.exe
.\bin\Release\net10.0\BibliotecaUniversitariaIA.exe
```

---

## Comparación de Arquitecturas

| Aspecto | Proyecto Final | Proyecto IA |
|--------|----------------|------------|
| **Estructura** | En capas (Models, Services, Data, UI) | Consolidada (un archivo) |
| **Número de archivos .cs** | Múltiples (~10+ archivos) | Un archivo + Program.cs |
| **Mantenibilidad** | Mayor, separación clara | Menor, código más compacto |
| **Escalabilidad** | Mejor para proyectos grandes | Limitada |
| **Namespace** | `Proyecto_final.*` | `BibliotecaUniversitaria` |
| **Persistencia** | `FileManager` en Data/ | `GestorArchivos` en mismo archivo |
| **Target Framework** | .NET 10 | .NET 10 |
| **Funcionalidad** | Idéntica | Idéntica |

---

## Estructura de Carpetas

```
Proyecto_final/
├── Proyecto_final/                    # Proyecto manual en capas
│   ├── Models/
│   ├── Services/
│   ├── Data/
│   ├── UI/
│   ├── Utilidades/
│   ├── Data_Files/
│   ├── Program.cs
│   └── Proyecto_final.csproj
│
├── Proyecto_IA/                       # Proyecto IA consolidado
│   ├── codigo_generado.cs
│   ├── Program.cs
│   ├── Data/
│   ├── BibliotecaUniversitariaIA.csproj
│   ├── README.md
│   └── .gitignore
│
├── DOCUMENTACION_PROYECTOS.md         # Este archivo
├── PROMPTS_GENERACION_IA.txt          # Documentación de prompts
├── README_ESTRUCTURA.md               # Documentación adicional
└── .git/                              # Control de versiones Git
```

---

## Notas Importantes

### 📌 Sobre el Proyecto Final
- Es la implementación **original y manual** desarrollada en clase
- Utiliza patrones profesionales de arquitectura en capas
- Ideal para aprender sobre estructuración de proyectos C# empresariales

### 📌 Sobre el Proyecto IA
- Fue **generado por Inteligencia Artificial** basado en prompts específicos
- Implementa exactamente las mismas funcionalidades que el manual
- Demostrativo del potencial de la IA en generación de código
- Para más detalles sobre los prompts usados, ver `PROMPTS_GENERACION_IA.txt`

### 📌 Datos de Prueba
Ambos proyectos incluyen los mismos archivos de datos:
- **libros.csv**: 5 libros de ejemplo
- **usuarios.txt**: 5 usuarios de ejemplo con datos separados por `|`
- **prestamos.txt**: 2 préstamos de ejemplo

### 📌 Requisitos Previos
- **.NET 10 SDK** instalado
- **Visual Studio 2026** (opcional, se puede usar CLI)
- **PowerShell** (para ejecutar comandos)

---

## Contacto / Información del Autor

**Nombre:** Kevin Isaac Figueroa Calderón  
**Carné:** FC260104  
**Universidad:** (Especificar si es necesario)  
**Fecha de creación:** 2025

---

**Última actualización:** 2025  
**Versión:** 1.0
