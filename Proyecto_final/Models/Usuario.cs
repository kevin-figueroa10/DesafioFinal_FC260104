namespace Proyecto_final.Models;

/// <summary>
/// Representa un usuario en el sistema de gestión de biblioteca.
/// </summary>
public struct Usuario
{
    public string Carne { get; set; }
    public string NombreCompleto { get; set; }
    public string Carrera { get; set; }
    public string CorreoElectronico { get; set; }
    public string Telefono { get; set; }
    public bool Activo { get; set; }

    public Usuario()
    {
        Carne = string.Empty;
        NombreCompleto = string.Empty;
        Carrera = string.Empty;
        CorreoElectronico = string.Empty;
        Telefono = string.Empty;
        Activo = true;
    }

    public override string ToString()
    {
        string estado = Activo ? "Activo" : "Inactivo";
        return $"Carné: {Carne} | Nombre: {NombreCompleto} | Carrera: {Carrera} | Correo: {CorreoElectronico} | Teléfono: {Telefono} | Estado: {estado}";
    }
}
