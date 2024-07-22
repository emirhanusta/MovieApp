using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Entities;

namespace MovieApp.Data.Abstract
{
    public interface IMovieRepository
    {
        IQueryable<Movie> Movies { get; }
        void SaveMovie(Movie entity);
        void UpdateMovie(Movie entity);
        void DeleteMovie(Movie entity);
    }
}