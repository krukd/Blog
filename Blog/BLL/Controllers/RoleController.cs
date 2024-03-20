using Blog.DAL.Data.Repositories;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.BLL.Controllers
{

    public class RoleController : Controller
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRepository<Role> _repo;


        public RoleController(IRepository<Role> repo, ILogger<RoleController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [Authorize]
        [Route("Role/Create")]
        public async Task<IActionResult> CreateRole()
        {

            var newRole = new Role
            {
                //Name = "Администратор",
                //Description = "Blog's administrator"
                //Name = "Модератор",
                //Description = "Blog's moderator"
                //Name = "Пользователь",
                //Description = "Blog's user"
            };

            _repo.Add(newRole);


            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        [Route("Role/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _repo.Get(id);
            if (role != null)
            {
                await _repo.Delete(role);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [Route("Role/Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var role = await _repo.Get(id);
            role.Name = "";
            role.Description = "";

            await _repo.Update(role);

            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        [Route("Roles")]
        public async Task<IActionResult> Index()
        {
            var roles = await _repo.GetAll();
            return View(roles);
        }

        [Authorize]
        [HttpGet]
        [Route("Roles/{id}")]
        public async Task<IActionResult> Index_2(int id)
        {
            var role = await _repo.Get(id);

            return View(role);
        }

    }
}
