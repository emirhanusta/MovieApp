using System.Security.Claims;
using MovieApp.Data.Abstract;
using MovieApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace MovieApp.Controllers
{

    public class UsersController : Controller
    {

        private UserManager<User> _userManager;
        private RoleManager<Role> _roleManager;
        private SignInManager<User> _signInManager;
        public UsersController(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        await _userManager.SetLockoutEndDateAsync(user, null);

                        return RedirectToAction("Index", "Home");
                    }
                    else if (result.IsLockedOut)
                    {
                        var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                        var timeLeft = lockoutDate.Value - DateTime.UtcNow;
                        ModelState.AddModelError("", $"Your account is locked out. Please try again in {timeLeft.Minutes} minutes.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Wrong password");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "user not found");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Name = model.FullName,
                    CreatedDate = DateTime.Now,
                    Image = "avatar.jpg"
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                _userManager.AddToRoleAsync(user, "User").Wait();
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Edit(string id)
        {

            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {

                ViewBag.Roles = await _roleManager.Roles.Select(i => i.Name).ToListAsync();
                return View(new UserEditViewmodel
                {
                    Id = user.Id,
                    FullName = user.Name,
                    Email = user.Email,
                    Image = user.Image
                });
            }
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Edit(long id, UserEditViewmodel model, IFormFile? imageFile)
        {
            if (id != model.Id)
            {
                return RedirectToAction("Index");
            }
            var userName = "";
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user != null)
                {
                    userName = user.UserName;
                    user.Email = model.Email;
                    user.Name = model.FullName;
                    if (imageFile != null)
                    {
                        user.Image = await SaveImageAsync(imageFile);
                    }

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded && !string.IsNullOrEmpty(model.Password))
                    {
                        await _userManager.RemovePasswordAsync(user);
                        await _userManager.AddPasswordAsync(user, model.Password);
                    }
                    foreach (IdentityError err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return RedirectToAction("Profile", new { username = userName });
        }

        public IActionResult Profile(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }
            var user = _userManager
            .Users
            .Include(x => x.Reviews)
                .ThenInclude(x => x.Movie)
            .Include(x => x.Reviews)
                .ThenInclude(x => x.Likes)
            .Include(x => x.Likes)
                .ThenInclude(x => x.Review)
                    .ThenInclude(x => x.Movie)
            .Include(x => x.Likes)
                .ThenInclude(x => x.Review)
                    .ThenInclude(x => x.User)
            .FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                return NotFound();
            }
            ViewBag.Username = username;
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Register");
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