# Sistema Integral de Gestión de Biblioteca Universitaria

## Información del Estudiante
- **Carné**: [Ingresa tu carné aquí]
- **Nombre Completo**: [Ingresa tu nombre aquí]

## Descripción del Proyecto
Sistema de gestión de biblioteca universitaria desarrollado en C# con .NET 10. Permite administrar libros, usuarios y préstamos con validaciones completas y persistencia de datos en archivos.

## Características Principales
- ✅ Gestión de Libros (registrar, buscar, actualizar, eliminar)
- ✅ Gestión de Usuarios (registrar, buscar, cambiar estado)
- ✅ Gestión de Préstamos (registrar, devoluciones, historial)
- ✅ Validaciones completas de datos
- ✅ Persistencia en archivos CSV y TXT
- ✅ Interfaz de consola profesional con tablas

## Requisitos Previos
- .NET 10 SDK
- Visual Studio Community 2026 o superior
- PowerShell o terminal compatible

## Instrucciones para Clonar y Ejecutar

### 1. Clonar el Repositorio
```bash
git clone https://github.com/tu-usuario/DesafioFinal_[Carné1]_[Carné2].git
cd DesafioFinal_[Carné1]_[Carné2]
```

### 2. Restaurar Dependencias
```bash
cd Proyecto_final
dotnet restore
```

### 3. Compilar el Proyecto
```bash
dotnet build
```

### 4. Ejecutar la Aplicación
```bash
dotnet run
```

## Estructura del Proyecto
```
Proyecto_final/
├── Program.cs                 # Punto de entrada de la aplicación
├── Models/                    # Modelos de datos
│   ├── Libro.cs
│   ├── Usuario.cs
│   └── Prestamo.cs
├── Services/                  # Servicios de lógica de negocio
│   ├── LibroService.cs
│   ├── UsuarioService.cs
│   └── PrestamoService.cs
├── Data/                      # Gestión de datos
│   ├── FileManager.cs
│   ├── libros.csv
│   ├── usuarios.txt
│   └── prestamos.txt
├── UI/                        # Interfaz de usuario
│   └── Menu.cs
├── Utilidades/                # Helpers y validadores
│   └── ValidatorHelper.cs
└── Proyecto_final.csproj      # Configuración del proyecto
```

## Instrucciones de Uso

### Menú Principal
1. **Gestionar Libros** - CRUD de libros con validaciones
2. **Gestionar Usuarios** - CRUD de usuarios con estado activo/inactivo
3. **Gestionar Préstamos** - Registrar préstamos y devoluciones
4. **Salir** - Cerrar la aplicación

### Validaciones
- Códigos de libro: Formato LIBxxxxx
- Carné de estudiante: 8 dígitos numéricos
- Correo electrónico: Debe contener @ y punto
- Límites: Max 10 libros, 5 usuarios, 10 préstamos activos
- Periodo de devolución: 14 días

## Datos de Ejemplo
El sistema incluye datos precargados para demostración:
- 9 libros registrados
- 5 usuarios registrados
- 7 préstamos históricos

## Video Demostrativo
[Incluir enlace al video demostrativo aquí]

## Notas Adicionales
- Todos los datos se persisten en archivos en la carpeta `Data/`
- Las validaciones aseguran integridad de datos
- La interfaz utiliza caracteres Unicode para tablas profesionales
- El sistema implementa separación de responsabilidades y buenas prácticas de programación

## Licencia
Proyecto de educación universitaria - 2026

