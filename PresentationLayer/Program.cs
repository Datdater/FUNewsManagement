using BusinessLogicLayer.MapperProfile;
using BusinessLogicLayer.Service;
using BusinessLogicLayer.Service.Contracts;
using DataAccessLayer.DatabaseConfiguration;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interface;
using DataAccessLayer.UnitOfWork;
using DataAccessLayer.UnitOfWork.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.MapperProfile;
using System.ComponentModel;

namespace PresentationLayer
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			//DI in DAL
			var conString = builder.Configuration.GetConnectionString("FUNewsManagement") ??
					   throw new InvalidOperationException("Connection string 'FUNewsManagement' not found.");
			builder.Services.AddDbContext<NewsManagementDB>(options => options.UseSqlServer(conString));
			builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();
			builder.Services.AddScoped<ITagRepository, TagRepository>();
			builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
			builder.Services.AddScoped<INewsTagRepository, NewsTagRepository>();
			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            //DI in BLL
            builder.Services.AddAutoMapper(typeof(FileMapper));
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(MapperModel));

            builder.Services.AddControllersWithViews();

			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.LoginPath = "/Account/login";
					options.AccessDeniedPath = "/Account/login";
				});
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
			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=newsarticle}/{action=GetAll}/{id?}");

			app.Run();
		}
	}
}
