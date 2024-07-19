using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.Data.Abstract;
using MovieApp.Data.Concrete.Context;
using MovieApp.Entities;

namespace MovieApp.Data.Concrete
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieDbContext _context;

        public MovieRepository(MovieDbContext context)
        {
            _context = context;
        }
        public IQueryable<Movie> Movies => _context.Movies;

        public void SaveMovie(Movie entity)
        {
            _context.Movies.Add(entity);
            _context.SaveChanges();
        }
    }


}