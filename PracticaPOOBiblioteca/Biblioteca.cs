using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaPOOBiblioteca
{
    internal class Biblioteca
    {
        private string _nombre;
        private List<Libro> _catalogo;
        private List<Cliente> _clientes;
        private List<Prestamo> _historial;

        // ── Propiedades ────────────────────────────────────────────────────
        public string Nombre => _nombre;
        public int TotalLibros => _catalogo.Count;
        public int TotalClientes => _clientes.Count;
        public int PrestamosActivos => _historial.Count(p => !p.Devuelto);

        // ── Constructor 1: solo nombre ─────────────────────────────────────
        public Biblioteca(string nombre)
        {
            _nombre = nombre;
            _catalogo = new List<Libro>();
            _clientes = new List<Cliente>();
            _historial = new List<Prestamo>();

            Console.WriteLine($"\n╔══════════════════════════════════════╗");
            Console.WriteLine($"  Biblioteca \"{_nombre}\" inicializada.");
            Console.WriteLine($"╚══════════════════════════════════════╝\n");
        }

        // ── Constructor 2: con catálogo inicial ────────────────────────────
        public Biblioteca(string nombre, List<Libro> catalogoInicial)
            : this(nombre)
        {
            _catalogo = new List<Libro>(catalogoInicial);
            Console.WriteLine($"  Catálogo cargado: {_catalogo.Count} libro(s).");
        }

        // ── Destructor ─────────────────────────────────────────────────────
        ~Biblioteca()
        {
            Console.WriteLine($"\n[-] Biblioteca \"{_nombre}\" cerrada.");
        }

        // ── Gestión de libros ──────────────────────────────────────────────
        public void AgregarLibro(Libro libro)
        {
            _catalogo.Add(libro);
            Console.WriteLine($"  + Libro agregado: \"{libro.Titulo}\"");
        }
        public List<Libro> BuscarLibro(string titulo)
        {
            return _catalogo
                .Where(l => l.Titulo.ToLower()
                .Contains(titulo.ToLower()))
                .ToList();
        }

        // Buscar por título y autor
        public List<Libro> BuscarLibro(string titulo, string autor)
        {
            return _catalogo
                .Where(l =>
                    l.Titulo.ToLower().Contains(titulo.ToLower()) &&
                    l.Autor.ToLower().Contains(autor.ToLower()))
                .ToList();
        }

        // Buscar por ISBN exacto
        public Libro BuscarLibro(string isbn, bool buscarPorISBN)
        {
            return _catalogo
                .FirstOrDefault(l => l.ISBN == isbn);
        }

        public List<Libro> BuscarPorTitulo(string texto) =>
            _catalogo.Where(l => l.Titulo.ToLower().Contains(texto.ToLower())).ToList();

        public List<Libro> BuscarPorAutor(string autor) =>
            _catalogo.Where(l => l.Autor.ToLower().Contains(autor.ToLower())).ToList();

        public Libro BuscarPorISBN(string isbn) =>
            _catalogo.FirstOrDefault(l => l.ISBN == isbn);

        // ── Gestión de clientes ────────────────────────────────────────────
        public void RegistrarCliente(Cliente cliente)
        {
            _clientes.Add(cliente);
            Console.WriteLine($"  + Cliente registrado: {cliente.NombreCompleto}");
        }

        public Cliente BuscarClientePorID(int id) =>
            _clientes.FirstOrDefault(c => c.ID == id);

        public List<Cliente> BuscarClientePorNombre(string nombre) =>
            _clientes.Where(c => c.NombreCompleto.ToLower().Contains(nombre.ToLower())).ToList();

        // ── Préstamos y devoluciones ───────────────────────────────────────
        public Prestamo PrestarLibro(Cliente cliente, Libro libro, int dias = 14)
        {
            if (libro == null)
            {
                Console.WriteLine("  X Libro no encontrado.");
                return null;
            }
            if (cliente == null)
            {
                Console.WriteLine("  X Cliente no encontrado.");
                return null;
            }
            if (!libro.Disponible)
            {
                Console.WriteLine($"  X \"{libro.Titulo}\" no tiene copias disponibles.");
                return null;
            }
            if (!cliente.PuedePedir)
            {
                Console.WriteLine($"  X {cliente.NombreCompleto} alcanzó su límite de préstamos.");
                return null;
            }
            if (cliente.TienePrestado(libro.ISBN))
            {
                Console.WriteLine($"  X {cliente.NombreCompleto} ya tiene ese libro.");
                return null;
            }

            libro.PrestarLibro();
            cliente.AgregarPrestamo(libro.ISBN);

            var prestamo = new Prestamo(cliente, libro, dias);
            _historial.Add(prestamo);

            Console.WriteLine($"\n  Prestamo registrado:");
            prestamo.MostrarInfo();
            return prestamo;
        }

        public bool DevolverLibro(int idPrestamo)
        {
            var prestamo = _historial.FirstOrDefault(p => p.ID == idPrestamo);

            if (prestamo == null)
            {
                Console.WriteLine("  X Prestamo no encontrado.");
                return false;
            }
            if (prestamo.Devuelto)
            {
                Console.WriteLine("  X Este prestamo ya fue devuelto.");
                return false;
            }

            prestamo.RegistrarDevolucion();
            prestamo.LibroPrestado.DevolverLibro();
            prestamo.ClientePrestamo.QuitarPrestamo(prestamo.LibroPrestado.ISBN);

            Console.WriteLine($"\n  Devolucion registrada:");
            prestamo.MostrarInfo();
            return true;
        }

        // ── Reportes ───────────────────────────────────────────────────────
        public void MostrarCatalogo()
        {
            Console.WriteLine($"\n── Catalogo ({TotalLibros} libros) ──────────────────");
            if (_catalogo.Count == 0) { Console.WriteLine("  (vacio)"); return; }
            foreach (var l in _catalogo)
                Console.WriteLine($"  * {l.ResumenCorto()}");
        }

        public void MostrarClientes()
        {
            Console.WriteLine($"\n── Clientes ({TotalClientes}) ──────────────────────");
            if (_clientes.Count == 0) { Console.WriteLine("  (ninguno)"); return; }
            foreach (var c in _clientes)
                Console.WriteLine($"  [{c.ID}] {c.NombreCompleto} — {c.LibrosActivos}/{c.LimiteLibros} prestamos");
        }

        public void MostrarHistorial()
        {
            Console.WriteLine($"\n── Historial de prestamos ({_historial.Count}) ────");
            if (_historial.Count == 0) { Console.WriteLine("  (ninguno)"); return; }
            foreach (var p in _historial)
                Console.WriteLine($"  * {p.ResumenCorto()}");
        }

        public void MostrarResumen()
        {
            Console.WriteLine("\n╔══════════════════════════════════════╗");
            Console.WriteLine($"  Biblioteca    : {_nombre}");
            Console.WriteLine($"  Libros        : {TotalLibros}");
            Console.WriteLine($"  Clientes      : {TotalClientes}");
            Console.WriteLine($"  Prestamos     : {PrestamosActivos} activos / {_historial.Count} totales");
            int moras = _historial.Count(p => p.EnMora);
            if (moras > 0)
                Console.WriteLine($"  En mora       : {moras}");
            Console.WriteLine("╚══════════════════════════════════════╝");
        }
    }
}
