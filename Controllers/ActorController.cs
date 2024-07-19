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
    public class ActorController : Controller
    {
        private readonly MovieDbContext _context;

        public ActorController(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(long? id)
        {
            var actor = await _context.Actors.Include(a => a.Movies).FirstOrDefaultAsync(a => a.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }
    
    }
}