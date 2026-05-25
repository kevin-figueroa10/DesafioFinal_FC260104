using Proyecto_final.Services;
using Proyecto_final.UI;
using Proyecto_final.Data;

// Inicializar servicios
var libroService = new LibroService();
var usuarioService = new UsuarioService();
var prestamoService = new PrestamoService(libroService, usuarioService);

// Crear y mostrar menú
var menu = new Menu(libroService, usuarioService, prestamoService);
menu.MostrarMenuPrincipal();
