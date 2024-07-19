using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Data.Concrete.Context;
using MovieApp.Entities;
using MovieApp.Models;

namespace MovieApp.Controllers;

public class HomeController : Controller
{
    private readonly MovieDbContext _context;

    public HomeController(MovieDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index() => View(await _context.Movies.ToListAsync());
}
