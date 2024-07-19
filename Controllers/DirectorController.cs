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
    public class DirectorController : Controller
    {
        
        private readonly MovieDbContext _context;

        public DirectorController(MovieDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Directors.Include(m => m.Movies).FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
    }
}