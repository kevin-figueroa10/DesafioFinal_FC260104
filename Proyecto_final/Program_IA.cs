using BibliotecaUniversitaria;

// Inicializar servicios
var gestor = new GestorArchivos();
var servicioLibros = new ServicioLibros(gestor);
var servicioUsuarios = new ServicioUsuarios(gestor);
var servicioPrestamos = new ServicioPrestamos(gestor, servicioLibros, servicioUsuarios);

// Mostrar menú
var menu = new MenuPrincipal(servicioLibros, servicioUsuarios, servicioPrestamos);
menu.MostrarMenu();
