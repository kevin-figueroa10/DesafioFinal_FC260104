using Proyecto_final.Data;
using Proyecto_final.Models;
using Proyecto_final.Utilidades;

namespace Proyecto_final.Services;

/// <summary>
/// Servicio para gestionar operaciones de préstamos (Módulo C).
/// </summary>
public class PrestamoService
{
    private List<Prestamo> prestamos;
    private readonly LibroService libroService;
    private readonly UsuarioService usuarioService;
    private const int MaximoPrestamos = 10;
    private const int DiasDevolucion = 14;

    public PrestamoService(LibroService libroService, UsuarioService usuarioService)
    {
        prestamos = FileManager.CargarPrestamos();
        this.libroService = libroService;
        this.usuarioService = usuarioService;
    }

    /// <summary>
    /// Obtiene la cantidad actual de préstamos activos.
    /// </summary>
    public int ObtenerCantidadPrestamosActivos() => prestamos.Count(p => p.EstaActivo);

    /// <summary>
    /// Registra un nuevo préstamo vinculando usuario y libro.
    /// </summary>
    public (bool exito, string mensaje) RegistrarPrestamo(string carne, string codigoLibro)
    {
        // Validar límite de préstamos activos
        if (ObtenerCantidadPrestamosActivos() >= MaximoPrestamos)
            return (false, $"Límite de préstamos activos alcanzado: {MaximoPrestamos}");

        // Validar usuario
        var usuario = usuarioService.BuscarUsuarioPorCarne(carne);
        if (usuario == null)
            return (false, $"No existe usuario con carné '{carne}'.");

        if (!usuario.Value.Activo)
            return (false, $"El usuario con carné '{carne}' está inactivo y no puede realizar préstamos.");

        // Validar libro
        var libro = libroService.BuscarLibroPorCodigo(codigoLibro);
        if (libro == null)
            return (false, $"No existe libro con código '{codigoLibro}'.");

        if (libro.Value.CantidadEjemplares <= 0)
            return (false, $"El libro '{libro.Value.Titulo}' no tiene ejemplares disponibles.");

        var prestamo = new Prestamo
        {
            Carne = carne,
            CodigoLibro = codigoLibro,
            FechaPrestamo = DateTime.Now,
            FechaEstimadaDevolucion = DateTime.Now.AddDays(DiasDevolucion),
            FechaDevolucion = null
        };

        prestamos.Add(prestamo);

        // Actualizar cantidad de ejemplares
        libroService.ActualizarCantidad(codigoLibro, libro.Value.CantidadEjemplares - 1);

        GuardarCambios();
        return (true, $"Préstamo registrado exitosamente. Devolución estimada: {prestamo.FechaEstimadaDevolucion:dd/MM/yyyy}");
    }

    /// <summary>
    /// Registra la devolución de un libro.
    /// </summary>
    public (bool exito, string mensaje) RegistrarDevolucion(string carne, string codigoLibro)
    {
        int indice = prestamos.FindIndex(p => 
            p.Carne.Equals(carne, StringComparison.OrdinalIgnoreCase) &&
            p.CodigoLibro.Equals(codigoLibro, StringComparison.OrdinalIgnoreCase) &&
            p.EstaActivo);

        if (indice < 0)
            return (false, $"No existe préstamo activo para el usuario '{carne}' y libro '{codigoLibro}'.");

        var prestamo = prestamos[indice];
        prestamos[indice] = new Prestamo
        {
            Carne = prestamo.Carne,
            CodigoLibro = prestamo.CodigoLibro,
            FechaPrestamo = prestamo.FechaPrestamo,
            FechaEstimadaDevolucion = prestamo.FechaEstimadaDevolucion,
            FechaDevolucion = DateTime.Now
        };

        // Actualizar cantidad de ejemplares
        var libro = libroService.BuscarLibroPorCodigo(codigoLibro);
        if (libro != null)
        {
            libroService.ActualizarCantidad(codigoLibro, libro.Value.CantidadEjemplares + 1);
        }

        GuardarCambios();

        // Verificar retraso
        bool hayRetraso = DateTime.Now > prestamo.FechaEstimadaDevolucion;
        string mensaje = "Devolución registrada exitosamente.";
        if (hayRetraso)
        {
            TimeSpan atraso = DateTime.Now - prestamo.FechaEstimadaDevolucion;
            mensaje += $" (Se devolvió {atraso.Days} días después de la fecha estimada)";
        }

        return (true, mensaje);
    }

    /// <summary>
    /// Obtiene todos los préstamos de un usuario.
    /// </summary>
    public List<Prestamo> ObtenerPrestamosUsuario(string carne)
    {
        return prestamos.Where(p => p.Carne.Equals(carne, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    /// <summary>
    /// Obtiene todos los préstamos activos de un usuario.
    /// </summary>
    public List<Prestamo> ObtenerPrestamosActivosUsuario(string carne)
    {
        return prestamos.Where(p => 
            p.Carne.Equals(carne, StringComparison.OrdinalIgnoreCase) && 
            p.EstaActivo).ToList();
    }

    /// <summary>
    /// Obtiene todos los préstamos registrados.
    /// </summary>
    public List<Prestamo> ObtenerTodosPrestamos() => new(prestamos);

    /// <summary>
    /// Obtiene todos los préstamos activos del sistema.
    /// </summary>
    public List<Prestamo> ObtenerPrestamosActivos()
    {
        return prestamos.Where(p => p.EstaActivo).ToList();
    }

    private void GuardarCambios()
    {
        FileManager.GuardarPrestamos(prestamos);
    }
}
