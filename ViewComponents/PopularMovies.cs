using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data.Abstract;

namespace MovieApp.ViewComponents
{
    public class PopularMovies : ViewComponent{
        
        private IMovieRepository _movieRepository;

        public PopularMovies(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var movies = await _movieRepository.Movies.Include(m => m.Reviews).OrderByDescending(m => m.Reviews.Count).Take(4).ToListAsync();
            return View(movies);
        }

    }
}