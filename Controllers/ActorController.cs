using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieApp.Data.Abstract;
using MovieApp.Data.Concrete.Context;
using MovieApp.Entities;
using Microsoft.AspNetCore.Authorization;

namespace MovieApp.Controllers
{
    public class ActorController : Controller
    {
        private readonly IActorRepository _actorRepository;

        public ActorController(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }

        [Authorize (Roles = "admin")]
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var actors = from a in _actorRepository.Actors
                         select a;

            if (!string.IsNullOrEmpty(searchString))
            {
                actors = actors.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
            }

            return View(await actors.ToListAsync());
        }
        
        public async Task<IActionResult> Details(long? id)
        {
            var actor = await _actorRepository.Actors.Include(a => a.Movies).FirstOrDefaultAsync(a => a.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }
    
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(Actor actor, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var imageFileName = SaveImageAsync(imageFile).Result;

                actor.Image = imageFileName;
                _actorRepository.AddActor(actor);
                return RedirectToAction("Index", "Home");
            }
            return View(actor);
        }

        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actor = await _actorRepository.Actors.Where(d => d.Id == id).FirstOrDefaultAsync();
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(Actor actor, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                var actorToUpdate = _actorRepository.Actors.FirstOrDefault(d => d.Id == actor.Id);
                if (imageFile != null)
                {
                    var imageFileName = SaveImageAsync(imageFile).Result;
                    actorToUpdate.Image = imageFileName;
                }
                actorToUpdate.Name = actor.Name;
                actorToUpdate.Biography = actor.Biography;
                _actorRepository.UpdateActor(actorToUpdate);
                return RedirectToAction("Index");
            }
            return View(actor);
        }

        [Authorize]
        public IActionResult Delete(long? id)
        {
            var actor = _actorRepository.Actors.FirstOrDefault(w => w.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            _actorRepository.DeleteActor(actor);
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