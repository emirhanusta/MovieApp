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
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserRepository _userRepository;

        public MoviesController(IMovieRepository context, IGenreRepoository genreRepository, IDirectorRepository directorRepository, IActorRepository actorRepository, IReviewRepository reviewRepository, IUserRepository userRepository)
        {
            _movieRepository = context;
            _genreRepository = genreRepository;
            _directorRepository = directorRepository;
            _actorRepository = actorRepository;
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
        }

        public IActionResult Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var movies = from m in _movieRepository.Movies
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.ToLower().Contains(searchString.ToLower()));
            }

            return View(movies);
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

        [Authorize]
        public IActionResult Create()
        {
            ViewBag.Genres = _genreRepository.Genres.ToList();
            ViewBag.Directors = new SelectList(_directorRepository.Directors, "Id", "Name");
            ViewBag.Actors = _actorRepository.Actors.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
        [Authorize]
        public IActionResult AddReview(long movieId, int rating, string content)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _userRepository.Users.FirstOrDefault(u => u.Id == long.Parse(userId));
                Console.WriteLine("review.user.Image " + user.Image);
                var review = new Review
                {
                    MovieId = movieId,
                    UserId = long.Parse(userId),
                    Rating = rating,
                    Content = content,
                    CreatedDate = DateTime.Now
                };

                _reviewRepository.SaveReview(review);
                return Json(new
                {
                    reviewId = review.Id,
                    userName = User.Identity.Name,
                    createdDate = review.CreatedDate.ToString("dd MMMM yyyy"),
                    rating = review.Rating,
                    content = review.Content,
                    user = new { Image = user.Image }
                });
            }

            return Unauthorized();
        }


        public async Task<IActionResult> NewMovies(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var movies = _movieRepository.Movies;

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.ToLower().Contains(searchString.ToLower()));
            }

            return View(await movies.OrderBy(m => m.CreatedDate).ToListAsync());
        }
        public async Task<IActionResult> PopularMovies(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var movies = _movieRepository.Movies;

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.ToLower().Contains(searchString.ToLower()));
            }

            return View(await movies.Include(m => m.Reviews).OrderByDescending(m => m.Reviews.Count).ToListAsync());
        }

        [Authorize (Roles = "admin")]
        public async Task<IActionResult> Edit(long id)
        {
            var movie = await _movieRepository.Movies.Include(m => m.Director).Include(m => m.Genres).Include(m => m.Actors).FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            var viewModel = new MovieUpdateViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                DirectorId = movie.DirectorId,
                GenreIds = movie.Genres.Select(g => g.Id).ToArray(),
                ActorIds = movie.Actors.Select(a => a.Id).ToArray(),
                Image = movie.Image
            };

            ViewBag.Directors = new SelectList(await _directorRepository.Directors.ToListAsync(), "Id", "Name");
            ViewBag.Genres = new SelectList(await _genreRepository.Genres.ToListAsync(), "Id", "Name");
            ViewBag.Actors = new SelectList(await _actorRepository.Actors.ToListAsync(), "Id", "Name");

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize (Roles = "admin")]
        public async Task<IActionResult> Edit(MovieUpdateViewModel model, long[] genreIds, long[] actorIds, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var movie = await _movieRepository.Movies.Include(m => m.Genres).Include(m => m.Actors).FirstOrDefaultAsync(m => m.Id == model.Id);
                    if (movie == null)
                    {
                        return NotFound();
                    }

                    if (imageFile != null)
                    {
                        var imagePath = await SaveImageAsync(imageFile);
                        movie.Image = imagePath;
                    }

                    movie.Title = model.Title;
                    movie.Description = model.Description;
                    movie.ReleaseDate = model.ReleaseDate;
                    movie.DirectorId = model.DirectorId;
                    movie.Genres = await _genreRepository.Genres.Where(g => model.GenreIds.Contains(g.Id)).ToListAsync();
                    movie.Actors = await _actorRepository.Actors.Where(a => model.ActorIds.Contains(a.Id)).ToListAsync();

                    _movieRepository.UpdateMovie(movie);
                }
                catch (Exception)
                {
                    return View(model);
                }
                return RedirectToAction(nameof(Index)); 
            }else{
                Console.WriteLine("Model state is not valid");
            }

            ViewBag.Directors = new SelectList(await _directorRepository.Directors.ToListAsync(), "Id", "Name");
            ViewBag.Genres = new SelectList(await _genreRepository.Genres.ToListAsync(), "Id", "Name");
            ViewBag.Actors = new SelectList(await _actorRepository.Actors.ToListAsync(), "Id", "Name");

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Delete(long? id)
        {
            var movie = _movieRepository.Movies.FirstOrDefault(w => w.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            _movieRepository.DeleteMovie(movie);
            return RedirectToAction("Index");

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