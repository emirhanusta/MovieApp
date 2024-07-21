using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieApp.Data.Abstract;
using MovieApp.Data.Concrete.Context;
using MovieApp.Entities;

namespace MovieApp.Controllers
{
    public class DirectorController : Controller
    {

        private readonly IDirectorRepository _directorRepository;

        public DirectorController(IDirectorRepository directorRepository)
        {
            _directorRepository = directorRepository;
        }

        [Authorize (Roles = "admin")]
        public IActionResult Index(string? searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var directors = from d in _directorRepository.Directors
                            select d;

            if (!string.IsNullOrEmpty(searchString))
            {
                directors = directors.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
            }

            return View(directors);
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _directorRepository.Directors.Include(m => m.Movies).FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [Authorize (Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize (Roles = "admin")]
        public IActionResult Create(Director director, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var imageFileName = SaveImageAsync(imageFile).Result;

                director.Image = imageFileName;
                _directorRepository.AddDirector(director);
                return RedirectToAction("Index", "Home");
            }
            return View(director);
        }

        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = await _directorRepository.Directors.Where(d => d.Id == id).FirstOrDefaultAsync();
            if (director == null)
            {
                return NotFound();
            }
            return View(director);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(Director director, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                var directorToUpdate = _directorRepository.Directors.FirstOrDefault(d => d.Id == director.Id);
                if (imageFile != null)
                {
                    var imageFileName = SaveImageAsync(imageFile).Result;
                    directorToUpdate.Image = imageFileName;
                }
                directorToUpdate.Name = director.Name;
                directorToUpdate.Biography = director.Biography;
                _directorRepository.UpdateDirector(directorToUpdate);
                return RedirectToAction("Index");
            }
            return View(director);
        }

        [Authorize]
        public async Task<IActionResult> Delete(long? id)
        {
            var director = _directorRepository.Directors.FirstOrDefault(w => w.Id == id);
            if (director == null)
            {
                return NotFound();
            }
            _directorRepository.DeleteDirector(director);
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