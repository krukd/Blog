using Blog.DAL.Data.DB;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blog.BLL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //// —оздание и добавление нового пользовател€ при обращении к главной странице
            //var newUser = new User
            //{
            //    //UserName = "dk", // ”кажите нужное им€ пользовател€
            //    Email = "test@mail.ru" // ”кажите нужный email пользовател€

            //};

            //_context.Users.Add(newUser);
            //_context.SaveChanges();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
