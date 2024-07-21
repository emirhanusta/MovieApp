using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using MovieApp.Data.Abstract;
using MovieApp.Data.Concrete;
using MovieApp.Data.Concrete.Context;
using MovieApp.Entities;
using MovieApp.Models;
namespace MovieApp.Controllers
{
    public class WatchlistController : Controller
    {
        private readonly IWatchlistRepository _watchlistRepository;
        private readonly IMovieRepository _movieRepository;

        public WatchlistController(IWatchlistRepository watchlistRepository, IMovieRepository movieRepository)
        {
            _watchlistRepository = watchlistRepository;
            _movieRepository = movieRepository;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var watchlist = _watchlistRepository.Watchlists.Include(w => w.Movies).Where(w => w.UserId == Convert.ToInt64(userId)).ToList();
            return View(watchlist);
        }

        [Authorize]
        public IActionResult Create()
        {
            ViewBag.Movies = _movieRepository.Movies.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(WatchlistCreateViewModel watchlist, long[] movieIds)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newWatchlist = new Watchlist
            {
                Name = watchlist.Name,
                Description = watchlist.Description,
                UserId = Convert.ToInt64(userId),
                Movies = _movieRepository.Movies.Where(m => movieIds.Contains(m.Id)).ToList(),
                CreatedDate = new DateTime(),
            };
            _watchlistRepository.AddWatchlist(newWatchlist);
            return RedirectToAction("Index");
        }

        public IActionResult Details(long id)
        {
            var watchlist = _watchlistRepository.Watchlists.Include(w => w.Movies).FirstOrDefault(w => w.Id == id);
            return View(watchlist);
        }

        [Authorize (Roles = "Admin")]
        public IActionResult Edit(long id)
        {
            var watchlist = _watchlistRepository.Watchlists.Include(w => w.Movies).FirstOrDefault(w => w.Id == id);
            if (watchlist == null)
            {
                return NotFound();
            }

            var watchlistEditViewModel = new WatchlistEditViewModel
            {
                Id = watchlist.Id,
                Name = watchlist.Name,
                Description = watchlist.Description,
                MovieIds = watchlist.Movies.Select(m => m.Id).ToArray()
            };

            ViewBag.MovieOptions = _movieRepository.Movies.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Title
            }).ToList();

            return View(watchlistEditViewModel);
        }



        [HttpPost]
        public IActionResult Edit(WatchlistEditViewModel watchlistEditViewModel, long[] movieIds)
        {
            if (ModelState.IsValid)
            {
                var watchlist = _watchlistRepository.Watchlists.Include(w => w.Movies).FirstOrDefault(w => w.Id == watchlistEditViewModel.Id);
                if (watchlist == null)
                {
                    return NotFound();
                }

                watchlist.Name = watchlistEditViewModel.Name;
                watchlist.Description = watchlistEditViewModel.Description;
                watchlist.Movies = _movieRepository.Movies.Where(m => movieIds.Contains(m.Id)).ToList();
                watchlist.UpdatedDate = DateTime.Now;
                _watchlistRepository.UpdateWatchlist(watchlist);
                return RedirectToAction("Details", new { id = watchlist.Id });
            }
            ViewBag.MovieOptions = _movieRepository.Movies.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Title
            }).ToList();

            return View(watchlistEditViewModel);
        }


        public IActionResult Delete(long id)
        {
            var watchlist = _watchlistRepository.Watchlists.FirstOrDefault(w => w.Id == id);
            if (watchlist == null)
            {
                return NotFound();
            }
            _watchlistRepository.DeleteWatchlist(watchlist);
            return RedirectToAction("Index");
        }
    }
}