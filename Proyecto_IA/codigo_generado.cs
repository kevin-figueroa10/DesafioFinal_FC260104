// ========================================================================
// SISTEMA INTEGRAL DE GESTIÓN DE BIBLIOTECA UNIVERSITARIA
// Versión generada con IA (Claude/ChatGPT)
// Código consolidado en un único archivo como lo requiere el desafío
// ========================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BibliotecaUniversitaria
{
    // =====================================================================
    // MODELOS DE DATOS
    // =====================================================================

    /// <summary>
    /// Representa un libro en la biblioteca
    /// </summary>
    public struct Libro
    {
        public string Codigo { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Editorial { get; set; }
        public int AnoPublicacion { get; set; }
        public string Categoria { get; set; }
        public int CantidadEjemplares { get; set; }

        public override string ToString()
        {
            return $"{Codigo} | {Titulo} | {Autor} | {Editorial} | {AnoPublicacion} | {Categoria} | {CantidadEjemplares}";
        }
    }

    /// <summary>
    /// Representa un usuario (estudiante) del sistema
    /// </summary>
    public struct Usuario
    {
        public string Carne { get; set; }
        public string NombreCompleto { get; set; }
        public string Carrera { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
        public bool Activo { get; set; }

        public override string ToString()
        {
            return $"{Carne} | {NombreCompleto} | {Carrera} | {CorreoElectronico} | {Telefono} | {(Activo ? "Activo" : "Inactivo")}";
        }
    }

    /// <summary>
    /// Representa un préstamo de libro a un usuario
    /// </summary>
    public struct Prestamo
    {
        public string Carne { get; set; }
        public string CodigoLibro { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaEstimadaDevolucion { get; set; }
        public DateTime? FechaDevolucion { get; set; }

        public bool EstaActivo => FechaDevolucion == null;

        public int DiasRetraso
        {
            get
            {
                if (FechaDevolucion == null)
                    return 0;
                var dias = (FechaDevolucion.Value - FechaEstimadaDevolucion).Days;
                return dias > 0 ? dias : 0;
            }
        }

        public override string ToString()
        {
            var devolucion = FechaDevolucion?.ToString("dd/MM/yyyy") ?? "Pendiente";
            var estado = EstaActivo ? "Activo" : $"Devuelto (Retraso: {DiasRetraso} días)";
            return $"{Carne} | {CodigoLibro} | {FechaPrestamo:dd/MM/yyyy} | {FechaEstimadaDevolucion:dd/MM/yyyy} | {devolucion} | {estado}";
        }
    }

    // =====================================================================
    // UTILIDADES DE VALIDACIÓN
    // =====================================================================

    public static class ValidadorHelper
    {
        // Validar código de libro (formato LIBxxxxx)
        public static bool ValidarCodigoLibro(string codigo)
        {
            return !string.IsNullOrWhiteSpace(codigo) && 
                   Regex.IsMatch(codigo, @"^LIB\d{5}$");
        }

        // Validar carné (8 dígitos numéricos)
        public static bool ValidarCarne(string carne)
        {
            return !string.IsNullOrWhiteSpace(carne) && 
                   Regex.IsMatch(carne, @"^\d{8}$");
        }

        // Validar correo electrónico
        public static bool ValidarCorreoElectronico(string correo)
        {
            return !string.IsNullOrWhiteSpace(correo) && 
                   Regex.IsMatch(correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        // Validar año de publicación
        public static bool ValidarAnio(int ano)
        {
            var anoActual = DateTime.Now.Year;
            return ano > 1000 && ano <= anoActual;
        }

        // Validar cantidad de ejemplares
        public static bool ValidarCantidad(int cantidad)
        {
            return cantidad > 0 && cantidad <= 100;
        }

        // Validar formato de fecha
        public static bool ValidarFormatoFecha(string fecha)
        {
            return DateTime.TryParseExact(fecha, "dd/MM/yyyy", null, 
                System.Globalization.DateTimeStyles.None, out _);
        }

        // Convertir string a DateTime
        public static DateTime ConvertirFecha(string fecha)
        {
            DateTime.TryParseExact(fecha, "dd/MM/yyyy", null,
                System.Globalization.DateTimeStyles.None, out var resultado);
            return resultado;
        }

        // Intentar parsear entero de forma segura
        public static bool TryParseEntero(string texto, out int resultado)
        {
            return int.TryParse(texto, out resultado);
        }
    }

    // =====================================================================
    // CAPA DE PERSISTENCIA
    // =====================================================================

    public class GestorArchivos
    {
        private readonly string _rutaDatos;

        public GestorArchivos()
        {
            _rutaDatos = ObtenerRutaDatos();
            if (!Directory.Exists(_rutaDatos))
                Directory.CreateDirectory(_rutaDatos);
        }

        private string ObtenerRutaDatos()
        {
            var ejecutable = AppDomain.CurrentDomain.BaseDirectory;
            var rutaData = Path.Combine(ejecutable, "Data");
            if (Directory.Exists(rutaData))
                return rutaData;

            var actual = new DirectoryInfo(ejecutable);
            while (actual != null)
            {
                rutaData = Path.Combine(actual.FullName, "Data");
                if (Directory.Exists(rutaData))
                    return rutaData;
                actual = actual.Parent;
            }

            return Path.Combine(ejecutable, "Data");
        }

        // ===================== LIBROS =====================

        public List<Libro> CargarLibros()
        {
            var ruta = Path.Combine(_rutaDatos, "libros.csv");
            var libros = new List<Libro>();

            if (!File.Exists(ruta))
                return libros;

            var lineas = File.ReadAllLines(ruta);
            for (int i = 1; i < lineas.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lineas[i])) continue;
                var partes = lineas[i].Split(',');
                if (partes.Length == 7)
                {
                    libros.Add(new Libro
                    {
                        Codigo = partes[0].Trim(),
                        Titulo = partes[1].Trim(),
                        Autor = partes[2].Trim(),
                        Editorial = partes[3].Trim(),
                        AnoPublicacion = int.Parse(partes[4].Trim()),
                        Categoria = partes[5].Trim(),
                        CantidadEjemplares = int.Parse(partes[6].Trim())
                    });
                }
            }
            return libros;
        }

        public void GuardarLibros(List<Libro> libros)
        {
            var ruta = Path.Combine(_rutaDatos, "libros.csv");
            var lineas = new List<string> { "Codigo,Titulo,Autor,Editorial,AnoPublicacion,Categoria,CantidadEjemplares" };
            foreach (var libro in libros)
                lineas.Add($"{libro.Codigo},{libro.Titulo},{libro.Autor},{libro.Editorial},{libro.AnoPublicacion},{libro.Categoria},{libro.CantidadEjemplares}");
            File.WriteAllLines(ruta, lineas);
        }

        // ===================== USUARIOS =====================

        public List<Usuario> CargarUsuarios()
        {
            var ruta = Path.Combine(_rutaDatos, "usuarios.txt");
            var usuarios = new List<Usuario>();

            if (!File.Exists(ruta))
                return usuarios;

            var lineas = File.ReadAllLines(ruta);
            foreach (var linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;
                var partes = linea.Split('|');
                if (partes.Length == 6)
                {
                    usuarios.Add(new Usuario
                    {
                        Carne = partes[0].Trim(),
                        NombreCompleto = partes[1].Trim(),
                        Carrera = partes[2].Trim(),
                        CorreoElectronico = partes[3].Trim(),
                        Telefono = partes[4].Trim(),
                        Activo = bool.Parse(partes[5].Trim())
                    });
                }
            }
            return usuarios;
        }

        public void GuardarUsuarios(List<Usuario> usuarios)
        {
            var ruta = Path.Combine(_rutaDatos, "usuarios.txt");
            var lineas = usuarios.Select(u => 
                $"{u.Carne}|{u.NombreCompleto}|{u.Carrera}|{u.CorreoElectronico}|{u.Telefono}|{u.Activo}").ToList();
            File.WriteAllLines(ruta, lineas);
        }

        // ===================== PRÉSTAMOS =====================

        public List<Prestamo> CargarPrestamos()
        {
            var ruta = Path.Combine(_rutaDatos, "prestamos.txt");
            var prestamos = new List<Prestamo>();

            if (!File.Exists(ruta))
                return prestamos;

            var lineas = File.ReadAllLines(ruta);
            foreach (var linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;
                var partes = linea.Split('|');
                if (partes.Length >= 4)
                {
                    var prestamo = new Prestamo
                    {
                        Carne = partes[0].Trim(),
                        CodigoLibro = partes[1].Trim(),
                        FechaPrestamo = ValidadorHelper.ConvertirFecha(partes[2].Trim()),
                        FechaEstimadaDevolucion = ValidadorHelper.ConvertirFecha(partes[3].Trim()),
                        FechaDevolucion = partes.Length > 4 && !string.IsNullOrWhiteSpace(partes[4].Trim()) 
                            ? ValidadorHelper.ConvertirFecha(partes[4].Trim()) 
                            : null
                    };
                    prestamos.Add(prestamo);
                }
            }
            return prestamos;
        }

        public void GuardarPrestamos(List<Prestamo> prestamos)
        {
            var ruta = Path.Combine(_rutaDatos, "prestamos.txt");
            var lineas = prestamos.Select(p =>
            {
                var devolucion = p.FechaDevolucion?.ToString("dd/MM/yyyy") ?? "";
                return $"{p.Carne}|{p.CodigoLibro}|{p.FechaPrestamo:dd/MM/yyyy}|{p.FechaEstimadaDevolucion:dd/MM/yyyy}|{devolucion}";
            }).ToList();
            File.WriteAllLines(ruta, lineas);
        }
    }

    // =====================================================================
    // SERVICIOS DE LÓGICA DE NEGOCIO
    // =====================================================================

    public class ServicioLibros
    {
        private List<Libro> _libros;
        private readonly GestorArchivos _gestor;
        private const int MAXIMO_LIBROS = 10;

        public ServicioLibros(GestorArchivos gestor)
        {
            _gestor = gestor;
            _libros = _gestor.CargarLibros();
        }

        public (bool, string) RegistrarLibro(string codigo, string titulo, string autor, 
            string editorial, int ano, string categoria, int cantidad)
        {
            if (_libros.Count >= MAXIMO_LIBROS)
                return (false, $"No se pueden registrar más de {MAXIMO_LIBROS} libros");

            if (!ValidadorHelper.ValidarCodigoLibro(codigo))
                return (false, "Código de libro inválido (debe ser LIBxxxxx)");

            if (_libros.Any(l => l.Codigo == codigo))
                return (false, "El código del libro ya existe");

            if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(autor))
                return (false, "Título y autor son obligatorios");

            if (!ValidadorHelper.ValidarAnio(ano))
                return (false, $"Año debe estar entre 1000 y {DateTime.Now.Year}");

            if (!ValidadorHelper.ValidarCantidad(cantidad))
                return (false, "Cantidad debe estar entre 1 y 100");

            _libros.Add(new Libro
            {
                Codigo = codigo,
                Titulo = titulo,
                Autor = autor,
                Editorial = editorial,
                AnoPublicacion = ano,
                Categoria = categoria,
                CantidadEjemplares = cantidad
            });

            _gestor.GuardarLibros(_libros);
            return (true, "Libro registrado exitosamente");
        }

        public Libro? BuscarLibroPorCodigo(string codigo)
        {
            var indice = _libros.FindIndex(l => l.Codigo == codigo);
            return indice >= 0 ? _libros[indice] : null;
        }

        public List<Libro> ObtenerTodosLibros() => _libros;

        public int ObtenerCantidadLibros() => _libros.Count;

        public (bool, string) ActualizarCantidad(string codigo, int nuevaCantidad)
        {
            var indice = _libros.FindIndex(l => l.Codigo == codigo);
            if (indice < 0)
                return (false, "Libro no encontrado");

            if (!ValidadorHelper.ValidarCantidad(nuevaCantidad))
                return (false, "Cantidad inválida");

            var libro = _libros[indice];
            libro.CantidadEjemplares = nuevaCantidad;
            _libros[indice] = libro;
            _gestor.GuardarLibros(_libros);
            return (true, "Cantidad actualizada");
        }

        public (bool, string) EliminarLibro(string codigo)
        {
            var indice = _libros.FindIndex(l => l.Codigo == codigo);
            if (indice < 0)
                return (false, "Libro no encontrado");

            _libros.RemoveAt(indice);
            _gestor.GuardarLibros(_libros);
            return (true, "Libro eliminado");
        }
    }

    public class ServicioUsuarios
    {
        private List<Usuario> _usuarios;
        private readonly GestorArchivos _gestor;
        private const int MAXIMO_USUARIOS = 5;

        public ServicioUsuarios(GestorArchivos gestor)
        {
            _gestor = gestor;
            _usuarios = _gestor.CargarUsuarios();
        }

        public (bool, string) RegistrarUsuario(string carne, string nombre, string carrera, 
            string correo, string telefono)
        {
            if (_usuarios.Count >= MAXIMO_USUARIOS)
                return (false, $"No se pueden registrar más de {MAXIMO_USUARIOS} usuarios");

            if (!ValidadorHelper.ValidarCarne(carne))
                return (false, "Carné inválido (debe ser 8 dígitos numéricos)");

            if (_usuarios.Any(u => u.Carne == carne))
                return (false, "El carné ya está registrado");

            if (!ValidadorHelper.ValidarCorreoElectronico(correo))
                return (false, "Correo electrónico inválido");

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(carrera))
                return (false, "Nombre y carrera son obligatorios");

            _usuarios.Add(new Usuario
            {
                Carne = carne,
                NombreCompleto = nombre,
                Carrera = carrera,
                CorreoElectronico = correo,
                Telefono = telefono,
                Activo = true
            });

            _gestor.GuardarUsuarios(_usuarios);
            return (true, "Usuario registrado exitosamente");
        }

        public Usuario? BuscarUsuarioPorCarne(string carne)
        {
            var indice = _usuarios.FindIndex(u => u.Carne == carne);
            return indice >= 0 ? _usuarios[indice] : null;
        }

        public List<Usuario> ObtenerTodosUsuarios() => _usuarios;

        public int ObtenerCantidadUsuarios() => _usuarios.Count;

        public (bool, string) CambiarEstadoUsuario(string carne, bool activo)
        {
            var indice = _usuarios.FindIndex(u => u.Carne == carne);
            if (indice < 0)
                return (false, "Usuario no encontrado");

            var usuario = _usuarios[indice];
            usuario.Activo = activo;
            _usuarios[indice] = usuario;
            _gestor.GuardarUsuarios(_usuarios);
            return (true, "Estado actualizado");
        }
    }

    public class ServicioPrestamos
    {
        private List<Prestamo> _prestamos;
        private readonly GestorArchivos _gestor;
        private readonly ServicioLibros _servicioLibros;
        private readonly ServicioUsuarios _servicioUsuarios;
        private const int PERIODO_PRESTAMO_DIAS = 14;
        private const int MAXIMO_PRESTAMOS_ACTIVOS = 10;

        public ServicioPrestamos(GestorArchivos gestor, ServicioLibros servicioLibros, ServicioUsuarios servicioUsuarios)
        {
            _gestor = gestor;
            _servicioLibros = servicioLibros;
            _servicioUsuarios = servicioUsuarios;
            _prestamos = _gestor.CargarPrestamos();
        }

        public (bool, string) RegistrarPrestamo(string carne, string codigoLibro)
        {
            // Validar usuario
            var usuario = _servicioUsuarios.BuscarUsuarioPorCarne(carne);
            if (usuario == null)
                return (false, "Usuario no encontrado");

            if (!usuario.Value.Activo)
                return (false, "El usuario está inactivo");

            // Validar cantidad de préstamos activos
            var prestamosActivos = _prestamos.Count(p => p.Carne == carne && p.EstaActivo);
            if (prestamosActivos >= MAXIMO_PRESTAMOS_ACTIVOS)
                return (false, $"El usuario ya tiene {MAXIMO_PRESTAMOS_ACTIVOS} préstamos activos");

            // Validar libro
            var libro = _servicioLibros.BuscarLibroPorCodigo(codigoLibro);
            if (libro == null)
                return (false, "Libro no encontrado");

            if (libro.Value.CantidadEjemplares <= 0)
                return (false, "No hay ejemplares disponibles");

            var hoy = DateTime.Now;
            var prestamo = new Prestamo
            {
                Carne = carne,
                CodigoLibro = codigoLibro,
                FechaPrestamo = hoy,
                FechaEstimadaDevolucion = hoy.AddDays(PERIODO_PRESTAMO_DIAS),
                FechaDevolucion = null
            };

            _prestamos.Add(prestamo);
            _servicioLibros.ActualizarCantidad(codigoLibro, libro.Value.CantidadEjemplares - 1);
            _gestor.GuardarPrestamos(_prestamos);

            return (true, "Préstamo registrado exitosamente");
        }

        public (bool, string) RegistrarDevolucion(string carne, string codigoLibro)
        {
            var indice = _prestamos.FindIndex(p => 
                p.Carne == carne && p.CodigoLibro == codigoLibro && p.EstaActivo);

            if (indice < 0)
                return (false, "Préstamo no encontrado");

            var prestamo = _prestamos[indice];
            prestamo.FechaDevolucion = DateTime.Now;
            _prestamos[indice] = prestamo;

            var libro = _servicioLibros.BuscarLibroPorCodigo(codigoLibro);
            if (libro != null)
                _servicioLibros.ActualizarCantidad(codigoLibro, libro.Value.CantidadEjemplares + 1);

            _gestor.GuardarPrestamos(_prestamos);

            if (prestamo.DiasRetraso > 0)
                return (true, $"Devolución registrada. Retraso: {prestamo.DiasRetraso} días");

            return (true, "Devolución registrada exitosamente");
        }

        public List<Prestamo> ObtenerTodosPrestamos() => _prestamos;

        public List<Prestamo> ObtenerPrestamosActivos()
            => _prestamos.Where(p => p.EstaActivo).ToList();

        public int ObtenerCantidadPrestamosActivos()
            => _prestamos.Count(p => p.EstaActivo);
    }

    // =====================================================================
    // INTERFAZ DE USUARIO
    // =====================================================================

    public class MenuPrincipal
    {
        private readonly ServicioLibros _servicioLibros;
        private readonly ServicioUsuarios _servicioUsuarios;
        private readonly ServicioPrestamos _servicioPrestamos;

        public MenuPrincipal(ServicioLibros servicioLibros, ServicioUsuarios servicioUsuarios, ServicioPrestamos servicioPrestamos)
        {
            _servicioLibros = servicioLibros;
            _servicioUsuarios = servicioUsuarios;
            _servicioPrestamos = servicioPrestamos;
        }

        public void MostrarMenu()
        {
            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════╗");
                Console.WriteLine("║  SISTEMA DE GESTIÓN DE BIBLIOTECA          ║");
                Console.WriteLine("║  Universidad - 2024                        ║");
                Console.WriteLine("╚════════════════════════════════════════════╝");
                Console.WriteLine("\n┌─ MENÚ PRINCIPAL ─────────────────────────┐");
                Console.WriteLine("│ 1. Gestionar Libros                      │");
                Console.WriteLine("│ 2. Gestionar Usuarios                    │");
                Console.WriteLine("│ 3. Gestionar Préstamos                   │");
                Console.WriteLine("│ 4. Salir                                 │");
                Console.WriteLine("└──────────────────────────────────────────┘");
                Console.Write("\n▶ Seleccione opción: ");

                if (int.TryParse(Console.ReadLine(), out int opcion))
                {
                    switch (opcion)
                    {
                        case 1: MenuLibros(); break;
                        case 2: MenuUsuarios(); break;
                        case 3: MenuPrestamos(); break;
                        case 4: salir = true; break;
                        default: MostrarError("Opción inválida");
                            break;
                    }
                }
                else
                {
                    MostrarError("Entrada inválida");
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione Enter para continuar...");
                    Console.ReadLine();
                }
            }

            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║        ¡Gracias por usar el sistema!       ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");
        }

        private void MenuLibros()
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════╗");
                Console.WriteLine("║         GESTIÓN DE LIBROS                  ║");
                Console.WriteLine("╚════════════════════════════════════════════╝");
                Console.WriteLine("\n┌─ OPCIONES ───────────────────────────────┐");
                Console.WriteLine("│ 1. Registrar Libro                       │");
                Console.WriteLine("│ 2. Listar Libros                         │");
                Console.WriteLine("│ 3. Buscar Libro                          │");
                Console.WriteLine("│ 4. Actualizar Cantidad                   │");
                Console.WriteLine("│ 5. Eliminar Libro                        │");
                Console.WriteLine("│ 6. Volver                                │");
                Console.WriteLine("└──────────────────────────────────────────┘");
                Console.Write("\n▶ Seleccione opción: ");

                if (int.TryParse(Console.ReadLine(), out int opcion))
                {
                    switch (opcion)
                    {
                        case 1: RegistrarLibro(); break;
                        case 2: ListarLibros(); break;
                        case 3: BuscarLibro(); break;
                        case 4: ActualizarCantidadLibro(); break;
                        case 5: EliminarLibro(); break;
                        case 6: volver = true; break;
                        default: MostrarError("Opción inválida"); break;
                    }
                }
                else
                {
                    MostrarError("Entrada inválida");
                }

                if (!volver)
                {
                    Console.WriteLine("\nPresione Enter para continuar...");
                    Console.ReadLine();
                }
            }
        }

        private void RegistrarLibro()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║       REGISTRAR NUEVO LIBRO                ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            Console.Write("Código (LIBxxxxx): ");
            var codigo = Console.ReadLine() ?? "";

            Console.Write("Título: ");
            var titulo = Console.ReadLine() ?? "";

            Console.Write("Autor: ");
            var autor = Console.ReadLine() ?? "";

            Console.Write("Editorial: ");
            var editorial = Console.ReadLine() ?? "";

            Console.Write("Año de publicación: ");
            if (!int.TryParse(Console.ReadLine(), out int ano))
            {
                MostrarError("Año inválido");
                return;
            }

            Console.Write("Categoría: ");
            var categoria = Console.ReadLine() ?? "";

            Console.Write("Cantidad de ejemplares: ");
            if (!int.TryParse(Console.ReadLine(), out int cantidad))
            {
                MostrarError("Cantidad inválida");
                return;
            }

            var (exito, mensaje) = _servicioLibros.RegistrarLibro(codigo, titulo, autor, editorial, ano, categoria, cantidad);
            if (exito)
                MostrarExito(mensaje);
            else
                MostrarError(mensaje);
        }

        private void ListarLibros()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║         LISTADO DE LIBROS                  ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            var libros = _servicioLibros.ObtenerTodosLibros();
            if (libros.Count == 0)
            {
                MostrarError("No hay libros registrados");
                return;
            }

            Console.WriteLine("┌─────────┬──────────────────┬────────────────┬──────────┬──────┬───────────┬────────┐");
            Console.WriteLine("│ Código  │ Título           │ Autor          │ Editorial│ Año  │ Categoría │ Ejempl │");
            Console.WriteLine("├─────────┼──────────────────┼────────────────┼──────────┼──────┼───────────┼────────┤");

            foreach (var libro in libros)
            {
                var titulo = libro.Titulo.Length > 16 ? libro.Titulo.Substring(0, 16) : libro.Titulo.PadRight(16);
                var autor = libro.Autor.Length > 14 ? libro.Autor.Substring(0, 14) : libro.Autor.PadRight(14);
                var editorial = libro.Editorial.Length > 8 ? libro.Editorial.Substring(0, 8) : libro.Editorial.PadRight(8);
                var categoria = libro.Categoria.Length > 9 ? libro.Categoria.Substring(0, 9) : libro.Categoria.PadRight(9);
                Console.WriteLine($"│ {libro.Codigo} │ {titulo} │ {autor} │ {editorial} │ {libro.AnoPublicacion} │ {categoria} │ {libro.CantidadEjemplares,6} │");
            }

            Console.WriteLine("└─────────┴──────────────────┴────────────────┴──────────┴──────┴───────────┴────────┘");
        }

        private void BuscarLibro()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║         BUSCAR LIBRO                       ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            Console.Write("Ingrese código del libro (LIBxxxxx): ");
            var codigo = Console.ReadLine() ?? "";

            var libro = _servicioLibros.BuscarLibroPorCodigo(codigo);
            if (libro == null)
            {
                MostrarError("Libro no encontrado");
                return;
            }

            Console.WriteLine("\n┌────────────────────────────────────────┐");
            Console.WriteLine($"│ Código: {libro.Value.Codigo,30}│");
            Console.WriteLine($"│ Título: {libro.Value.Titulo,30}│");
            Console.WriteLine($"│ Autor: {libro.Value.Autor,31}│");
            Console.WriteLine($"│ Editorial: {libro.Value.Editorial,28}│");
            Console.WriteLine($"│ Año: {libro.Value.AnoPublicacion,34}│");
            Console.WriteLine($"│ Categoría: {libro.Value.Categoria,28}│");
            Console.WriteLine($"│ Ejemplares: {libro.Value.CantidadEjemplares,26}│");
            Console.WriteLine("└────────────────────────────────────────┘");
        }

        private void ActualizarCantidadLibro()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║      ACTUALIZAR CANTIDAD DE LIBROS         ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            Console.Write("Código del libro: ");
            var codigo = Console.ReadLine() ?? "";

            Console.Write("Nueva cantidad: ");
            if (!int.TryParse(Console.ReadLine(), out int cantidad))
            {
                MostrarError("Cantidad inválida");
                return;
            }

            var (exito, mensaje) = _servicioLibros.ActualizarCantidad(codigo, cantidad);
            if (exito)
                MostrarExito(mensaje);
            else
                MostrarError(mensaje);
        }

        private void EliminarLibro()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║         ELIMINAR LIBRO                     ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            Console.Write("Código del libro a eliminar: ");
            var codigo = Console.ReadLine() ?? "";

            Console.Write("¿Está seguro? (S/N): ");
            if (Console.ReadLine()?.ToUpper() != "S")
            {
                MostrarError("Operación cancelada");
                return;
            }

            var (exito, mensaje) = _servicioLibros.EliminarLibro(codigo);
            if (exito)
                MostrarExito(mensaje);
            else
                MostrarError(mensaje);
        }

        private void MenuUsuarios()
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════╗");
                Console.WriteLine("║         GESTIÓN DE USUARIOS                ║");
                Console.WriteLine("╚════════════════════════════════════════════╝");
                Console.WriteLine("\n┌─ OPCIONES ───────────────────────────────┐");
                Console.WriteLine("│ 1. Registrar Usuario                     │");
                Console.WriteLine("│ 2. Listar Usuarios                       │");
                Console.WriteLine("│ 3. Buscar Usuario                        │");
                Console.WriteLine("│ 4. Cambiar Estado de Usuario             │");
                Console.WriteLine("│ 5. Volver                                │");
                Console.WriteLine("└──────────────────────────────────────────┘");
                Console.Write("\n▶ Seleccione opción: ");

                if (int.TryParse(Console.ReadLine(), out int opcion))
                {
                    switch (opcion)
                    {
                        case 1: RegistrarUsuario(); break;
                        case 2: ListarUsuarios(); break;
                        case 3: BuscarUsuario(); break;
                        case 4: CambiarEstadoUsuario(); break;
                        case 5: volver = true; break;
                        default: MostrarError("Opción inválida"); break;
                    }
                }
                else
                {
                    MostrarError("Entrada inválida");
                }

                if (!volver)
                {
                    Console.WriteLine("\nPresione Enter para continuar...");
                    Console.ReadLine();
                }
            }
        }

        private void RegistrarUsuario()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║       REGISTRAR NUEVO USUARIO              ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            Console.Write("Carné (8 dígitos): ");
            var carne = Console.ReadLine() ?? "";

            Console.Write("Nombre completo: ");
            var nombre = Console.ReadLine() ?? "";

            Console.Write("Carrera: ");
            var carrera = Console.ReadLine() ?? "";

            Console.Write("Correo electrónico: ");
            var correo = Console.ReadLine() ?? "";

            Console.Write("Teléfono: ");
            var telefono = Console.ReadLine() ?? "";

            var (exito, mensaje) = _servicioUsuarios.RegistrarUsuario(carne, nombre, carrera, correo, telefono);
            if (exito)
                MostrarExito(mensaje);
            else
                MostrarError(mensaje);
        }

        private void ListarUsuarios()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║         LISTADO DE USUARIOS                ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            var usuarios = _servicioUsuarios.ObtenerTodosUsuarios();
            if (usuarios.Count == 0)
            {
                MostrarError("No hay usuarios registrados");
                return;
            }

            Console.WriteLine("┌──────────┬──────────────────┬──────────────┬────────────────────┬───────────┬────────┐");
            Console.WriteLine("│ Carné    │ Nombre           │ Carrera      │ Correo             │ Teléfono  │ Estado │");
            Console.WriteLine("├──────────┼──────────────────┼──────────────┼────────────────────┼───────────┼────────┤");

            foreach (var usuario in usuarios)
            {
                var nombre = usuario.NombreCompleto.Length > 16 ? usuario.NombreCompleto.Substring(0, 16) : usuario.NombreCompleto.PadRight(16);
                var carrera = usuario.Carrera.Length > 12 ? usuario.Carrera.Substring(0, 12) : usuario.Carrera.PadRight(12);
                var correo = usuario.CorreoElectronico.Length > 18 ? usuario.CorreoElectronico.Substring(0, 18) : usuario.CorreoElectronico.PadRight(18);
                var estado = usuario.Activo ? "Activo " : "Inactiv";
                Console.WriteLine($"│ {usuario.Carne} │ {nombre} │ {carrera} │ {correo} │ {usuario.Telefono.PadRight(9)} │ {estado} │");
            }

            Console.WriteLine("└──────────┴──────────────────┴──────────────┴────────────────────┴───────────┴────────┘");
        }

        private void BuscarUsuario()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║         BUSCAR USUARIO                     ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            Console.Write("Ingrese carné del usuario: ");
            var carne = Console.ReadLine() ?? "";

            var usuario = _servicioUsuarios.BuscarUsuarioPorCarne(carne);
            if (usuario == null)
            {
                MostrarError("Usuario no encontrado");
                return;
            }

            Console.WriteLine("\n┌────────────────────────────────────────┐");
            Console.WriteLine($"│ Carné: {usuario.Value.Carne,32}│");
            Console.WriteLine($"│ Nombre: {usuario.Value.NombreCompleto,28}│");
            Console.WriteLine($"│ Carrera: {usuario.Value.Carrera,28}│");
            Console.WriteLine($"│ Correo: {usuario.Value.CorreoElectronico,28}│");
            Console.WriteLine($"│ Teléfono: {usuario.Value.Telefono,26}│");
            Console.WriteLine($"│ Estado: {(usuario.Value.Activo ? "Activo" : "Inactivo"),26}│");
            Console.WriteLine("└────────────────────────────────────────┘");
        }

        private void CambiarEstadoUsuario()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║      CAMBIAR ESTADO DEL USUARIO            ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            Console.Write("Carné del usuario: ");
            var carne = Console.ReadLine() ?? "";

            var usuario = _servicioUsuarios.BuscarUsuarioPorCarne(carne);
            if (usuario == null)
            {
                MostrarError("Usuario no encontrado");
                return;
            }

            Console.WriteLine($"\nEstado actual: {(usuario.Value.Activo ? "Activo" : "Inactivo")}");
            Console.Write("¿Desea cambiar el estado? (S/N): ");
            if (Console.ReadLine()?.ToUpper() != "S")
            {
                MostrarError("Operación cancelada");
                return;
            }

            var (exito, mensaje) = _servicioUsuarios.CambiarEstadoUsuario(carne, !usuario.Value.Activo);
            if (exito)
                MostrarExito(mensaje);
            else
                MostrarError(mensaje);
        }

        private void MenuPrestamos()
        {
            bool volver = false;
            while (!volver)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════╗");
                Console.WriteLine("║         GESTIÓN DE PRÉSTAMOS               ║");
                Console.WriteLine("╚════════════════════════════════════════════╝");
                Console.WriteLine("\n┌─ OPCIONES ───────────────────────────────┐");
                Console.WriteLine("│ 1. Registrar Préstamo                    │");
                Console.WriteLine("│ 2. Registrar Devolución                  │");
                Console.WriteLine("│ 3. Listar Todos los Préstamos            │");
                Console.WriteLine("│ 4. Ver Préstamos Activos                 │");
                Console.WriteLine("│ 5. Ver Préstamos de Usuario              │");
                Console.WriteLine("│ 6. Volver                                │");
                Console.WriteLine("└──────────────────────────────────────────┘");
                Console.Write("\n▶ Seleccione opción: ");

                if (int.TryParse(Console.ReadLine(), out int opcion))
                {
                    switch (opcion)
                    {
                        case 1: RegistrarPrestamo(); break;
                        case 2: RegistrarDevolucion(); break;
                        case 3: ListarTodosPrestamos(); break;
                        case 4: ListarPrestamosActivos(); break;
                        case 5: ListarPrestamosUsuario(); break;
                        case 6: volver = true; break;
                        default: MostrarError("Opción inválida"); break;
                    }
                }
                else
                {
                    MostrarError("Entrada inválida");
                }

                if (!volver)
                {
                    Console.WriteLine("\nPresione Enter para continuar...");
                    Console.ReadLine();
                }
            }
        }

        private void RegistrarPrestamo()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║       REGISTRAR NUEVO PRÉSTAMO             ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            Console.Write("Carné del usuario: ");
            var carne = Console.ReadLine() ?? "";

            Console.Write("Código del libro: ");
            var codigo = Console.ReadLine() ?? "";

            var (exito, mensaje) = _servicioPrestamos.RegistrarPrestamo(carne, codigo);
            if (exito)
                MostrarExito(mensaje);
            else
                MostrarError(mensaje);
        }

        private void RegistrarDevolucion()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║         REGISTRAR DEVOLUCIÓN               ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            Console.Write("Carné del usuario: ");
            var carne = Console.ReadLine() ?? "";

            Console.Write("Código del libro: ");
            var codigo = Console.ReadLine() ?? "";

            var (exito, mensaje) = _servicioPrestamos.RegistrarDevolucion(carne, codigo);
            if (exito)
                MostrarExito(mensaje);
            else
                MostrarError(mensaje);
        }

        private void ListarTodosPrestamos()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║      LISTADO DE TODOS LOS PRÉSTAMOS        ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            var prestamos = _servicioPrestamos.ObtenerTodosPrestamos();
            if (prestamos.Count == 0)
            {
                MostrarError("No hay préstamos registrados");
                return;
            }

            Console.WriteLine("┌──────────┬──────────┬──────────────┬──────────────┬──────────────┬──────────┐");
            Console.WriteLine("│ Carné    │ Código   │ F. Préstamo  │ F. Devolución│ F. Devuelto  │ Estado   │");
            Console.WriteLine("├──────────┼──────────┼──────────────┼──────────────┼──────────────┼──────────┤");

            foreach (var prestamo in prestamos)
            {
                var estado = prestamo.EstaActivo ? "Activo    " : $"Devuelto ({prestamo.DiasRetraso}d)";
                var devuelto = prestamo.FechaDevolucion?.ToString("dd/MM/yyyy") ?? "Pendiente    ";
                Console.WriteLine($"│ {prestamo.Carne} │ {prestamo.CodigoLibro} │ {prestamo.FechaPrestamo:dd/MM/yyyy} │ {prestamo.FechaEstimadaDevolucion:dd/MM/yyyy} │ {devuelto} │ {estado} │");
            }

            Console.WriteLine("└──────────┴──────────┴──────────────┴──────────────┴──────────────┴──────────┘");
        }

        private void ListarPrestamosActivos()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║       PRÉSTAMOS ACTIVOS                    ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            var prestamos = _servicioPrestamos.ObtenerPrestamosActivos();
            if (prestamos.Count == 0)
            {
                MostrarError("No hay préstamos activos");
                return;
            }

            Console.WriteLine($"Total de préstamos activos: {prestamos.Count}\n");
            Console.WriteLine("┌──────────┬──────────┬──────────────┬──────────────┐");
            Console.WriteLine("│ Carné    │ Código   │ F. Préstamo  │ F. Devolución│");
            Console.WriteLine("├──────────┼──────────┼──────────────┼──────────────┤");

            foreach (var prestamo in prestamos)
            {
                Console.WriteLine($"│ {prestamo.Carne} │ {prestamo.CodigoLibro} │ {prestamo.FechaPrestamo:dd/MM/yyyy} │ {prestamo.FechaEstimadaDevolucion:dd/MM/yyyy} │");
            }

            Console.WriteLine("└──────────┴──────────┴──────────────┴──────────────┘");
        }

        private void ListarPrestamosUsuario()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║     PRÉSTAMOS DE UN USUARIO                ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            Console.Write("Ingrese carné del usuario: ");
            var carne = Console.ReadLine() ?? "";

            var prestamos = _servicioPrestamos.ObtenerTodosPrestamos()
                .Where(p => p.Carne == carne).ToList();

            if (prestamos.Count == 0)
            {
                MostrarError("No hay préstamos para este usuario");
                return;
            }

            Console.WriteLine($"\nPréstamos del usuario {carne}:\n");
            Console.WriteLine("┌──────────┬──────────────┬──────────────┬──────────────┬──────────┐");
            Console.WriteLine("│ Código   │ F. Préstamo  │ F. Devolución│ F. Devuelto  │ Estado   │");
            Console.WriteLine("├──────────┼──────────────┼──────────────┼──────────────┼──────────┤");

            foreach (var prestamo in prestamos)
            {
                var estado = prestamo.EstaActivo ? "Activo" : "Devuelto";
                var devuelto = prestamo.FechaDevolucion?.ToString("dd/MM/yyyy") ?? "Pendiente    ";
                Console.WriteLine($"│ {prestamo.CodigoLibro} │ {prestamo.FechaPrestamo:dd/MM/yyyy} │ {prestamo.FechaEstimadaDevolucion:dd/MM/yyyy} │ {devuelto} │ {estado,8} │");
            }

            Console.WriteLine("└──────────┴──────────────┴──────────────┴──────────────┴──────────┘");
        }

        private void MostrarExito(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✓ {mensaje}");
            Console.ResetColor();
        }

        private void MostrarError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n✗ {mensaje}");
            Console.ResetColor();
        }
    }

    // =====================================================================
    // PUNTO DE ENTRADA
    // =====================================================================

    class Program
    {
        static void Main(string[] args)
        {
            // Inicializar servicios
            var gestor = new GestorArchivos();
            var servicioLibros = new ServicioLibros(gestor);
            var servicioUsuarios = new ServicioUsuarios(gestor);
            var servicioPrestamos = new ServicioPrestamos(gestor, servicioLibros, servicioUsuarios);

            // Mostrar menú
            var menu = new MenuPrincipal(servicioLibros, servicioUsuarios, servicioPrestamos);
            menu.MostrarMenu();
        }
    }
}
