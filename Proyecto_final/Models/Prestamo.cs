namespace Proyecto_final.Models;

/// <summary>
/// Representa un préstamo de libro en el sistema.
/// </summary>
public struct Prestamo
{
    public string Carne { get; set; }
    public string CodigoLibro { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public DateTime FechaEstimadaDevolucion { get; set; }
    public DateTime? FechaDevolucion { get; set; }

    public Prestamo()
    {
        Carne = string.Empty;
        CodigoLibro = string.Empty;
        FechaPrestamo = DateTime.Now;
        FechaEstimadaDevolucion = DateTime.Now.AddDays(14);
        FechaDevolucion = null;
    }

    public bool EstaActivo => FechaDevolucion == null;

    public override string ToString()
    {
        string estado = EstaActivo ? "Activo" : "Devuelto";
        string devolucion = FechaDevolucion.HasValue ? FechaDevolucion.Value.ToString("dd/MM/yyyy") : "Pendiente";
        return $"Carné: {Carne} | Código Libro: {CodigoLibro} | Préstamo: {FechaPrestamo:dd/MM/yyyy} | Estimada: {FechaEstimadaDevolucion:dd/MM/yyyy} | Devolución: {devolucion} | Estado: {estado}";
    }
}
