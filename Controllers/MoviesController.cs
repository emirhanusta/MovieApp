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
using MovieApp.Data.Abstract;
using MovieApp.Data.Concrete;
using MovieApp.Data.Concrete.Context;
using MovieApp.Entities;
using MovieApp.Models;

namespace MovieApp.Controllers
{
    public class MoviesController : Controller
    {

        private readonly IMovieRepository _movieRepository;
        private readonly IGenreRepoository _genreRepository;
        private readonly IDirectorRepository _directorRepository;
        private readonly IActorRepository _actorRepository;

        public MoviesController(IMovieRepository context, IGenreRepoository genreRepository, IDirectorRepository directorRepository, IActorRepository actorRepository)
        {
            _movieRepository = context;
            _genreRepository = genreRepository;
            _directorRepository = directorRepository;
            _actorRepository = actorRepository;
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _movieRepository.Movies.Include(m => m.Director).Include(m => m.Genres).Include(m => m.Actors).Include(m => m.Reviews).ThenInclude(r => r.User).ThenInclude(r => r.Likes).FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        public IActionResult Create()
        {
            ViewBag.Genres = _genreRepository.Genres.ToList();
            ViewBag.Directors = new SelectList(_directorRepository.Directors, "Id", "Name");
            ViewBag.Actors = _actorRepository.Actors.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MovieCreateViewModel model, long[] genreIds, long[] actorIds, IFormFile imageFile)
        {
            model.Image = ImagePath(imageFile);
            if (genreIds.Length == 0)
            {
                ModelState.AddModelError("Genres", "At least one genre must be selected.");
            }

            if (actorIds.Length == 0)
            {
                ModelState.AddModelError("Actors", "At least one actor must be selected.");
            }

            if (ModelState.IsValid)
            {
                var movie = new Movie
                {
                    Title = model.Title,
                    Description = model.Description,
                    ReleaseDate = model.ReleaseDate,
                    DirectorId = model.DirectorId,
                    Image = model.Image,
                    Genres = _genreRepository.Genres.Where(g => genreIds.Contains(g.Id)).ToList(),
                    Actors = _actorRepository.Actors.Where(a => actorIds.Contains(a.Id)).ToList()
                };

                _movieRepository.SaveMovie(movie);
                return RedirectToAction("Index", "Home"); // Redirect to the index or another appropriate view
            }

            ViewBag.Genres = _genreRepository.Genres.ToList();
            ViewBag.Directors = new SelectList(_directorRepository.Directors, "Id", "Name");
            ViewBag.Actors = _actorRepository.Actors.ToList();

            return View(model);
        }

        private string ImagePath(IFormFile imageFile)
        {
            var allowenExtensions = new[] { ".jpg", ".png", ".jpeg" };
            var randomfileName = string.Empty;
            if (imageFile != null)
            {
                var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                if (!allowenExtensions.Contains(extension))
                {
                    ModelState.AddModelError("", "Geçerli bir resim seçiniz.");
                }
                else
                {
                    randomfileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomfileName);

                    try
                    {
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            imageFile.CopyToAsync(stream);
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Dosya yüklenirken bir hata oluştu.");
                    }

                    if (!allowenExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("", "Geçerli bir resim seçiniz.");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Bir resim dosyası seçiniz.");
            }
            return randomfileName;
        }
    }
}