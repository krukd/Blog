using Blog.DAL.Data.Repositories;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.BLL.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ILogger<ArticleController> _logger;
        private readonly IArticleRepository _repo;


        public ArticleController(IArticleRepository repo, ILogger<ArticleController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [Route("Article/Create")]
        public async Task<IActionResult> CreateArticle()
        {

            var newArticle = new Article
            {
                Title = "New Article 2",
                Content = "This is a new article content.",
                PublishedDate = DateTime.Now,
                UserId = 2 
            };

            _repo.Add(newArticle);


            return RedirectToAction("Index", "Home");
        }

        // DELETE: User/Delete/1

        [Route("Article/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _repo.Get(id);
            if (article != null)
            {
                await _repo.Delete(article);
            }

            return RedirectToAction("Index", "Home");
        }


        [Route("Article/Update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var article = await _repo.Get(id);
            article.Title = "Article updated";
            article.Content = "New content";
            article.PublishedDate = DateTime.Now;
            article.UserId = 3;

            await _repo.Update(article);

            return RedirectToAction("Index", "Home");
        }

        //[Route("Users")]
        //public IActionResult Index()
        //{
        //    var users = _repo.GetAll();
        //    return View(users);
        //}


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
