using System.Globalization;
using Proyecto_final.Models;
using Proyecto_final.Utilidades;

namespace Proyecto_final.Data;

/// <summary>
/// Gestiona la carga y guardado de archivos de datos.
/// </summary>
public static class FileManager
{
    private static readonly string DataPath = GetDataPath();

    /// <summary>
    /// Propiedad pública para verificar la ruta de datos (debug)
    /// </summary>
    public static string RutaDatos => DataPath;

    private static string GetDataPath()
    {
        // Usar el directorio del ensamblado actual
        string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
        string binDir = Path.GetDirectoryName(assemblyLocation) ?? AppDomain.CurrentDomain.BaseDirectory;

        // Buscar la carpeta Data subiendo desde bin
        DirectoryInfo current = new DirectoryInfo(binDir);

        for (int i = 0; i < 6; i++)
        {
            if (current == null)
                break;

            string dataPath = Path.Combine(current.FullName, "Data");
            if (Directory.Exists(dataPath))
            {
                return dataPath;
            }

            current = current.Parent;
        }

        // Fallback: usar la carpeta Data en el directorio raíz del proyecto
        string projectRoot = new DirectoryInfo(binDir).Parent?.Parent?.Parent?.FullName ?? binDir;
        string defaultDataPath = Path.Combine(projectRoot, "Data");

        if (!Directory.Exists(defaultDataPath))
        {
            Directory.CreateDirectory(defaultDataPath);
        }

        return defaultDataPath;
    }

    static FileManager()
    {
        if (!Directory.Exists(DataPath))
        {
            Directory.CreateDirectory(DataPath);
        }
    }

    #region Libros

    /// <summary>
    /// Carga los libros desde libros.csv.
    /// </summary>
    public static List<Libro> CargarLibros()
    {
        string rutaArchivo = Path.Combine(DataPath, "libros.csv");
        List<Libro> libros = new();

        if (!File.Exists(rutaArchivo))
            return libros;

        try
        {
            string[] lineas = File.ReadAllLines(rutaArchivo);
            foreach (string linea in lineas.Skip(1)) // Saltar encabezado
            {
                if (string.IsNullOrWhiteSpace(linea))
                    continue;

                string[] partes = linea.Split(',');
                if (partes.Length >= 7)
                {
                    var libro = new Libro
                    {
                        Codigo = partes[0].Trim(),
                        Titulo = partes[1].Trim(),
                        Autor = partes[2].Trim(),
                        Editorial = partes[3].Trim(),
                        AnoPublicacion = int.TryParse(partes[4].Trim(), out int ano) ? ano : 0,
                        Categoria = partes[5].Trim(),
                        CantidadEjemplares = int.TryParse(partes[6].Trim(), out int cant) ? cant : 0
                    };
                    libros.Add(libro);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar libros: {ex.Message}");
        }

        return libros;
    }

    /// <summary>
    /// Guarda los libros en libros.csv.
    /// </summary>
    public static void GuardarLibros(List<Libro> libros)
    {
        string rutaArchivo = Path.Combine(DataPath, "libros.csv");

        try
        {
            var lineas = new List<string> { "Codigo,Titulo,Autor,Editorial,AnoPublicacion,Categoria,CantidadEjemplares" };

            foreach (var libro in libros)
            {
                lineas.Add($"{libro.Codigo},{libro.Titulo},{libro.Autor},{libro.Editorial},{libro.AnoPublicacion},{libro.Categoria},{libro.CantidadEjemplares}");
            }

            File.WriteAllLines(rutaArchivo, lineas);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar libros: {ex.Message}");
        }
    }

    #endregion

    #region Usuarios

    /// <summary>
    /// Carga los usuarios desde usuarios.txt.
    /// </summary>
    public static List<Usuario> CargarUsuarios()
    {
        string rutaArchivo = Path.Combine(DataPath, "usuarios.txt");
        List<Usuario> usuarios = new();

        if (!File.Exists(rutaArchivo))
        {
            Console.WriteLine($"⚠ Archivo no encontrado: {rutaArchivo}");
            return usuarios;
        }

        try
        {
            string[] lineas = File.ReadAllLines(rutaArchivo);
            Console.WriteLine($"✓ Leyendo {lineas.Length} líneas de {rutaArchivo}");

            foreach (string linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea))
                    continue;

                string[] partes = linea.Split('|');
                if (partes.Length >= 6)
                {
                    var usuario = new Usuario
                    {
                        Carne = partes[0].Trim(),
                        NombreCompleto = partes[1].Trim(),
                        Carrera = partes[2].Trim(),
                        CorreoElectronico = partes[3].Trim(),
                        Telefono = partes[4].Trim(),
                        Activo = partes[5].Trim().Equals("activo", StringComparison.OrdinalIgnoreCase)
                    };
                    usuarios.Add(usuario);
                    Console.WriteLine($"  → Usuario cargado: {usuario.Carne}");
                }
            }

            Console.WriteLine($"✓ Total usuarios cargados: {usuarios.Count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar usuarios: {ex.Message}");
        }

        return usuarios;
    }

