using Blog.DAL.Data.Repositories;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.BLL.Controllers
{
    public class TagController : Controller
    {
        private readonly ILogger<TagController> _logger;
        private readonly IRepository<Tag> _repo;

        public TagController(IRepository<Tag> repo, ILogger<TagController> logger)
        {
            _repo = repo;
            _logger = logger;
        }


        [Route("Tag/Create")]
        public async Task<IActionResult> CreateTag()
        {

            var newTag = new Tag
            {
                Content = "C#",
            };

            _repo.Add(newTag);


            return RedirectToAction("Index", "Home");
        }



        [Route("Tag/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tag = await _repo.Get(id);
            if (tag != null)
            {
                await _repo.Delete(tag);
            }

            return RedirectToAction("Index", "Home");
        }


        [Route("Tag/Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var tag = await _repo.Get(id);
            tag.Content = "C#";
           
            await _repo.Update(tag);

            return RedirectToAction("Index", "Home");
        }

        [Route("Tags")]
        public async Task<IActionResult> Index()
        {
            var tags = await _repo.GetAll();
            return View(tags);
        }


        [HttpGet]
        [Route("Tags/{id}")]
        public async Task<IActionResult> Index_2(int id)
        {
            var tag = await _repo.Get(id);

            return View(tag);
        }
    }
}
