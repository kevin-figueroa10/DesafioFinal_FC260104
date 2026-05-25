namespace Proyecto_final.Models;

/// <summary>
/// Representa un libro en el sistema de gestión de biblioteca.
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

    public Libro()
    {
        Codigo = string.Empty;
        Titulo = string.Empty;
        Autor = string.Empty;
        Editorial = string.Empty;
        AnoPublicacion = 0;
        Categoria = string.Empty;
        CantidadEjemplares = 0;
    }

    public override string ToString()
    {
        return $"Código: {Codigo} | Título: {Titulo} | Autor: {Autor} | Editorial: {Editorial} | Año: {AnoPublicacion} | Categoría: {Categoria} | Cantidad: {CantidadEjemplares}";
    }
}
