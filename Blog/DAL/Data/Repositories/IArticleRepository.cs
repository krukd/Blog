using Blog.DAL.Models;

namespace Blog.DAL.Data.Repositories
{
    public interface IArticleRepository
    {
        Task Add(Article item);
        Task<Article> Get(int id);
        Task<IEnumerable<Article>> GetAll();
        Task Update(Article item);
        Task Delete(Article item);
    }
}
