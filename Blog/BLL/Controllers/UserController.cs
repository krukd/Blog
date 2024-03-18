using Blog.DAL.Data.DB;
using Blog.DAL.Data.Repositories;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blog.BLL.Controllers
{
    public class UserController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _repo;
        private readonly IRepository<Role> _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserController(ILogger<HomeController> logger, IUserRepository repo, IRepository<Role> repository, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _repo = repo;
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }


        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _repo.GetByLogin(email);
            if (user != null)
            {
                // Успешная аутентификация
                // Добавляем роли пользователя в клаймы
                string role = user.RoleId.ToString();
                List<Claim> claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Email),
                            new Claim(ClaimTypes.Role, role),
                        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false 
                };

                await _httpContextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View();
            }
        }

        [Route("Logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([Bind("Id,FirstName,LastName,Email,Password")] User newUser)
        {
            if (ModelState.IsValid)
            {
                // Присвоение базовой роли "Пользователь"
                var baseRole = await _repository.Get(3); // Получить роль "Пользователь" из репозитория
                newUser.Roles.Add(baseRole); // Добавить роль к пользователю

                await _repo.Add(newUser);

                return View(newUser);

            }
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine($"Ошибка валидации: {error.ErrorMessage}");
                }
            }

            // Вывод параметров модели, которые не прошли валидацию
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                if (state.Errors.Any())
                {
                    Console.WriteLine($"Параметр: {key}");
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"  - Ошибка: {error.ErrorMessage}");
                    }
                }
            }

            return View(newUser);
        }

        // DELETE: User/Delete/1

        [Route("User/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _repo.Get(id);
            if (user != null)
            {
                await _repo.Delete(user);
            }

            return RedirectToAction("Index", "Home");
        }


        [Route("User/Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var user = await _repo.Get(id);
            user.FirstName = "John";
            user.LastName = "Puprkin";
            user.Email = "email@mail.ru";
            user.Password = "123456";

            await _repo.Update(user);

            return RedirectToAction("Index", "Home");
        }




        [Route("Users")]
        public async Task<IActionResult> Index()
        {
            var users = await _repo.GetAll();
            return View(users);
        }


        [HttpGet]
        [Route("Users/{id}")]
        public async Task<IActionResult> Index_2(int id)
        {
            var user = await _repo.Get(id);

            return View(user);
        }
    }

    

    
}
