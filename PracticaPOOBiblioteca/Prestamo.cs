using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaPOOBiblioteca
{
    internal class Prestamo
    {
        // ── Campos ─────────────────────────────────────────────────────────
        private static int _contadorID = 1;

        private int _id;
        private Cliente _cliente;         
        private Libro _libro;           
        private DateTime _fechaPrestamo;
        private DateTime _fechaDevolucionEsperada;
        private DateTime? _fechaDevolucionReal;  
        private bool _devuelto;

        // ── Propiedades ────────────────────────────────────────────────────
        public int ID => _id;
        public Cliente ClientePrestamo => _cliente;
        public Libro LibroPrestado => _libro;
        public DateTime FechaPrestamo => _fechaPrestamo;
        public bool Devuelto => _devuelto;
        public bool EnMora =>
            !_devuelto && DateTime.Now > _fechaDevolucionEsperada;

        // ── Constructor: días de préstamo como parámetro por valor ─────────
        public Prestamo(Cliente cliente, Libro libro, int diasPrestamo = 14)
        {
            _id = _contadorID++;
            _cliente = cliente;   // paso por referencia (objeto)
            _libro = libro;
            _fechaPrestamo = DateTime.Now;
            _fechaDevolucionEsperada = DateTime.Now.AddDays(diasPrestamo);
            _fechaDevolucionReal = null;
            _devuelto = false;
        }

        // ── Destructor ─────────────────────────────────────────────────────
        ~Prestamo()
        {
            Console.WriteLine($"[-] Préstamo #{_id} eliminado de memoria.");
        }

        // ── Métodos ────────────────────────────────────────────────────────

        /// <summary>Registra la devolución del préstamo.</summary>
        public void RegistrarDevolucion()
        {
            _fechaDevolucionReal = DateTime.Now;
            _devuelto = true;
        }

        /// <summary>Muestra el ticket del préstamo.</summary>
        public void MostrarInfo()
        {
            Console.WriteLine("╔═════════════════════════════════════════╗");
            Console.WriteLine($"  Préstamo  #  : {_id}");
            Console.WriteLine($"  Cliente      : {_cliente.NombreCompleto}");
            Console.WriteLine($"  Libro        : {_libro.Titulo}");
            Console.WriteLine($"  Prestado     : {_fechaPrestamo:dd/MM/yyyy}");
            Console.WriteLine($"  Dev. esperada: {_fechaDevolucionEsperada:dd/MM/yyyy}");

            if (_devuelto)
                Console.WriteLine($"  Dev. real    : {_fechaDevolucionReal:dd/MM/yyyy} ✔");
            else
                Console.WriteLine($"  Estado       : {(EnMora ? "⚠ EN MORA" : "En curso")}");

            Console.WriteLine("╚═════════════════════════════════════════╝");
        }

        /// <summary>Resumen corto para listados.</summary>
        public string ResumenCorto()
        {
            string estado = _devuelto ? "Devuelto" : (EnMora ? "EN MORA" : "Activo");
            return $"#{_id} | {_cliente.NombreCompleto} | {_libro.Titulo} | {estado}";
        }
    }
}
