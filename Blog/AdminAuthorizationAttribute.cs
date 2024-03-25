using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog
{
    public class AdminAuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            // Получаем роль пользователя из клайма
            var roleClaim = user.FindFirst(ClaimTypes.Role);
            if (roleClaim == null || roleClaim.Value != "4")
            {
                context.Result = new ForbidResult(); // Отказываем в доступе
            }
        }
    }
}
