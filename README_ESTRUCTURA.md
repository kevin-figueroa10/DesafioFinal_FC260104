# 🎓 Sistema Integral de Gestión de Biblioteca Universitaria

## 📂 Estructura del Repositorio

Este repositorio contiene **DOS versiones** del proyecto:

### 1️⃣ **Proyecto_final/** - Versión Manual (Original)
La versión implementada manualmente en arquitectura de capas con:
- ✅ Código fuente organizado (Models, Services, Data, UI)
- ✅ Validaciones centralizadas
- ✅ Interfaz profesional con tablas
- ✅ Persistencia en archivos

**Ejecutar:**
```powershell
cd Proyecto_final
dotnet run
```

### 2️⃣ **Proyecto_IA/** - Versión Generada con IA
La versión completa generada por IA (ChatGPT/Claude) en un único archivo:
- ✅ 1,185 líneas en `codigo_generado.cs`
- ✅ Todas las características implementadas
- ✅ Código compilable y funcional
- ✅ Mismo comportamiento que la versión manual

**Ejecutar:**
```powershell
cd Proyecto_IA
dotnet run
```

---

## 🚀 Cómo Ejecutar Cualquier Versión

### Versión Manual (Recomendado para revisar código)
```powershell
cd Proyecto_final
dotnet restore
dotnet run
```

### Versión IA (Código generado por IA)
```powershell
cd Proyecto_IA
dotnet restore
dotnet run
```

---

## 📋 Información del Proyecto

- **Estudiante**: Kevin Isaac Figueroa Calderón
- **Carné**: FC260104
- **Asignatura**: Programación II
- **Desafío**: Sistema Integral de Gestión de Biblioteca Universitaria

---

## ✨ Características Principales

### Módulo de Libros
- ✅ Registrar, buscar, actualizar, eliminar
- ✅ Máximo 10 libros
- ✅ Validación de código (LIBxxxxx)

### Módulo de Usuarios
- ✅ Registrar, buscar, cambiar estado
- ✅ Máximo 5 usuarios
- ✅ Validación de carné (8 dígitos)
- ✅ Validación de email

### Módulo de Préstamos
- ✅ Registrar préstamo con validaciones
- ✅ Registrar devolución con retraso
- ✅ Ver historial
- ✅ Período: 14 días

---

## 📁 Contenido de Cada Carpeta

### Proyecto_final/ (Versión Manual)
```
├── Program.cs                          # Punto de entrada
├── Models/                             # Structs
│   ├── Libro.cs
│   ├── Usuario.cs
│   └── Prestamo.cs
├── Services/                           # Lógica de negocio
│   ├── LibroService.cs
│   ├── UsuarioService.cs
│   └── PrestamoService.cs
├── Data/                               # Persistencia
│   ├── FileManager.cs
│   ├── libros.csv
│   ├── usuarios.txt
│   └── prestamos.txt
├── UI/                                 # Interfaz
│   └── Menu.cs
├── Utilidades/                         # Validaciones
│   └── ValidatorHelper.cs
├── README.md
├── RESUMEN_ENTREGA.md
├── ESTADO_FINAL.txt
└── Proyecto_final.csproj
```

### Proyecto_IA/ (Versión IA)
```
├── Program.cs                          # Punto de entrada
├── codigo_generado.cs                  # 1,185 líneas de código IA
├── Data/                               # Datos compartidos
│   ├── FileManager.cs
│   ├── libros.csv
│   ├── usuarios.txt
│   └── prestamos.txt
├── README.md                           # Instrucciones IA
├── BibliotecaUniversitariaIA.csproj    # Configuración
└── .gitignore
```

---

## 📚 Documentación

- **README.md** (en cada carpeta) - Instrucciones específicas
- **RESUMEN_ENTREGA.md** - Resumen completo del proyecto
- **ESTADO_FINAL.txt** - Checklist de requisitos
- **prompt_utilizado.txt** - Prompt ingresado a IA

---

## 🔗 Enlaces Importantes

- **Repositorio**: https://github.com/kevin-figueroa10/DesafioFinal_FC260104
- **Rama Principal**: main
- **Commits**: 23+ commits descriptivos en español

---

## ✅ Estado del Proyecto

- ✅ Compilación: Exitosa
- ✅ Ejecución: Completamente funcional
- ✅ Documentación: Completa
- ✅ Versión Manual: Implementada
- ✅ Versión IA: Implementada
- ✅ Git: Sincronizado

---

## 💡 Notas Importantes

- Ambas versiones funcionan idénticamente
- Comparten los mismos archivos de datos
- Puede ejecutar una u otra sin problemas
- Todo el código está documentado y comentado
- Las validaciones previenen errores de usuario

---

## 🎯 Próximos Pasos (Opcional)

1. Crear video demostrativo de la aplicación
2. Subirlo a YouTube
3. Actualizar README con el enlace

---

**¡PROYECTO COMPLETADO Y LISTO PARA PRESENTACIÓN!** 🎉

Para ejecutar, simplemente:
```powershell
cd Proyecto_final    # o cd Proyecto_IA
dotnet run
```
