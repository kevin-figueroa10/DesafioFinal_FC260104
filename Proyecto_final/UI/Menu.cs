using Proyecto_final.Services;
using Proyecto_final.Utilidades;

namespace Proyecto_final.UI;

/// <summary>
/// Gestiona la interfaz de menús del sistema.
/// </summary>
public class Menu
{
    private readonly LibroService libroService;
    private readonly UsuarioService usuarioService;
    private readonly PrestamoService prestamoService;

    public Menu(LibroService libroService, UsuarioService usuarioService, PrestamoService prestamoService)
    {
        this.libroService = libroService;
        this.usuarioService = usuarioService;
        this.prestamoService = prestamoService;
    }

    /// <summary>
    /// Muestra el menú principal del sistema.
    /// </summary>
    public void MostrarMenuPrincipal()
    {
        bool salir = false;

        while (!salir)
        {
            LimpiarPantalla();
            MostrarEncabezado("SISTEMA INTEGRAL DE GESTIÓN DE BIBLIOTECA UNIVERSITARIA");
            Console.WriteLine("\n╔═══════════════════════════════════════════════════════╗");
            Console.WriteLine("║                   MENÚ PRINCIPAL                      ║");
            Console.WriteLine("╠═══════════════════════════════════════════════════════╣");
            Console.WriteLine("║  1. Gestionar Libros                                  ║");
            Console.WriteLine("║  2. Gestionar Usuarios                                ║");
            Console.WriteLine("║  3. Gestionar Préstamos                               ║");
            Console.WriteLine("║  4. Salir                                             ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════╝\n");

            Console.Write("Seleccione una opción (1-4): ");
            string? opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    MenuGestionLibros();
                    break;
                case "2":
                    MenuGestionUsuarios();
                    break;
                case "3":
                    MenuGestionPrestamos();
                    break;
                case "4":
                    Console.WriteLine("\n¡Gracias por usar el sistema! Hasta pronto.");
                    salir = true;
                    break;
                default:
                    MostrarError("Opción inválida. Por favor, intente de nuevo.");
                    PausarPantalla();
                    break;
            }
        }
    }

    #region Menú Gestión de Libros

    private void MenuGestionLibros()
    {
        bool volver = false;

        while (!volver)
        {
            LimpiarPantalla();
            MostrarEncabezado("GESTIÓN DE LIBROS");
            Console.WriteLine($"Libros registrados: {libroService.ObtenerCantidadLibros()}/10\n");
            Console.WriteLine("╔═══════════════════════════════════════════════════════╗");
            Console.WriteLine("║  1. Registrar Libro                                   ║");
            Console.WriteLine("║  2. Buscar Libro                                      ║");
            Console.WriteLine("║  3. Listar Todos los Libros                           ║");
            Console.WriteLine("║  4. Eliminar Libro                                    ║");
            Console.WriteLine("║  5. Actualizar Cantidad de Ejemplares                 ║");
            Console.WriteLine("║  6. Volver                                            ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════╝\n");

            Console.Write("Seleccione una opción (1-6): ");
            string? opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    RegistrarLibro();
                    break;
                case "2":
                    BuscarLibro();
                    break;
                case "3":
                    ListarLibros();
                    break;
                case "4":
                    EliminarLibro();
                    break;
                case "5":
                    ActualizarCantidadLibro();
                    break;
                case "6":
                    volver = true;
                    break;
                default:
                    MostrarError("Opción inválida. Por favor, intente de nuevo.");
                    PausarPantalla();
                    break;
            }
        }
    }

    private void RegistrarLibro()
    {
        LimpiarPantalla();
        MostrarEncabezado("REGISTRAR NUEVO LIBRO");

        Console.Write("Código de libro (8 caracteres alfanuméricos, ej: LIB00001): ");
        string? codigo = Console.ReadLine();

        Console.Write("Título del libro: ");
        string? titulo = Console.ReadLine();

        Console.Write("Autor: ");
        string? autor = Console.ReadLine();

        Console.Write("Editorial: ");
        string? editorial = Console.ReadLine();

        Console.Write("Año de publicación: ");
        if (!ValidatorHelper.TryParseEntero(Console.ReadLine(), out int ano))
        {
            MostrarError("Año inválido.");
            PausarPantalla();
            return;
        }

        Console.Write("Categoría: ");
        string? categoria = Console.ReadLine();

        Console.Write("Cantidad de ejemplares: ");
        if (!ValidatorHelper.TryParseEntero(Console.ReadLine(), out int cantidad))
        {
            MostrarError("Cantidad inválida.");
            PausarPantalla();
            return;
        }

        var (exito, mensaje) = libroService.RegistrarLibro(codigo ?? "", titulo ?? "", autor ?? "", 
                                                            editorial ?? "", ano, categoria ?? "", cantidad);

        if (exito)
            MostrarExito(mensaje);
        else
            MostrarError(mensaje);

        PausarPantalla();
    }

    private void BuscarLibro()
    {
        LimpiarPantalla();
        MostrarEncabezado("BUSCAR LIBRO POR CÓDIGO");

        Console.Write("Ingrese el código de libro: ");
        string? codigo = Console.ReadLine();

        var libro = libroService.BuscarLibroPorCodigo(codigo ?? "");
        if (libro != null)
        {
            Console.WriteLine("\n╔═══════════════════════════════════════════════════════╗");
            Console.WriteLine($"║ {libro.Value}");
            Console.WriteLine("╚═══════════════════════════════════════════════════════╝");
        }
        else
            MostrarError("Libro no encontrado.");

        PausarPantalla();
    }

    private void ListarLibros()
    {
        LimpiarPantalla();
        MostrarEncabezado("LISTA DE TODOS LOS LIBROS");

        var libros = libroService.ObtenerTodosLibros();
        if (libros.Count == 0)
        {
            MostrarError("No hay libros registrados.");
        }
        else
        {
            Console.WriteLine($"\nTotal de libros: {libros.Count}\n");

            // Encabezados
            Console.WriteLine("┌──────────┬──────────────────────────┬──────────────────┬────────────────┬──────┬─────────────────┬──────────┐");
            Console.WriteLine("│ Código   │ Título                   │ Autor            │ Editorial      │ Año  │ Categoría       │ Ejemplar │");
            Console.WriteLine("├──────────┼──────────────────────────┼──────────────────┼────────────────┼──────┼─────────────────┼──────────┤");

            foreach (var libro in libros)
            {
                string codigo = libro.Codigo.Length > 8 ? libro.Codigo.Substring(0, 8) : libro.Codigo.PadRight(8);
                string titulo = libro.Titulo.Length > 24 ? libro.Titulo.Substring(0, 24) : libro.Titulo.PadRight(24);
                string autor = libro.Autor.Length > 16 ? libro.Autor.Substring(0, 16) : libro.Autor.PadRight(16);
                string editorial = libro.Editorial.Length > 14 ? libro.Editorial.Substring(0, 14) : libro.Editorial.PadRight(14);
                string categoria = libro.Categoria.Length > 15 ? libro.Categoria.Substring(0, 15) : libro.Categoria.PadRight(15);

                Console.WriteLine($"│ {codigo} │ {titulo} │ {autor} │ {editorial} │ {libro.AnoPublicacion:D4} │ {categoria} │ {libro.CantidadEjemplares,8} │");
            }

            Console.WriteLine("└──────────┴──────────────────────────┴──────────────────┴────────────────┴──────┴─────────────────┴──────────┘");
        }

        PausarPantalla();
    }

    private void EliminarLibro()
    {
        LimpiarPantalla();
        MostrarEncabezado("ELIMINAR UN LIBRO");

        Console.Write("Ingrese el código de libro a eliminar: ");
        string? codigo = Console.ReadLine();

        var (exito, mensaje) = libroService.EliminarLibro(codigo ?? "");
        if (exito)
            MostrarExito(mensaje);
        else
            MostrarError(mensaje);

        PausarPantalla();
    }

    private void ActualizarCantidadLibro()
    {
        LimpiarPantalla();
        MostrarEncabezado("ACTUALIZAR CANTIDAD DE EJEMPLARES");

        Console.Write("Ingrese el código de libro: ");
        string? codigo = Console.ReadLine();

        Console.Write("Nueva cantidad: ");
        if (!ValidatorHelper.TryParseEntero(Console.ReadLine(), out int nuevaCantidad))
        {
            MostrarError("Cantidad inválida.");
            PausarPantalla();
            return;
        }

        var (exito, mensaje) = libroService.ActualizarCantidad(codigo ?? "", nuevaCantidad);
        if (exito)
            MostrarExito(mensaje);
        else
            MostrarError(mensaje);

        PausarPantalla();
    }

    #endregion

    #region Menú Gestión de Usuarios

    private void MenuGestionUsuarios()
    {
        bool volver = false;

        while (!volver)
        {
            LimpiarPantalla();
            MostrarEncabezado("GESTIÓN DE USUARIOS");
            Console.WriteLine($"Usuarios registrados: {usuarioService.ObtenerCantidadUsuarios()}/5\n");
            Console.WriteLine("╔═══════════════════════════════════════════════════════╗");
            Console.WriteLine("║  1. Registrar Usuario                                 ║");
            Console.WriteLine("║  2. Buscar por Carné                                  ║");
            Console.WriteLine("║  3. Buscar por Nombre                                 ║");
            Console.WriteLine("║  4. Listar Todos los Usuarios                         ║");
            Console.WriteLine("║  5. Cambiar Estado de Usuario                         ║");
            Console.WriteLine("║  6. Volver                                            ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════╝\n");

            Console.Write("Seleccione una opción (1-6): ");
            string? opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    RegistrarUsuario();
                    break;
                case "2":
                    BuscarUsuarioPorCarne();
                    break;
                case "3":
                    BuscarUsuarioPorNombre();
                    break;
                case "4":
                    ListarUsuarios();
                    break;
                case "5":
                    CambiarEstadoUsuario();
                    break;
                case "6":
                    volver = true;
                    break;
                default:
                    MostrarError("Opción inválida. Por favor, intente de nuevo.");
                    PausarPantalla();
                    break;
            }
        }
    }

    private void RegistrarUsuario()
    {
        LimpiarPantalla();
        MostrarEncabezado("REGISTRAR NUEVO USUARIO");

        Console.Write("Carné (8 dígitos numéricos, ej: 12345678): ");
        string? carne = Console.ReadLine();

        Console.Write("Nombre completo: ");
        string? nombre = Console.ReadLine();

        Console.Write("Carrera: ");
        string? carrera = Console.ReadLine();

        Console.Write("Correo electrónico (ej: usuario@dominio.com): ");
        string? correo = Console.ReadLine();

        Console.Write("Teléfono: ");
        string? telefono = Console.ReadLine();

        var (exito, mensaje) = usuarioService.RegistrarUsuario(carne ?? "", nombre ?? "", carrera ?? "", 
                                                                correo ?? "", telefono ?? "");

        if (exito)
            MostrarExito(mensaje);
        else
            MostrarError(mensaje);

        PausarPantalla();
    }

    private void BuscarUsuarioPorCarne()
    {
        LimpiarPantalla();
        MostrarEncabezado("BUSCAR USUARIO POR CARNÉ");

        Console.Write("Ingrese el carné del usuario: ");
        string? carne = Console.ReadLine();

        var usuario = usuarioService.BuscarUsuarioPorCarne(carne ?? "");
        if (usuario != null)
        {
            Console.WriteLine("\n╔═══════════════════════════════════════════════════════╗");
            Console.WriteLine($"║ {usuario.Value}");
            Console.WriteLine("╚═══════════════════════════════════════════════════════╝");
        }
        else
            MostrarError("Usuario no encontrado.");

        PausarPantalla();
    }

    private void BuscarUsuarioPorNombre()
    {
        LimpiarPantalla();
        MostrarEncabezado("BUSCAR USUARIOS POR NOMBRE");

        Console.Write("Ingrese el nombre a buscar: ");
        string? nombre = Console.ReadLine();

        var usuarios = usuarioService.BuscarUsuariosPorNombre(nombre ?? "");
        if (usuarios.Count > 0)
        {
            Console.WriteLine($"\nUsuarios encontrados: {usuarios.Count}\n");

            Console.WriteLine("┌──────────┬────────────────────┬──────────────────┬────────────────────┬───────────┬─────────┐");
            Console.WriteLine("│ Carné    │ Nombre             │ Carrera          │ Correo             │ Teléfono  │ Estado  │");
            Console.WriteLine("├──────────┼────────────────────┼──────────────────┼────────────────────┼───────────┼─────────┤");

            foreach (var usuario in usuarios)
            {
                string carne = usuario.Carne.PadRight(8);
                string nombrePadded = usuario.NombreCompleto.Length > 18 ? usuario.NombreCompleto.Substring(0, 18) : usuario.NombreCompleto.PadRight(18);
                string carrera = usuario.Carrera.Length > 16 ? usuario.Carrera.Substring(0, 16) : usuario.Carrera.PadRight(16);
                string correo = usuario.CorreoElectronico.Length > 18 ? usuario.CorreoElectronico.Substring(0, 18) : usuario.CorreoElectronico.PadRight(18);
                string telefono = usuario.Telefono.PadRight(9);
                string estado = usuario.Activo ? "Activo" : "Inactivo";

                Console.WriteLine($"│ {carne} │ {nombrePadded} │ {carrera} │ {correo} │ {telefono} │ {estado,-7} │");
            }

            Console.WriteLine("└──────────┴────────────────────┴──────────────────┴────────────────────┴───────────┴─────────┘");
        }
        else
            MostrarError("No se encontraron usuarios con ese nombre.");

        PausarPantalla();
    }

    private void ListarUsuarios()
    {
        LimpiarPantalla();
        MostrarEncabezado("LISTA DE TODOS LOS USUARIOS");

        var usuarios = usuarioService.ObtenerTodosUsuarios();
        if (usuarios.Count == 0)
        {
            MostrarError("No hay usuarios registrados.");
        }
        else
        {
            Console.WriteLine($"\nTotal de usuarios: {usuarios.Count}\n");

            // Encabezados
            Console.WriteLine("┌──────────┬────────────────────┬──────────────────┬────────────────────┬───────────┬─────────┐");
            Console.WriteLine("│ Carné    │ Nombre             │ Carrera          │ Correo             │ Teléfono  │ Estado  │");
            Console.WriteLine("├──────────┼────────────────────┼──────────────────┼────────────────────┼───────────┼─────────┤");

            foreach (var usuario in usuarios)
            {
                string carne = usuario.Carne.PadRight(8);
                string nombre = usuario.NombreCompleto.Length > 18 ? usuario.NombreCompleto.Substring(0, 18) : usuario.NombreCompleto.PadRight(18);
                string carrera = usuario.Carrera.Length > 16 ? usuario.Carrera.Substring(0, 16) : usuario.Carrera.PadRight(16);
                string correo = usuario.CorreoElectronico.Length > 18 ? usuario.CorreoElectronico.Substring(0, 18) : usuario.CorreoElectronico.PadRight(18);
                string telefono = usuario.Telefono.PadRight(9);
                string estado = usuario.Activo ? "Activo" : "Inactivo";

                Console.WriteLine($"│ {carne} │ {nombre} │ {carrera} │ {correo} │ {telefono} │ {estado,-7} │");
            }

            Console.WriteLine("└──────────┴────────────────────┴──────────────────┴────────────────────┴───────────┴─────────┘");
        }

        PausarPantalla();
    }

    private void CambiarEstadoUsuario()
    {
        LimpiarPantalla();
        MostrarEncabezado("CAMBIAR ESTADO DE USUARIO");

        Console.Write("Ingrese el carné del usuario: ");
        string? carne = Console.ReadLine();

        var usuario = usuarioService.BuscarUsuarioPorCarne(carne ?? "");
        if (usuario == null)
        {
            MostrarError("Usuario no encontrado.");
            PausarPantalla();
            return;
        }

        Console.WriteLine($"\nEstado actual: {(usuario.Value.Activo ? "Activo" : "Inactivo")}");
        Console.WriteLine("¿Desea cambiar el estado? (S/N): ");
        string? respuesta = Console.ReadLine();

        if (respuesta?.Equals("S", StringComparison.OrdinalIgnoreCase) == true)
        {
            var (exito, mensaje) = usuarioService.CambiarEstadoUsuario(carne ?? "", !usuario.Value.Activo);
            if (exito)
                MostrarExito(mensaje);
            else
                MostrarError(mensaje);
        }

        PausarPantalla();
    }

    #endregion

    #region Menú Gestión de Préstamos

    private void MenuGestionPrestamos()
    {
        bool volver = false;

        while (!volver)
        {
            LimpiarPantalla();
            MostrarEncabezado("GESTIÓN DE PRÉSTAMOS");
            Console.WriteLine($"Préstamos activos: {prestamoService.ObtenerCantidadPrestamosActivos()}/10\n");
            Console.WriteLine("╔═══════════════════════════════════════════════════════╗");
            Console.WriteLine("║  1. Registrar Préstamo                                ║");
            Console.WriteLine("║  2. Registrar Devolución                              ║");
            Console.WriteLine("║  3. Ver Préstamos Activos de Usuario                  ║");
            Console.WriteLine("║  4. Ver Todos los Préstamos                           ║");
            Console.WriteLine("║  5. Ver Historial de Préstamos                        ║");
            Console.WriteLine("║  6. Volver                                            ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════╝\n");

            Console.Write("Seleccione una opción (1-6): ");
            string? opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    RegistrarPrestamo();
                    break;
                case "2":
                    RegistrarDevolucion();
                    break;
                case "3":
                    VerPrestamosActivosUsuario();
                    break;
                case "4":
                    ListarTodosPrestamos();
                    break;
                case "5":
                    VerHistorialPrestamosUsuario();
                    break;
                case "6":
                    volver = true;
                    break;
                default:
                    MostrarError("Opción inválida. Por favor, intente de nuevo.");
                    PausarPantalla();
                    break;
            }
        }
    }

    private void RegistrarPrestamo()
    {
        LimpiarPantalla();
        MostrarEncabezado("REGISTRAR NUEVO PRÉSTAMO");

        // Validar y obtener carné
        Console.Write("Ingrese el carné del usuario: ");
        string? carneInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(carneInput) || !ValidatorHelper.ValidarCarne(carneInput))
        {
            MostrarError("Carné inválido. Debe contener exactamente 8 dígitos numéricos.");
            PausarPantalla();
            return;
        }

        var usuario = usuarioService.BuscarUsuarioPorCarne(carneInput);
        if (usuario == null)
        {
            MostrarError($"No existe usuario con carné '{carneInput}'.");
            PausarPantalla();
            return;
        }

        // Validar y obtener código de libro
        Console.Write("Ingrese el código del libro: ");
        string? codigoLibroInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(codigoLibroInput) || !ValidatorHelper.ValidarCodigoLibro(codigoLibroInput))
        {
            MostrarError("Código de libro inválido. Debe tener el formato: LIBxxxxx");
            PausarPantalla();
            return;
        }

        var libro = libroService.BuscarLibroPorCodigo(codigoLibroInput);
        if (libro == null)
        {
            MostrarError($"No existe libro con código '{codigoLibroInput}'.");
            PausarPantalla();
            return;
        }

        if (libro.Value.CantidadEjemplares <= 0)
        {
            MostrarError($"El libro '{libro.Value.Titulo}' no tiene ejemplares disponibles.");
            PausarPantalla();
            return;
        }

        // Si todas las validaciones pasaron, registrar el préstamo
        var (exito, mensaje) = prestamoService.RegistrarPrestamo(carneInput, codigoLibroInput);

        if (exito)
            MostrarExito(mensaje);
        else
            MostrarError(mensaje);

        PausarPantalla();
    }

    private void RegistrarDevolucion()
    {
        LimpiarPantalla();
        MostrarEncabezado("REGISTRAR DEVOLUCIÓN DE LIBRO");

        // Validar y obtener carné
        Console.Write("Ingrese el carné del usuario: ");
        string? carneInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(carneInput) || !ValidatorHelper.ValidarCarne(carneInput))
        {
            MostrarError("Carné inválido. Debe contener exactamente 8 dígitos numéricos.");
            PausarPantalla();
            return;
        }

        var usuario = usuarioService.BuscarUsuarioPorCarne(carneInput);
        if (usuario == null)
        {
            MostrarError($"No existe usuario con carné '{carneInput}'.");
            PausarPantalla();
            return;
        }

        // Validar y obtener código de libro
        Console.Write("Ingrese el código del libro: ");
        string? codigoLibroInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(codigoLibroInput) || !ValidatorHelper.ValidarCodigoLibro(codigoLibroInput))
        {
            MostrarError("Código de libro inválido. Debe tener el formato: LIBxxxxx");
            PausarPantalla();
            return;
        }

        var libro = libroService.BuscarLibroPorCodigo(codigoLibroInput);
        if (libro == null)
        {
            MostrarError($"No existe libro con código '{codigoLibroInput}'.");
            PausarPantalla();
            return;
        }

        // Si todas las validaciones pasaron, registrar la devolución
        var (exito, mensaje) = prestamoService.RegistrarDevolucion(carneInput, codigoLibroInput);

        if (exito)
            MostrarExito(mensaje);
        else
            MostrarError(mensaje);

        PausarPantalla();
    }

    private void VerPrestamosActivosUsuario()
    {
        LimpiarPantalla();
        MostrarEncabezado("PRÉSTAMOS ACTIVOS DE USUARIO");

        Console.Write("Ingrese el carné del usuario: ");
        string? carne = Console.ReadLine();

        var prestamos = prestamoService.ObtenerPrestamosActivosUsuario(carne ?? "");
        if (prestamos.Count > 0)
        {
            Console.WriteLine($"\nPréstamos activos: {prestamos.Count}\n");

            Console.WriteLine("┌──────────┬──────────┬──────────────┬──────────────┬─────────┐");
            Console.WriteLine("│ Carné    │ Código   │ Prestamo     │ Retorno      │ Días    │");
            Console.WriteLine("├──────────┼──────────┼──────────────┼──────────────┼─────────┤");

            foreach (var prestamo in prestamos)
            {
                string carneStr = prestamo.Carne.PadRight(8);
                string codigo = prestamo.CodigoLibro.PadRight(8);
                string fechaPrestamo = prestamo.FechaPrestamo.ToString("dd/MM/yyyy");
                string fechaRetorno = prestamo.FechaEstimadaDevolucion.ToString("dd/MM/yyyy");
                string diasRestantes = ((prestamo.FechaEstimadaDevolucion - DateTime.Now).Days.ToString()).PadRight(5);

                Console.WriteLine($"│ {carneStr} │ {codigo} │ {fechaPrestamo} │ {fechaRetorno} │ {diasRestantes} │");
            }

            Console.WriteLine("└──────────┴──────────┴──────────────┴──────────────┴─────────┘");
        }
        else
            MostrarError("No hay préstamos activos para este usuario.");

        PausarPantalla();
    }

    private void ListarTodosPrestamos()
    {
        LimpiarPantalla();
        MostrarEncabezado("LISTA DE TODOS LOS PRÉSTAMOS");

        var prestamos = prestamoService.ObtenerTodosPrestamos();
        if (prestamos.Count == 0)
        {
            MostrarError("No hay préstamos registrados.");
        }
        else
        {
            Console.WriteLine($"\nTotal de préstamos: {prestamos.Count}\n");

            Console.WriteLine("┌──────────┬──────────┬──────────────┬──────────────┬──────────────┬──────────────┐");
            Console.WriteLine("│ Carné    │ Código   │ Prestamo     │ Estimada     │ Devuelto     │ Estado       │");
            Console.WriteLine("├──────────┼──────────┼──────────────┼──────────────┼──────────────┼──────────────┤");

            foreach (var prestamo in prestamos)
            {
                string carne = prestamo.Carne.Substring(0, Math.Min(8, prestamo.Carne.Length)).PadRight(8);
                string codigo = prestamo.CodigoLibro.Substring(0, Math.Min(8, prestamo.CodigoLibro.Length)).PadRight(8);
                string fechaPrestamo = prestamo.FechaPrestamo.ToString("dd/MM/yyyy");
                string fechaEstimada = prestamo.FechaEstimadaDevolucion.ToString("dd/MM/yyyy");
                string fechaDevuelto = prestamo.FechaDevolucion.HasValue 
                    ? prestamo.FechaDevolucion.Value.ToString("dd/MM/yyyy")
                    : "---";
                fechaDevuelto = fechaDevuelto.PadRight(12);

                string estado = prestamo.EstaActivo ? "Activo" : "Completado";
                estado = estado.PadRight(12);

                Console.WriteLine($"│ {carne} │ {codigo} │ {fechaPrestamo} │ {fechaEstimada} │ {fechaDevuelto} │ {estado} │");
            }

            Console.WriteLine("└──────────┴──────────┴──────────────┴──────────────┴──────────────┴──────────────┘");
        }

        PausarPantalla();
    }

    private void VerHistorialPrestamosUsuario()
    {
        LimpiarPantalla();
        MostrarEncabezado("HISTORIAL DE PRÉSTAMOS DE USUARIO");

        Console.Write("Ingrese el carné del usuario: ");
        string? carne = Console.ReadLine();

        var prestamos = prestamoService.ObtenerPrestamosUsuario(carne ?? "");
        if (prestamos.Count > 0)
        {
            Console.WriteLine($"\nTotal de préstamos (activos e históricos): {prestamos.Count}\n");

            Console.WriteLine("┌──────────┬──────────┬──────────────┬──────────────┬──────────────┬──────────────┐");
            Console.WriteLine("│ Carné    │ Código   │ Prestamo     │ Estimada     │ Devuelto     │ Estado       │");
            Console.WriteLine("├──────────┼──────────┼──────────────┼──────────────┼──────────────┼──────────────┤");

            foreach (var prestamo in prestamos)
            {
                string carneStr = prestamo.Carne.Substring(0, Math.Min(8, prestamo.Carne.Length)).PadRight(8);
                string codigo = prestamo.CodigoLibro.Substring(0, Math.Min(8, prestamo.CodigoLibro.Length)).PadRight(8);
                string fechaPrestamo = prestamo.FechaPrestamo.ToString("dd/MM/yyyy");
                string fechaEstimada = prestamo.FechaEstimadaDevolucion.ToString("dd/MM/yyyy");
                string fechaDevuelto = prestamo.FechaDevolucion.HasValue 
                    ? prestamo.FechaDevolucion.Value.ToString("dd/MM/yyyy")
                    : "---";
                fechaDevuelto = fechaDevuelto.PadRight(12);

                string estado = prestamo.EstaActivo ? "Activo" : "Completado";
                estado = estado.PadRight(12);

                Console.WriteLine($"│ {carneStr} │ {codigo} │ {fechaPrestamo} │ {fechaEstimada} │ {fechaDevuelto} │ {estado} │");
            }

            Console.WriteLine("└──────────┴──────────┴──────────────┴──────────────┴──────────────┴──────────────┘");
        }
        else
            MostrarError("No hay préstamos para este usuario.");

        PausarPantalla();
    }

    #endregion

    #region Utilidades de Interfaz

    private static void MostrarEncabezado(string titulo)
    {
        Console.WriteLine("\n╔═══════════════════════════════════════════════════════╗");
        Console.WriteLine($"║ {CentrarTexto(titulo, 55)} ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════╝\n");
    }

    private static void MostrarExito(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n✓ {mensaje}");
        Console.ResetColor();
    }

    private static void MostrarError(string mensaje)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n✗ {mensaje}");
        Console.ResetColor();
    }

    private static void PausarPantalla()
    {
        Console.WriteLine("\nPresione cualquier tecla para continuar...");
        Console.ReadKey();
    }

    private static void LimpiarPantalla()
    {
        Console.Clear();
    }

    private static string CentrarTexto(string texto, int ancho)
    {
        int espacios = (ancho - texto.Length) / 2;
        return texto.PadLeft(texto.Length + espacios).PadRight(ancho);
    }

    #endregion
}
