using BibliotecaUniversitaria;

// ============================================================
// VERSIÓN GENERADA CON INTELIGENCIA ARTIFICIAL
// Sistema Integral de Gestión de Biblioteca Universitaria
// ============================================================

// Inicializar servicios
var gestor = new GestorArchivos();
var servicioLibros = new ServicioLibros(gestor);
var servicioUsuarios = new ServicioUsuarios(gestor);
var servicioPrestamos = new ServicioPrestamos(gestor, servicioLibros, servicioUsuarios);

// Mostrar menú principal
var menu = new MenuPrincipal(servicioLibros, servicioUsuarios, servicioPrestamos);
menu.MostrarMenu();
