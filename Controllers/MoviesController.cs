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
        private readonly IReviewRepository _reviewRepository;

        public MoviesController(IMovieRepository context, IGenreRepoository genreRepository, IDirectorRepository directorRepository, IActorRepository actorRepository, IReviewRepository reviewRepository)
        {
            _movieRepository = context;
            _genreRepository = genreRepository;
            _directorRepository = directorRepository;
            _actorRepository = actorRepository;
            _reviewRepository = reviewRepository;
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
        public async Task<IActionResult> Create(MovieCreateViewModel model, long[] genreIds, long[] actorIds, IFormFile imageFile)
        {
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
                var imagePath = await SaveImageAsync(imageFile);
                var movie = new Movie
                {
                    Title = model.Title,
                    Description = model.Description,
                    ReleaseDate = model.ReleaseDate,
                    DirectorId = model.DirectorId,
                    Image = imagePath,
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddReview(long movieId, int rating, string content)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var review = new Review
                {
                    MovieId = movieId,
                    UserId = long.Parse(userId),
                    Rating = rating,
                    Content = content,
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now)
                };

                // Save the review to the database
                _reviewRepository.SaveReview(review);

                // Redirect back to the movie details page
                return RedirectToAction("Details", new { id = movieId });
            }

            return RedirectToAction("Login", "Users"); // Redirect to login if the user is not authenticated
        }


        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
            var randomFileName = string.Empty;

            if (imageFile != null && imageFile.Length > 0)
            {
                var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Please select a valid image file.");
                }
                else
                {
                    randomFileName = $"{Guid.NewGuid()}{extension}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                    try
                    {
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("ImageFile", "An error occurred while uploading the file.");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Please select an image file.");
            }

            return randomFileName;
        }

    }
}