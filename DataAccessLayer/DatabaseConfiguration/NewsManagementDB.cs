using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DatabaseConfiguration
{
	public class NewsManagementDB : DbContext
	{
		public DbSet<SystemAccount> SystemAccount { get; set; }
		public DbSet<Category> Category { get; set; }
		public DbSet<NewsTag> NewsTag { get; set; }
		public DbSet<NewsArticle> NewsArticle { get; set; }
		public DbSet<Tag> Tag { get; set; }

		public NewsManagementDB(DbContextOptions options) : base(options) 
		{ 
			//Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<NewsTag>()
			.HasKey(nt => new { nt.NewsArticleID, nt.TagID });
			modelBuilder.Entity<NewsArticle>()
				.HasOne(n => n.CreatedBy) //one newsarticle will have 1 system
				.WithMany(a => a.CreatedNewsArticles)
				.HasForeignKey(n => n.CreatedByID)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<NewsArticle>()
				.HasOne(n => n.UpdatedBy)
				.WithMany(a => a.UpdatedNewsArticles)
				.HasForeignKey(n => n.UpdatedByID)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<NewsArticle>()
				.HasMany(a => a.NewsTags)
				.WithOne(n => n.NewsArticle)
				.HasForeignKey(n => n.NewsArticleID);

			modelBuilder.Entity<Tag>()
				.HasMany(n => n.NewsTags)
				.WithOne(a => a.Tag)
				.HasForeignKey(n => n.TagID);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, CategoryName = "Technology", CategoryDescription = "Latest tech news and trends", ParentCategoryID = null, IsActive = true },
                new Category { CategoryID = 2, CategoryName = "Sports", CategoryDescription = "All about sports", ParentCategoryID = null, IsActive = true },
                new Category { CategoryID = 3, CategoryName = "AI", CategoryDescription = "Artificial Intelligence updates", ParentCategoryID = 1, IsActive = true }
            );

            // Seed System Accounts
            modelBuilder.Entity<SystemAccount>().HasData(
                new SystemAccount { AccountID = 1, AccountName = "Huynh Hai Hong", AccountEmail = "lecture@gmail.com", AccountRole = "2", AccountPassword = "123456" },
                new SystemAccount { AccountID = 2, AccountName = "Nguyen Lu Bo", AccountEmail = "lubo@gmail.com", AccountRole = "1", AccountPassword = "123456" }
            );

            // Seed Tags
            modelBuilder.Entity<Tag>().HasData(
                new Tag { TagID = 1, TagName = "Breaking", Note = "Important breaking news" },
                new Tag { TagID = 2, TagName = "Trending", Note = "Currently trending topics" }
            );

            
        }

	}
}
