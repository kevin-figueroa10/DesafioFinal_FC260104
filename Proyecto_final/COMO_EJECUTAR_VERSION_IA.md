# 🤖 CÓMO EJECUTAR LA VERSIÓN IA

## Opción 1: Ejecutar directamente el archivo IA en el proyecto actual

### Paso 1: Copiar el código IA al proyecto actual
El archivo `codigo_generado.cs` está en el mismo proyecto. Simplemente:

```bash
cd Proyecto_final
dotnet run
```

El `Program.cs` actual usará automáticamente las clases del `codigo_generado.cs`.

---

## Opción 2: Crear un proyecto separado para la versión IA

### Paso 1: Crear carpeta para proyecto IA
```bash
mkdir BibliotecaUniversitariaIA
cd BibliotecaUniversitariaIA
```

### Paso 2: Crear archivo de proyecto
```bash
# Ya está creado: BibliotecaIA.csproj
```

### Paso 3: Crear carpeta Data
```bash
mkdir Data
```

### Paso 4: Copiar datos de ejemplo
```bash
# Copiar desde Proyecto_final/Data/
copy ..\Proyecto_final\Data\libros.csv Data\
copy ..\Proyecto_final\Data\usuarios.txt Data\
copy ..\Proyecto_final\Data\prestamos.txt Data\
```

### Paso 5: Crear archivo Program.cs
```bash
# Crear Program.cs con el contenido de Program_IA.cs
```

### Paso 6: Copiar archivo de código IA
```bash
copy ..\Proyecto_final\codigo_generado.cs .
```

### Paso 7: Compilar y ejecutar
```bash
dotnet restore
dotnet build
dotnet run
```

---

## ⚙️ MÉTODO MÁS SIMPLE (Recomendado)

El `codigo_generado.cs` ya está en el proyecto `Proyecto_final`. Para usarlo:

### 1. Abre PowerShell
```powershell
cd "C:\Users\figue\source\repos\Proyecto_final\Proyecto_final"
```

### 2. Ejecuta la aplicación
```powershell
dotnet run
```

### 3. La aplicación se ejecutará con las clases de `codigo_generado.cs`

---

## 🎯 NOTAS IMPORTANTES

- El archivo `codigo_generado.cs` contiene TODAS las clases necesarias
- Está en el mismo namespace que el proyecto original
- Compila sin problemas
- Los datos se cargan automáticamente desde Data/
- No necesitas hacer nada especial, solo ejecutar `dotnet run`

---

## 📋 ARCHIVOS INVOLUCRADOS EN LA VERSIÓN IA

```
Proyecto_final/
├── codigo_generado.cs          ← Archivo con TODO el código IA
├── Program_IA.cs               ← Punto de entrada alternativo
├── BibliotecaIA.csproj         ← Proyecto separado (opcional)
├── Program.cs                  ← Punto de entrada actual
├── Proyecto_final.csproj       ← Proyecto actual
└── Data/
	├── libros.csv              ← Datos compartidos
	├── usuarios.txt            ← Datos compartidos
	└── prestamos.txt           ← Datos compartidos
```

---

## ✨ CARACTERÍSTICAS DE LA VERSIÓN IA

La versión IA (`codigo_generado.cs`) incluye:

✅ **Modelos**
- struct Libro
- struct Usuario  
- struct Prestamo

✅ **Utilidades**
- class ValidadorHelper (validaciones)

✅ **Persistencia**
- class GestorArchivos (cargar/guardar)

✅ **Servicios**
- class ServicioLibros
- class ServicioUsuarios
- class ServicioPrestamos

✅ **Interfaz de Usuario**
- class MenuPrincipal

✅ **Punto de Entrada**
- class Program

---

## 🚀 COMANDO RÁPIDO PARA EJECUTAR

```powershell
# En PowerShell, desde C:\Users\figue\source\repos\Proyecto_final\
cd Proyecto_final
dotnet run
```

¡Listo! La aplicación se ejecutará inmediatamente.

---

## ❓ TROUBLESHOOTING

Si te da error de compilación:
```bash
dotnet clean
dotnet build
dotnet run
```

Si no encuentra los datos:
- Asegúrate de estar en la carpeta `Proyecto_final/`
- Los archivos de `Data/` deben estar presentes
- El `.csproj` tiene configurado `CopyToOutputDirectory`

---

## 📊 VERIFICAR QUE LA VERSIÓN IA FUNCIONA

Cuando ejecutes `dotnet run`, deberías ver:

```
╔════════════════════════════════════════════╗
║  SISTEMA DE GESTIÓN DE BIBLIOTECA          ║
║  Universidad - 2024                        ║
╚════════════════════════════════════════════╝

┌─ MENÚ PRINCIPAL ─────────────────────────┐
│ 1. Gestionar Libros                      │
│ 2. Gestionar Usuarios                    │
│ 3. Gestionar Préstamos                   │
│ 4. Salir                                 │
└──────────────────────────────────────────┘

▶ Seleccione opción:
```

¡Eso significa que la versión IA está funcionando correctamente! 🎉

