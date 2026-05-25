using Proyecto_final.Data;
using Proyecto_final.Models;
using Proyecto_final.Utilidades;

namespace Proyecto_final.Services;

/// <summary>
/// Servicio para gestionar operaciones de libros (Módulo A).
/// </summary>
public class LibroService
{
    private List<Libro> libros;
    private const int MaximoLibros = 10;

    public LibroService()
    {
        libros = FileManager.CargarLibros();
    }

    /// <summary>
    /// Obtiene la cantidad actual de libros registrados.
    /// </summary>
    public int ObtenerCantidadLibros() => libros.Count;

    /// <summary>
    /// Registra un nuevo libro en el sistema.
    /// </summary>
    public (bool exito, string mensaje) RegistrarLibro(string codigo, string titulo, string autor, 
                                                        string editorial, int ano, string categoria, int cantidad)
    {
        // Validar límite
        if (libros.Count >= MaximoLibros)
            return (false, $"No se pueden registrar más libros. Límite alcanzado: {MaximoLibros}");

        // Validar código
        if (!ValidatorHelper.ValidarCodigoLibro(codigo))
            return (false, "El código debe tener exactamente 8 caracteres alfanuméricos (ej: LIB00001).");

        // Verificar duplicado
        if (libros.Any(l => l.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase)))
            return (false, $"El código '{codigo}' ya existe en el sistema.");

        // Validar año
        if (!ValidatorHelper.ValidarAnio(ano))
            return (false, $"El año debe estar entre 1900 y {DateTime.Now.Year}.");

        // Validar cantidad
        if (!ValidatorHelper.ValidarCantidad(cantidad))
            return (false, "La cantidad debe ser un valor no negativo.");

        // Validar cadenas
        if (!ValidatorHelper.ValidarCadenaNoVacia(titulo) ||
            !ValidatorHelper.ValidarCadenaNoVacia(autor) ||
            !ValidatorHelper.ValidarCadenaNoVacia(editorial) ||
            !ValidatorHelper.ValidarCadenaNoVacia(categoria))
            return (false, "Todos los campos de texto deben tener contenido válido.");

        var libro = new Libro
        {
            Codigo = codigo,
            Titulo = titulo,
            Autor = autor,
            Editorial = editorial,
            AnoPublicacion = ano,
            Categoria = categoria,
            CantidadEjemplares = cantidad
        };

        libros.Add(libro);
        GuardarCambios();
        return (true, $"Libro '{titulo}' registrado exitosamente.");
    }

    /// <summary>
    /// Busca un libro por código.
    /// </summary>
    public Libro? BuscarLibroPorCodigo(string codigo)
    {
        int indice = libros.FindIndex(l => l.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));
        return indice >= 0 ? libros[indice] : null;
    }

    /// <summary>
    /// Obtiene todos los libros registrados.
    /// </summary>
    public List<Libro> ObtenerTodosLibros() => new(libros);

    /// <summary>
    /// Actualiza la cantidad de ejemplares de un libro.
    /// </summary>
    public (bool exito, string mensaje) ActualizarCantidad(string codigo, int nuevaCantidad)
    {
        if (!ValidatorHelper.ValidarCantidad(nuevaCantidad))
            return (false, "La cantidad debe ser un valor no negativo.");

        var libro = BuscarLibroPorCodigo(codigo);
        if (libro == null)
            return (false, $"No se encontró un libro con el código '{codigo}'.");

        int indice = libros.FindIndex(l => l.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));
        if (indice >= 0)
        {
            libros[indice] = new Libro
            {
                Codigo = libro.Value.Codigo,
                Titulo = libro.Value.Titulo,
                Autor = libro.Value.Autor,
                Editorial = libro.Value.Editorial,
                AnoPublicacion = libro.Value.AnoPublicacion,
                Categoria = libro.Value.Categoria,
                CantidadEjemplares = nuevaCantidad
            };
            GuardarCambios();
            return (true, $"Cantidad actualizada para '{libro.Value.Titulo}'.");
        }

        return (false, "Error al actualizar el libro.");
    }

    /// <summary>
    /// Elimina un libro del sistema.
    /// </summary>
    public (bool exito, string mensaje) EliminarLibro(string codigo)
    {
        var libro = BuscarLibroPorCodigo(codigo);
        if (libro == null)
            return (false, $"No se encontró un libro con el código '{codigo}'.");

        libros.Remove(libro.Value);
        GuardarCambios();
        return (true, $"Libro '{libro.Value.Titulo}' eliminado exitosamente.");
    }

    private void GuardarCambios()
    {
        FileManager.GuardarLibros(libros);
    }
}
