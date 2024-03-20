using Blog.DAL.Data.DB;
using Blog.DAL.Data.Repositories;
using Blog.DAL.Models;
using Blog.DAL.Data.UoW;
using Microsoft.EntityFrameworkCore;
using System;
using Blog.Extensions;
using System.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Blog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")))
            .AddUnitOfWork()
            .AddCustomRepository<Comment, CommentRepository>()
            .AddCustomRepository<Role, RoleRepository>();



            builder.Services
            .AddTransient<IUserRepository, UserRepository>()
             .AddTransient<IArticleRepository, ArticleRepository>()
             .AddCustomRepository<Tag, TagRepository>();

            builder.Services.AddHttpContextAccessor();

            // Добавляем аутентификацию
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Время жизни куки
                    options.LoginPath = "/Login"; // Путь для перенаправления на страницу входа
                    options.AccessDeniedPath = "/AccessDenied"; // Путь для перенаправления на страницу доступа запрещен
                    options.SlidingExpiration = true; // Обновлять время жизни куки при каждом запросе
                });



            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();



            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

