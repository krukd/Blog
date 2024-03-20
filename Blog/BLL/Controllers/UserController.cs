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

        [Authorize]
        [Route("MyPage")]
        public IActionResult MyPage()
        {
            // Получаем текущего аутентифицированного пользователя
            var user = _repo.GetByLogin(User.Identity.Name); 

            // Возвращаем представление 
            return View(user);
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

            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
                throw new ArgumentNullException("Incorrect credentials");

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
                

                return RedirectToAction("MyPage", "User");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View();
            }
        }

        [Authorize]
        [HttpGet]
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
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register([Bind("Id,FirstName,LastName,Email,Password")] User newUser)
        {
            if (ModelState.IsValid)
            {
                // Присвоение базовой роли "Пользователь"
                var baseRole = await _repository.Get(3); // Получить роль "Пользователь" из репозитория
                newUser.Roles.Add(baseRole); // Добавить роль к пользователю

                await _repo.Add(newUser);

                return RedirectToAction("Index", "Home");

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
        [AdminAuthorization]
        [Authorize]
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

        [Authorize]
        [Route("User/Update")]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var user = await _repo.Get(id);
            return View(user);
        }

        [Authorize]
        [Route("User/Update")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([Bind("Id,FirstName,LastName,Email,Password")] User user)
        {
            
            await _repo.Update(user);
            //return RedirectToAction("Index", "Home");
           return RedirectToAction("MyPage", "User");
        }



        [Authorize]
        [Route("Users")]
        public async Task<IActionResult> Index()
        {
            var users = await _repo.GetAll();
            return View(users);
        }

        [Authorize]
        [HttpGet]
        [Route("Users/{id}")]
        public async Task<IActionResult> Index_2(int id)
        {
            var user = await _repo.Get(id);

            return View(user);
        }
    }

    

    
}
