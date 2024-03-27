using Blog.DAL.Data.Repositories;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.BLL.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ILogger<ArticleController> _logger;
        private readonly IArticleRepository _repo;
        private readonly IRepository<Tag> _tagRepo;
        private readonly IUserRepository _userRepo;


        public ArticleController(IArticleRepository article_repo, IRepository<Tag> tag_repo, IUserRepository user_repo, ILogger<ArticleController> logger)
        {
            _repo = article_repo;
            _tagRepo = tag_repo;
            _userRepo = user_repo;
            _logger = logger;
        }



        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await _tagRepo.GetAll();
           
            return View(new AddArticleViewModel() { Tags = tags.ToList() });
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(AddArticleViewModel model, List<int> SelectedTags)
        {
            // Получаем логин текущего пользователя из контекста сессии
            string? currentUserLogin = User?.Identity?.Name;
            var user = _userRepo.GetByLogin(currentUserLogin);

            var tags = new List<Tag>();

            SelectedTags.ForEach(async id => tags.Add(await _tagRepo.Get(id)));

            var article = new Article
            {
                UserId = user.Id,
                Author = user,
                PublishedDate = DateTime.Now,
                Title = model.Title,
                Content = model.Content,
                Tags = tags
            };

            await _repo.Add(article);

            return RedirectToAction("GetAll", "Article");
        }





        [Authorize]
        [HttpGet]
        [ActionName("GetAll")]
        public async Task<IActionResult> GettAll()
        {
            var articles = await _repo.GetAll();
            return View(articles);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var article = await _repo.Get(id);
            var tags = await _tagRepo.GetAll();
           

            return View(new EditArticleViewModel()
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                TagsSelected = article.Tags.ToList(),
                Tags = tags.ToList()
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditArticleViewModel model, List<int> SelectedTags)
        {
            string? currentUserLogin = User?.Identity?.Name;
            var user = _userRepo.GetByLogin(currentUserLogin);

            var tags = new List<Tag>();
            SelectedTags.ForEach(async id => tags.Add(await _tagRepo.Get(id)));

            var article = new Article
            {
                Id = model.Id,
                UserId = user.Id,
                Author = user,
                PublishedDate = DateTime.Now,
                Title = model.Title,
                Content = model.Content,
                Tags = tags
            };

            await _repo.Update(article);
           
            return RedirectToAction("GetAll", "Article");
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _repo.Get(id);
            await _repo.Delete(article);
            
            return RedirectToAction("GetAll", "Article");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var article = await _repo.Get(id);

            return View(article);
        }

        [Authorize]
        [HttpGet]
        [Route("Article/GetArticlesByAuthorId/{authorId}")]
        public async Task<IActionResult> GetArticlesByAuthorId(int authorId)
        {
            _logger.LogInformation("Trying to get articles for author with ID {AuthorId}", authorId);
            var articles = _repo.GetArticlesByAuthorId(authorId);

            _logger.LogInformation("Retrieved {Count} articles for author with ID {AuthorId}", articles.Count(), authorId);

            

            return View(articles);
        }
    }
}
