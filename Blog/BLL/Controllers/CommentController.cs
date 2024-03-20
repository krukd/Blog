using Blog.DAL.Data.Repositories;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.BLL.Controllers
{
    public class CommentController : Controller
    {
        private readonly ILogger<CommentController> _logger;
        private readonly IRepository<Comment> _repo;

        public CommentController(IRepository<Comment> repo, ILogger<CommentController> logger)
        {
            _repo = repo;
            _logger = logger;
        }


        [Authorize]
        [Route("Comment/Create")]
        public async Task<IActionResult> CreateComment()
        {

            var newComment = new Comment
            {
                Content = "Comment 3",
                PostedDate = DateTime.Now,
                UserId = 2,
                ArticleId = 2
            };

            _repo.Add(newComment);


            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        [Route("Comment/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _repo.Get(id);
            if (comment != null)
            {
                await _repo.Delete(comment);
            }

            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        [Route("Comment/Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var comment = await _repo.Get(id);
            comment.Content = "Comment updated";
            comment.PostedDate = DateTime.Now;
            comment.UserId = 3;
            comment.ArticleId = 3;

            await _repo.Update(comment);

            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        [Route("Comments")]
        public async Task<IActionResult> Index()
        {
            var comments = await _repo.GetAll();
            return View(comments);
        }

        [Authorize]
        [HttpGet]
        [Route("Comments/{id}")]
        public async Task<IActionResult> Index_2(int id)
        {
            var comment = await _repo.Get(id);

            return View(comment);
        }


    }
}
