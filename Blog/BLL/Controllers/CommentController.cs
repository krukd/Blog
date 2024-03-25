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

        public CommentController(IRepository<Comment> repo, ILogger<CommentController> logger)
        {
            _repo = repo;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _repo.GetAll();

            return View(comments);
        }


        [HttpGet]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await _repo.Get(id);

            return View(comment);
        }


        //[HttpGet]
        //public IActionResult Add()
        //{

        //    return View();
        //}


        //[HttpPost]
        [Route("Comment/Add")]
        public async Task<IActionResult> Add()
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment
                {
                    Content = "HTML",
                    PostedDate = DateTime.Now,
                    UserId = 12,
                    ArticleId = 1,
                };

                await _repo.Add(comment);

                return RedirectToAction("Index", "Home");
            }
            else
            {
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
                // Если модель не прошла валидацию, вернуть представление с сообщением об ошибке
                return RedirectToAction("Index", "Home");
            }

            //// Проверка модели на валидность
            //if (ModelState.IsValid)
            //{
            //    // Добавление комментария в базу данных (псевдокод)
            //    _repo.Add(newComment);

            //    // Перенаправление пользователя на другую страницу после успешного добавления
            //    return RedirectToAction("Index", "Home");
            //}
            //else
            //{
            //    // Вывод параметров модели, которые не прошли валидацию
            //    foreach (var key in ModelState.Keys)
            //    {
            //        var state = ModelState[key];
            //        if (state.Errors.Any())
            //        {
            //            Console.WriteLine($"Параметр: {key}");
            //            foreach (var error in state.Errors)
            //            {
            //                Console.WriteLine($"  - Ошибка: {error.ErrorMessage}");
            //            }
            //        }
            //    }
            //    // Если модель не прошла валидацию, вернуть представление с сообщением об ошибке
            //    return View(newComment);
            //}
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _repo.Get(id);
            await _repo.Delete(role);

            return RedirectToAction("Index", "Comments");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var comment = await _repo.Get(id);
            _logger.LogInformation("CommentsController - Update");
            return View(comment);
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmUpdating(Comment comment)
        {
            await _repo.Update(comment);
            _logger.LogInformation("CommentsController - Update - complete");
            return RedirectToAction("Index", "Comments");
        }

    }
}


