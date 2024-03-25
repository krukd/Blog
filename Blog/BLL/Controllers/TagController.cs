using Blog.DAL.Data.Repositories;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagViewModel addTagViewModel)
        {
            var tag = new Tag
            {
                Content = addTagViewModel.Content,
                
            };

            await _repo.Add(tag);

            return RedirectToAction("List");
        }


        [Authorize]
        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            var tags = await _repo.GetAll();
            return View(tags);
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var tag = await _repo.Get(id);
            await _repo.Delete(tag);

            return RedirectToAction("List");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var tag = await _repo.Get(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagViewModel
                {
                    Id = id,
                    Content = tag.Content,
                   
                };
                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagViewModel editTagViewModel)
        {
            var tag = new Tag
            {
                Id = editTagViewModel.Id,
                Content = editTagViewModel.Content,
               
            };

            _repo.Update(tag);


            return RedirectToAction("List");
        }

        

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var tag = await _repo.Get(id);

            return View(tag);
        }
    }
}
