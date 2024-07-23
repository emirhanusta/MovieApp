using System.Security.Claims;
using MovieApp.Data.Abstract;
using MovieApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Entities;
using Microsoft.AspNetCore.Authorization;

namespace MovieApp.Controllers
{

    public class UsersController : Controller
    {

        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
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

                var user = await _userRepository.Users.FirstOrDefaultAsync(x => x.Username == model.Username || x.Email == model.Email);
                if (user == null)
                {
                    _userRepository.AddUser(new User
                    {
                        Username = model.Username,
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        Image = "avatar.jpg"
                    });
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "UserName ya da Email adresi kullanımda.");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isUser = await _userRepository.Users.FirstOrDefaultAsync(x => x.Email == model.Email && x.Password == model.Password);
                if (isUser != null)
                {
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, isUser.Id.ToString()),
                        new Claim(ClaimTypes.Name, isUser.Username ?? ""),
                        new Claim(ClaimTypes.GivenName, isUser.Name ?? ""),
                        new Claim(ClaimTypes.UserData, isUser.Image ?? "")
                    };

                    if (isUser.Email == "info@emirhanusta.com")
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                    }

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya parola hatalı emin miyiz!");
                }
            }
            return View(model);
        }

        [Authorize]
        public IActionResult Profile(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return NotFound();
            }
            var user = _userRepository
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
            .FirstOrDefault(x => x.Username == username);

            if (user == null)
            {
                return NotFound();
            }
            ViewBag.Username = username;
            return View(user);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var user = _userRepository.Users.FirstOrDefault(x => x.Id == id);
            if (user != null)
            {
                return View(user);
            }
            return NotFound();
        }
        

        public IActionResult Delete(int id)
        {
            var user = _userRepository.Users.FirstOrDefault(x => x.Id == id);
            Logout();
            if (user != null)
            {
                _userRepository.DeleteUser(user);
                return RedirectToAction("Login");
            }
            return NotFound();
        }
    }
}