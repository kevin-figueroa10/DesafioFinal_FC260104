using Proyecto_final.Data;
using Proyecto_final.Models;
using Proyecto_final.Utilidades;

namespace Proyecto_final.Services;

/// <summary>
/// Servicio para gestionar operaciones de usuarios (Módulo B).
/// </summary>
public class UsuarioService
{
    private List<Usuario> usuarios;
    private const int MaximoUsuarios = 5;

    public UsuarioService()
    {
        usuarios = FileManager.CargarUsuarios();
    }

    /// <summary>
    /// Obtiene la cantidad actual de usuarios registrados.
    /// </summary>
    public int ObtenerCantidadUsuarios() => usuarios.Count;

    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    /// </summary>
    public (bool exito, string mensaje) RegistrarUsuario(string carne, string nombre, string carrera, 
                                                          string correo, string telefono)
    {
        // Validar límite
        if (usuarios.Count >= MaximoUsuarios)
            return (false, $"No se pueden registrar más usuarios. Límite alcanzado: {MaximoUsuarios}");

        // Validar carné
        if (!ValidatorHelper.ValidarCarne(carne))
            return (false, "El carné debe tener exactamente 8 dígitos numéricos (ej: 12345678).");

        // Verificar duplicado
        if (usuarios.Any(u => u.Carne.Equals(carne, StringComparison.OrdinalIgnoreCase)))
            return (false, $"El carné '{carne}' ya existe en el sistema.");

        // Validar correo
        if (!ValidatorHelper.ValidarCorreoElectronico(correo))
            return (false, "El correo debe contener un '@' y un punto posterior (ej: usuario@dominio.com).");

        // Validar cadenas
        if (!ValidatorHelper.ValidarCadenaNoVacia(nombre) ||
            !ValidatorHelper.ValidarCadenaNoVacia(carrera) ||
            !ValidatorHelper.ValidarCadenaNoVacia(telefono))
            return (false, "Todos los campos deben tener contenido válido.");

        var usuario = new Usuario
        {
            Carne = carne,
            NombreCompleto = nombre,
            Carrera = carrera,
            CorreoElectronico = correo,
            Telefono = telefono,
            Activo = true
        };

        usuarios.Add(usuario);
        GuardarCambios();
        return (true, $"Usuario '{nombre}' registrado exitosamente.");
    }

    /// <summary>
    /// Busca un usuario por carné.
    /// </summary>
    public Usuario? BuscarUsuarioPorCarne(string carne)
    {
        int indice = usuarios.FindIndex(u => u.Carne.Equals(carne, StringComparison.OrdinalIgnoreCase));
        return indice >= 0 ? usuarios[indice] : null;
    }

    /// <summary>
    /// Busca usuarios por nombre.
    /// </summary>
    public List<Usuario> BuscarUsuariosPorNombre(string nombre)
    {
        return usuarios.Where(u => u.NombreCompleto.Contains(nombre, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    /// <summary>
    /// Obtiene todos los usuarios registrados.
    /// </summary>
    public List<Usuario> ObtenerTodosUsuarios() => new(usuarios);

    /// <summary>
    /// Cambia el estado de un usuario (activo/inactivo).
    /// </summary>
    public (bool exito, string mensaje) CambiarEstadoUsuario(string carne, bool activo)
    {
        var usuario = BuscarUsuarioPorCarne(carne);
        if (usuario == null)
            return (false, $"No se encontró usuario con carné '{carne}'.");

        int indice = usuarios.FindIndex(u => u.Carne.Equals(carne, StringComparison.OrdinalIgnoreCase));
        if (indice >= 0)
        {
            usuarios[indice] = new Usuario
            {
                Carne = usuario.Value.Carne,
                NombreCompleto = usuario.Value.NombreCompleto,
                Carrera = usuario.Value.Carrera,
                CorreoElectronico = usuario.Value.CorreoElectronico,
                Telefono = usuario.Value.Telefono,
                Activo = activo
            };
            GuardarCambios();
            string estado = activo ? "Activado" : "Desactivado";
            return (true, $"Usuario {estado} exitosamente.");
        }

        return (false, "Error al cambiar el estado del usuario.");
    }

    private void GuardarCambios()
    {
        FileManager.GuardarUsuarios(usuarios);
    }
}
