using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaPOOBiblioteca
{
    internal class Program
    {
        private static Biblioteca biblioteca;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            InicializarDatosDemostracion();

            bool salir = false;
            while (!salir)
            {
                MostrarMenuPrincipal();
                string opcion = Console.ReadLine()?.Trim();

                switch (opcion)
                {
                    case "1": MenuBuscarLibro(); break;
                    case "2": MenuPrestarLibro(); break;
                    case "3": MenuDevolverLibro(); break;
                    case "4": MenuRegistrarLibro(); break;
                    case "5": MenuRegistrarCliente(); break;
                    case "6": biblioteca.MostrarCatalogo(); Pausa(); break;
                    case "7": biblioteca.MostrarClientes(); Pausa(); break;
                    case "8": biblioteca.MostrarHistorial(); Pausa(); break;
                    case "9": biblioteca.MostrarResumen(); Pausa(); break;
                    case "0":
                        salir = true;
                        Console.WriteLine("\n  Hasta luego. ¡Que disfrute sus lecturas!\n");
                        break;
                    default:
                        Console.WriteLine("  Opción no válida. Intente de nuevo.");
                        Pausa();
                        break;
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // MENÚS
        // ══════════════════════════════════════════════════════════════════

        static void MostrarMenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine($"  {biblioteca.Nombre}");
            Console.WriteLine("╠══════════════════════════════════════════╣");
            Console.WriteLine("  [1] Buscar libro");
            Console.WriteLine("  [2] Prestar libro");
            Console.WriteLine("  [3] Devolver libro");
            Console.WriteLine("  [4] Registrar nuevo libro");
            Console.WriteLine("  [5] Registrar nuevo cliente");
            Console.WriteLine("  ─────────────────────────");
            Console.WriteLine("  [6] Ver catálogo completo");
            Console.WriteLine("  [7] Ver clientes");
            Console.WriteLine("  [8] Ver historial de préstamos");
            Console.WriteLine("  [9] Resumen general");
            Console.WriteLine("  [0] Salir");
            Console.WriteLine("╚══════════════════════════════════════════╝");
            Console.Write("  Opción: ");
        }

        // ── 1. Buscar libro ────────────────────────────────────────────────
        static void MenuBuscarLibro()
        {
            Console.Clear();
            Console.WriteLine("── Buscar libro ─────────────────────────────");
            Console.WriteLine("  [1] Por título");
            Console.WriteLine("  [2] Por autor");
            Console.WriteLine("  [3] Por ISBN");
            Console.Write("  Opción: ");
            string op = Console.ReadLine()?.Trim();

            List<Libro> resultados = null;
            Libro unico = null;

            switch (op)
            {
                case "1":
                    Console.Write("  Título (o parte): ");
                    string titulo = Console.ReadLine();
                    resultados = biblioteca.BuscarPorTitulo(titulo);
                    break;
                case "2":
                    Console.Write("  Autor (o parte): ");
                    string autor = Console.ReadLine();
                    resultados = biblioteca.BuscarPorAutor(autor);
                    break;
                case "3":
                    Console.Write("  ISBN exacto: ");
                    string isbn = Console.ReadLine()?.Trim();
                    unico = biblioteca.BuscarPorISBN(isbn);
                    break;
            }

            Console.WriteLine();
            if (unico != null)
            {
                unico.MostrarInfo();
            }
            else if (resultados != null)
            {
                if (resultados.Count == 0)
                    Console.WriteLine("  No se encontraron resultados.");
                else
                    foreach (var l in resultados)
                        l.MostrarInfo();
            }
            Pausa();
        }

        // ── 2. Prestar libro ───────────────────────────────────────────────
        static void MenuPrestarLibro()
        {
            Console.Clear();
            Console.WriteLine("── Prestar libro ────────────────────────────");
            biblioteca.MostrarCatalogo();

            Console.Write("\n  ISBN del libro a prestar: ");
            string isbn = Console.ReadLine()?.Trim();
            Libro libro = biblioteca.BuscarPorISBN(isbn);

            if (libro == null)
            {
                Console.WriteLine("  ✘ ISBN no encontrado.");
                Pausa(); return;
            }

            biblioteca.MostrarClientes();
            Console.Write("\n  ID del cliente: ");
            if (!int.TryParse(Console.ReadLine(), out int idCliente))
            {
                Console.WriteLine("  ✘ ID no válido.");
                Pausa(); return;
            }
            Cliente cliente = biblioteca.BuscarClientePorID(idCliente);

            Console.Write("  Días de préstamo [Enter = 14]: ");
            string diasStr = Console.ReadLine()?.Trim();
            int dias = string.IsNullOrEmpty(diasStr) ? 14 : int.Parse(diasStr);

            biblioteca.PrestarLibro(cliente, libro, dias);
            Pausa();
        }

        // ── 3. Devolver libro ──────────────────────────────────────────────
        static void MenuDevolverLibro()
        {
            Console.Clear();
            Console.WriteLine("── Devolver libro ───────────────────────────");
            biblioteca.MostrarHistorial();

            Console.Write("\n  ID del préstamo a devolver: ");
            if (!int.TryParse(Console.ReadLine(), out int idPrestamo))
            {
                Console.WriteLine("  ✘ ID no válido.");
                Pausa(); return;
            }

            biblioteca.DevolverLibro(idPrestamo);
            Pausa();
        }

        // ── 4. Registrar libro ─────────────────────────────────────────────
        static void MenuRegistrarLibro()
        {
            Console.Clear();
            Console.WriteLine("── Registrar nuevo libro ────────────────────");

            Console.Write("  Título   : "); string titulo = Console.ReadLine();
            Console.Write("  Autor    : "); string autor = Console.ReadLine();
            Console.Write("  Año      : ");
            int.TryParse(Console.ReadLine(), out int anio);
            Console.Write("  Género   : "); string genero = Console.ReadLine();
            Console.Write("  Ejemplares [1]: ");
            string ejStr = Console.ReadLine()?.Trim();
            int ejemplares = string.IsNullOrEmpty(ejStr) ? 1 : int.Parse(ejStr);

            // Usamos constructor 2 (completo) — el ISBN se genera automáticamente
            // pero aquí lo pedimos para demostrar el paso por valor de tipos primitivos
            Console.Write("  ISBN (dejar vacío = auto): ");
            string isbn = Console.ReadLine()?.Trim();

            Libro nuevo;
            if (string.IsNullOrEmpty(isbn))
                nuevo = new Libro(titulo, autor, anio, $"AUTO-{DateTime.Now.Ticks % 10000}",
                                  genero, ejemplares);
            else
                nuevo = new Libro(titulo, autor, anio, isbn, genero, ejemplares);

            biblioteca.AgregarLibro(nuevo);
            nuevo.MostrarInfo();
            Pausa();
        }

        // ── 5. Registrar cliente ───────────────────────────────────────────
        static void MenuRegistrarCliente()
        {
            Console.Clear();
            Console.WriteLine("── Registrar nuevo cliente ──────────────────");

            Console.Write("  Nombre   : "); string nombre = Console.ReadLine();
            Console.Write("  Apellido : "); string apellido = Console.ReadLine();
            Console.Write("  Email    : "); string email = Console.ReadLine();
            Console.Write("  Límite de libros [3]: ");
            string limStr = Console.ReadLine()?.Trim();
            int limite = string.IsNullOrEmpty(limStr) ? 3 : int.Parse(limStr);

            Cliente nuevo;
            if (string.IsNullOrEmpty(email))
                nuevo = new Cliente(nombre, apellido);              // constructor 1
            else
                nuevo = new Cliente(nombre, apellido, email, limite); // constructor 2

            biblioteca.RegistrarCliente(nuevo);
            nuevo.MostrarInfo();
            Pausa();
        }

        // ══════════════════════════════════════════════════════════════════
        // DATOS DE DEMOSTRACIÓN
        // ══════════════════════════════════════════════════════════════════

        static void InicializarDatosDemostracion()
        {

            // Constructor 1 de Biblioteca (solo nombre)
            biblioteca = new Biblioteca("Biblioteca Virtual Cervantes");

            // Libros usando distintos constructores
            var libro1 = new Libro("Don Quijote de la Mancha",
                                   "Miguel de Cervantes", 1605);                    // ctor 1

            var libro2 = new Libro("Cien años de soledad",
                                   "Gabriel García Márquez", 1967,
                                   "978-958-614-073-4", "Realismo mágico", 3);      // ctor 2

            var libro3 = new Libro("1984",
                                   "George Orwell", 1949,
                                   "978-0-451-52493-5", "Distopía", 2);             // ctor 2

            var libro4 = new Libro(libro2, 2);                                      // ctor 3 (copia)

            var libro5 = new Libro("El nombre de la rosa",
                                   "Umberto Eco", 1980,
                                   "978-84-322-1698-2", "Novela histórica", 1);

            biblioteca.AgregarLibro(libro1);
            biblioteca.AgregarLibro(libro2);
            biblioteca.AgregarLibro(libro3);
            biblioteca.AgregarLibro(libro4);
            biblioteca.AgregarLibro(libro5);

            // Clientes usando distintos constructores
            var cliente1 = new Cliente("Ana", "García");                          // ctor 1
            var cliente2 = new Cliente("Carlos", "Martínez",
                                       "carlos@mail.com", 5);                        // ctor 2 (VIP)
            var cliente3 = new Cliente("Lucía", "Hernández", "lucia@mail.com", 3);

            biblioteca.RegistrarCliente(cliente1);
            biblioteca.RegistrarCliente(cliente2);
            biblioteca.RegistrarCliente(cliente3);

            // Préstamos de ejemplo
            biblioteca.PrestarLibro(cliente1, libro1, 7);
            biblioteca.PrestarLibro(cliente2, libro2, 14);

            Console.WriteLine("\n  Datos de demostración cargados. Presione Enter...");
            Console.ReadLine();

            Console.WriteLine("\n=== DEMOSTRACIÓN SOBRECARGA ===");

            var resultado1 = biblioteca.BuscarLibro("1984");

            var resultado2 =
                biblioteca.BuscarLibro(
                    "Cien años de soledad",
                    "Gabriel");

            var resultado3 =
                biblioteca.BuscarLibro(
                    "978-0-451-52493-5",
                    true);

            Console.WriteLine(
                $"Busqueda 1: {resultado1.Count} resultado(s)");

            Console.WriteLine(
                $"Busqueda 2: {resultado2.Count} resultado(s)");

            Console.WriteLine(
                $"Busqueda 3: {(resultado3 != null ? "Encontrado" : "No encontrado")}");
        }

        // ── Utilidad ───────────────────────────────────────────────────────
        static void Pausa()
        {
            Console.WriteLine("\n  [Enter para continuar]");
            Console.ReadLine();
        }
    }
}
