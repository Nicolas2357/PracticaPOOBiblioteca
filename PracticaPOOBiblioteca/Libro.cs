using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaPOOBiblioteca
{
    internal class Libro
    {
        // ── Campos privados ────────────────────────────────────────────────
        private string _titulo;
        private string _autor;
        private int _año;
        private bool _disponible;
        private string _isbn;
        private string _genero;
        private int _ejemplares;
        private int _copiasPrestadas;

        // ── Propiedades ────────────────────────────────────────────────────
        public string Titulo => _titulo;
        public string Autor => _autor;
        public int Anio => _año;
        public bool Disponible => _copiasPrestadas < _ejemplares;
        public string ISBN => _isbn;
        public string Genero => _genero;
        public int Ejemplares => _ejemplares;
        public int CopiasDisponibles => _ejemplares - _copiasPrestadas;

        // ── Constructor 1: básico ──────────────────────────────────────────
        public Libro(string titulo, string autor, int año)
        {
            _titulo = titulo;
            _autor = autor;
            _año = año;
            _isbn = GenerarISBN();
            _genero = "Sin clasificar";
            _ejemplares = 1;
            _copiasPrestadas = 0;

            Console.WriteLine($"[+] Libro creado: \"{_titulo}\"");
        }

        // ── Constructor 2: completo ────────────────────────────────────────
        public Libro(string titulo, string autor, int anio,
                     string isbn, string genero, int ejemplares)
        {
            _titulo = titulo;
            _autor = autor;
            _año = anio;
            _isbn = isbn;
            _genero = genero;
            _ejemplares = ejemplares > 0 ? ejemplares : 1;
            _copiasPrestadas = 0;

            Console.WriteLine($"[+] Libro registrado: \"{_titulo}\" ({_ejemplares} ej.)");
        }

        // ── Constructor 3: copia de otro libro ────────────────────────────
        public Libro(Libro original, int ejemplaresExtra)
        {
            _titulo = original._titulo;
            _autor = original._autor;
            _año = original._año;
            _isbn = original._isbn;
            _genero = original._genero;
            _ejemplares = original._ejemplares + ejemplaresExtra;
            _copiasPrestadas = original._copiasPrestadas;

            Console.WriteLine($"[+] Copia ampliada: \"{_titulo}\" ({_ejemplares} ejemplares en total)");
        }

        // ── Destructor ─────────────────────────────────────────────────────
        ~Libro()
        {
            Console.WriteLine($"[-] Libro eliminado de memoria: \"{_titulo}\"");
        }

        // ── Métodos ────────────────────────────────────────────────────────
        public bool PrestarLibro()
        {
            if (Disponible)
            {
                _copiasPrestadas++;
                return true;
            }
            return false;
        }

        public bool DevolverLibro()
        {
            if (_copiasPrestadas > 0)
            {
                _copiasPrestadas--;
                return true;
            }
            return false;
        }

        public void MostrarInfo()
        {
            Console.WriteLine("┌─────────────────────────────────────────┐");
            Console.WriteLine($"  Título   : {_titulo}");
            Console.WriteLine($"  Autor    : {_autor}");
            Console.WriteLine($"  Año      : {_año}");
            Console.WriteLine($"  ISBN     : {_isbn}");
            Console.WriteLine($"  Género   : {_genero}");
            Console.WriteLine($"  Copias   : {CopiasDisponibles}/{_ejemplares} disponibles");
            Console.WriteLine($"  Estado   : {(Disponible ? "Disponible" : "Sin copias")}");
            Console.WriteLine("└─────────────────────────────────────────┘");
        }

        public string ResumenCorto() =>
            $"{_titulo} — {_autor} ({_año}) [{(Disponible ? "Disp." : "No disp.")}]";

        private static string GenerarISBN()
        {
            var rnd = new Random();
            return $"978-{rnd.Next(100, 999)}-{rnd.Next(10000, 99999)}-{rnd.Next(0, 9)}";
        }
    }
}

