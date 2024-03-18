using Blog.DAL.Data.DB;
using Blog.DAL.Data.Repositories;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Controllers
{
    public class UserController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _repo;


        public UserController(ILogger<HomeController> logger, IUserRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet]
        [Route("Register")]
        public async Task<IActionResult> Register()
        {

            //var newUser = new User
            //{
            //    FirstName = "Dima",
            //    LastName = "Smirnov",
            //    Email = "test3@test.com",
            //    Password = "123",
            //    RoleId = 3,
            //};

            //_repo.Add(newUser);


            //return RedirectToAction("Index", "Home");

            return View();
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
