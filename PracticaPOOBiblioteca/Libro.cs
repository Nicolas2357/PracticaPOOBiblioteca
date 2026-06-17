using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaPOOBiblioteca
{
    internal class Libro
    {
        private string titulo;
        private string autor;
        private int anio;
        private bool disponible;

        public Libro(string titulo, string autor, int anio)
        {
            this.titulo = titulo;
            this.autor = autor;
            this.anio = anio;
            disponible = true;
        }

        public void MostrarInfo()
        {
            Console.WriteLine("Título: " + titulo);
            Console.WriteLine("Autor: " + autor);
            Console.WriteLine("Año: " + anio);
            Console.WriteLine("Disponible: " + disponible);
        }

        public void PrestarLibro()
        {
            if (disponible)
            {
                disponible = false;
                Console.WriteLine("Libro prestado correctamente.");
            }
            else
            {
                Console.WriteLine("El libro ya está prestado.");
            }
        }

        public void DevolverLibro()
        {
            disponible = true;
            Console.WriteLine("Libro devuelto correctamente.");
        }
    }
}
