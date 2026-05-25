using System.Text.RegularExpressions;

namespace Proyecto_final.Utilidades;

/// <summary>
/// Utilidad para validar datos según los requisitos del sistema.
/// </summary>
public static class ValidatorHelper
{
    /// <summary>
    /// Valida que el código de libro sea exactamente 8 caracteres alfanuméricos.
    /// </summary>
    public static bool ValidarCodigoLibro(string codigo)
    {
        return !string.IsNullOrWhiteSpace(codigo) && 
               codigo.Length == 8 && 
               Regex.IsMatch(codigo, @"^[A-Za-z0-9]+$");
    }

    /// <summary>
    /// Valida que el carné sea exactamente 8 dígitos numéricos.
    /// </summary>
    public static bool ValidarCarne(string carne)
    {
        return !string.IsNullOrWhiteSpace(carne) && 
               carne.Length == 8 && 
               Regex.IsMatch(carne, @"^\d+$");
    }

    /// <summary>
    /// Valida que el correo electrónico contenga el carácter '@' y un punto posterior.
    /// </summary>
    public static bool ValidarCorreoElectronico(string correo)
    {
        return !string.IsNullOrWhiteSpace(correo) && 
               Regex.IsMatch(correo, @"^[^@]+@[^@]+\.[^@]+$");
    }

    /// <summary>
    /// Valida que el año sea entre 1900 y el año actual.
    /// </summary>
    public static bool ValidarAnio(int anio)
    {
        int anioActual = DateTime.Now.Year;
        return anio >= 1900 && anio <= anioActual;
    }

    /// <summary>
    /// Valida que la cantidad sea un valor no negativo.
    /// </summary>
    public static bool ValidarCantidad(int cantidad)
    {
        return cantidad >= 0;
    }

    /// <summary>
    /// Valida que la cadena no esté vacía ni contenga solo espacios en blanco.
    /// </summary>
    public static bool ValidarCadenaNoVacia(string? cadena)
    {
        return !string.IsNullOrWhiteSpace(cadena);
    }

    /// <summary>
    /// Valida que una fecha tenga formato dd/MM/yyyy.
    /// </summary>
    public static bool ValidarFormatoFecha(string fecha)
    {
        return DateTime.TryParseExact(fecha, "dd/MM/yyyy", null, 
            System.Globalization.DateTimeStyles.None, out _);
    }

    /// <summary>
    /// Convierte una cadena a DateTime si tiene formato válido.
    /// </summary>
    public static DateTime? ConvertirFecha(string fecha)
    {
        if (DateTime.TryParseExact(fecha, "dd/MM/yyyy", null, 
            System.Globalization.DateTimeStyles.None, out var resultado))
        {
            return resultado;
        }
        return null;
    }

    /// <summary>
    /// Intenta convertir una cadena a entero.
    /// </summary>
    public static bool TryParseEntero(string? valor, out int resultado)
    {
        return int.TryParse(valor, out resultado);
    }
}
