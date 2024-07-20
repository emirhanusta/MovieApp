using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data.Abstract;

namespace MovieApp.ViewComponents
{
    public class NewMovies : ViewComponent
    {
        private readonly IMovieRepository _movieRepository;

        public NewMovies(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var movies = await _movieRepository.Movies.OrderBy(m => m.CreatedDate).Take(100).ToListAsync();
            return View(movies);
        }
    }
}