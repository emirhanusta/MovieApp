using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieApp.Data.Concrete.Context;

namespace MovieApp.Controllers
{
    public class GenreController : Controller
    {
        private readonly MovieDbContext _context;

        public GenreController(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var genre = await _context.Genres.Include(g => g.Movies).FirstOrDefaultAsync(g => g.Id == id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }
    }
}