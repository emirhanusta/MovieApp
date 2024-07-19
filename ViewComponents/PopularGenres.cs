using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data.Abstract;

namespace MovieApp.ViewComponents
{
    public class PopularGenres : ViewComponent
    {
        private readonly IGenreRepoository genreRepoository;

        public PopularGenres(IGenreRepoository genreRepoository)
        {
            this.genreRepoository = genreRepoository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genres = await genreRepoository.Genres.Include(g => g.Movies).OrderByDescending(g => g.Movies.Count).Take(5).ToListAsync();
            return View(genres);
        }

    }
}