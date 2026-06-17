using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaPOOBiblioteca
{
    internal class Cliente
    {
        // ── Campos ─────────────────────────────────────────────────────────
        private static int _contadorID = 1;          // variable de clase (static)

        private int _id;
        private string _nombre;
        private string _apellido;
        private string _email;
        private int _limiteLibros;            // máx. préstamos simultáneos
        private List<string> _librosPrestados;        // ISBNs activos

        // ── Propiedades ────────────────────────────────────────────────────
        public int ID => _id;
        public string Nombre => _nombre;
        public string Apellido => _apellido;
        public string NombreCompleto => $"{_nombre} {_apellido}";
        public string Email => _email;
        public int LimiteLibros => _limiteLibros;
        public int LibrosActivos => _librosPrestados.Count;
        public bool PuedePedir => _librosPrestados.Count < _limiteLibros;

        // ── Constructor 1: básico ──────────────────────────────────────────
        public Cliente(string nombre, string apellido)
        {
            _id = _contadorID++;
            _nombre = nombre;
            _apellido = apellido;
            _email = "sin_email@biblioteca.com";
            _limiteLibros = 3;                     // límite por defecto
            _librosPrestados = new List<string>();

            Console.WriteLine($"[+] Cliente registrado: {NombreCompleto} (ID: {_id})");
        }

        // ── Constructor 2: completo con email y límite personalizado ───────
        public Cliente(string nombre, string apellido, string email, int limiteLibros)
        {
            _id = _contadorID++;
            _nombre = nombre;
            _apellido = apellido;
            _email = email;
            _limiteLibros = limiteLibros > 0 ? limiteLibros : 3;
            _librosPrestados = new List<string>();

            Console.WriteLine($"[+] Cliente VIP registrado: {NombreCompleto} " +
                              $"(ID: {_id}, límite: {_limiteLibros})");
        }

        // ── Destructor ─────────────────────────────────────────────────────
        ~Cliente()
        {
            Console.WriteLine($"[-] Cliente eliminado de memoria: {NombreCompleto}");
        }

        // ── Métodos ────────────────────────────────────────────────────────

        /// <summary>Agrega el ISBN del libro a los préstamos activos del cliente.</summary>
        public bool AgregarPrestamo(string isbn)
        {
            if (!PuedePedir) return false;
            _librosPrestados.Add(isbn);
            return true;
        }

        /// <summary>Quita el ISBN al registrar la devolución.</summary>
        public bool QuitarPrestamo(string isbn)
        {
            return _librosPrestados.Remove(isbn);
        }

        /// <summary>Verifica si el cliente ya tiene prestado ese ISBN.</summary>
        public bool TienePrestado(string isbn) =>
            _librosPrestados.Contains(isbn);

        /// <summary>Ficha completa del cliente.</summary>
        public void MostrarInfo()
        {
            Console.WriteLine("┌─────────────────────────────────────────┐");
            Console.WriteLine($"  ID       : {_id}");
            Console.WriteLine($"  Nombre   : {NombreCompleto}");
            Console.WriteLine($"  Email    : {_email}");
            Console.WriteLine($"  Préstamos: {LibrosActivos}/{_limiteLibros}");

            if (_librosPrestados.Count > 0)
            {
                Console.WriteLine("  Libros activos (ISBN):");
                foreach (string isbn in _librosPrestados)
                    Console.WriteLine($"    • {isbn}");
            }
            Console.WriteLine("└─────────────────────────────────────────┘");
        }

    }
}
