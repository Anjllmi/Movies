using Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Services
{
    public class MovieSingleton
    {
        /*Lista de películas, solo lectura*/
        private readonly List<Movie> Items = new();

        /// <summary>
        /// Método que obtiene todos los elementos de la lista con sintaxis LAMBDA.
        /// </summary>
        /// <returns></returns>
        public List<Movie> GetAll() => Items;

        /// <summary>
        /// Variable que obtiene todos los elementos de la lista con sintaxis LAMBDA.
        /// </summary>
        /// <returns></returns>
        public List<Movie> All { get => Items; }


        /// <summary>
        /// Añade película a la lista.
        /// </summary>
        /// <param name="movie"></param>
        public void Add(Movie movie)
        {
            Items.Add(movie);
        }

    }
}