    /// <summary>
    /// Guarda los usuarios en usuarios.txt.
    /// </summary>
    public static void GuardarUsuarios(List<Usuario> usuarios)
    {
        string rutaArchivo = Path.Combine(DataPath, "usuarios.txt");

        try
        {
            var lineas = new List<string>();

            foreach (var usuario in usuarios)
            {
                string estado = usuario.Activo ? "activo" : "inactivo";
                lineas.Add($"{usuario.Carne}|{usuario.NombreCompleto}|{usuario.Carrera}|{usuario.CorreoElectronico}|{usuario.Telefono}|{estado}");
            }

            File.WriteAllLines(rutaArchivo, lineas);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar usuarios: {ex.Message}");
        }
    }

    #endregion

    #region Préstamos

    /// <summary>
    /// Carga los préstamos desde prestamos.txt.
    /// </summary>
    public static List<Prestamo> CargarPrestamos()
    {
        string rutaArchivo = Path.Combine(DataPath, "prestamos.txt");
        List<Prestamo> prestamos = new();

        if (!File.Exists(rutaArchivo))
            return prestamos;

        try
        {
            string[] lineas = File.ReadAllLines(rutaArchivo);
            foreach (string linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea))
                    continue;

                string[] partes = linea.Split('|');
                if (partes.Length >= 4)
                {
                    var prestamo = new Prestamo
                    {
                        Carne = partes[0].Trim(),
                        CodigoLibro = partes[1].Trim(),
                        FechaPrestamo = ValidatorHelper.ConvertirFecha(partes[2].Trim()) ?? DateTime.Now,
                        FechaEstimadaDevolucion = ValidatorHelper.ConvertirFecha(partes[3].Trim()) ?? DateTime.Now.AddDays(14),
                        FechaDevolucion = partes.Length > 4 && !string.IsNullOrWhiteSpace(partes[4].Trim()) 
                            ? ValidatorHelper.ConvertirFecha(partes[4].Trim()) 
                            : null
                    };
                    prestamos.Add(prestamo);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar préstamos: {ex.Message}");
        }

        return prestamos;
    }

    /// <summary>
    /// Guarda los préstamos en prestamos.txt.
    /// </summary>
    public static void GuardarPrestamos(List<Prestamo> prestamos)
    {
        string rutaArchivo = Path.Combine(DataPath, "prestamos.txt");

        try
        {
            var lineas = new List<string>();

            foreach (var prestamo in prestamos)
            {
                string devolucion = prestamo.FechaDevolucion.HasValue 
                    ? prestamo.FechaDevolucion.Value.ToString("dd/MM/yyyy") 
                    : string.Empty;

                lineas.Add($"{prestamo.Carne}|{prestamo.CodigoLibro}|{prestamo.FechaPrestamo:dd/MM/yyyy}|{prestamo.FechaEstimadaDevolucion:dd/MM/yyyy}|{devolucion}");
            }

            File.WriteAllLines(rutaArchivo, lineas);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar préstamos: {ex.Message}");
        }
    }

    #endregion
}
