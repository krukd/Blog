using Blog.DAL.Data.Repositories;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog.BLL.Controllers
{
    public class CommentController : Controller
    {
        private readonly ILogger<CommentController> _logger;
        private readonly IRepository<Comment> _repo;
        private readonly IUserRepository _userRepo;

        public CommentController(IRepository<Comment> repo, ILogger<CommentController> logger, IUserRepository userRepo)
        {
            _repo = repo;
            _logger = logger;
            _userRepo = userRepo;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _repo.GetAll();
            
            return View(comments);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var comment = await _repo.Get(id);
           
            return View(comment);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(Comment newComment)
        {
            // Получаем логин текущего пользователя из контекста сессии
            string? currentUserLogin = User?.Identity?.Name;
            var user = _userRepo.GetByLogin(currentUserLogin);

            var comment = new Comment
            {               
                PostedDate = DateTime.Now,               
                Content = newComment.Content,             
            };

            
            await _repo.Add(newComment);

            return RedirectToAction("GetAll", "Comment");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _repo.Get(id);
            await _repo.Delete(role);
           
            return RedirectToAction("GetAll", "Comment");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var comment = await _repo.Get(id);
            
            return View(new EditCommentViewModel()
            {
                Id = comment.Id,
                Content = comment.Content,
                PostedDate = comment.PostedDate,
                UserId = comment.UserId,
                ArticleId = comment.ArticleId
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditCommentViewModel editCommentViewModel)
        {

            // Получаем логин текущего пользователя из контекста сессии
            string? currentUserLogin = User?.Identity?.Name;
            var user = _userRepo.GetByLogin(currentUserLogin);

            var comment = new Comment
            {
                Id = editCommentViewModel.Id,
                Content = editCommentViewModel.Content,
                PostedDate = editCommentViewModel.PostedDate,
                UserId = editCommentViewModel.UserId,
                ArticleId = editCommentViewModel.ArticleId
            };
            await _repo.Update(comment);
           
            return RedirectToAction("GetAll", "Comment");
        }

    }
}


