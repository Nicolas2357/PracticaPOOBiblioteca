using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaPOOBiblioteca
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Libro libro1 = new Libro(
           "Don Quijote de la Mancha",
           "Miguel de Cervantes",
           1605);

            libro1.MostrarInfo();

            Console.WriteLine();

            libro1.PrestarLibro();

            Console.WriteLine();

            libro1.MostrarInfo();

            Console.WriteLine();

            libro1.DevolverLibro();

            Console.WriteLine();

            libro1.MostrarInfo();
        }
    }
}
