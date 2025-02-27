using DataAccessLayer.DatabaseConfiguration;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repositories.Interface;
using DataAccessLayer.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly NewsManagementDB _context;

		public ISystemAccountRepository SystemAccounts { get; }
		public ICategoryRepository Categories { get; }
		public INewsArticleRepository NewsArticles { get; }
		public INewsTagRepository NewsTags { get; }
		public ITagRepository Tags { get; }

		public UnitOfWork(
			NewsManagementDB context,
			ISystemAccountRepository systemAccountRepository,
			ICategoryRepository categoryRepository,
			INewsArticleRepository newsArticleRepository,
			INewsTagRepository newsTagRepository,
			ITagRepository tagRepository)
		{
			_context = context;
			SystemAccounts = systemAccountRepository;
			Categories = categoryRepository;
			NewsArticles = newsArticleRepository;
			NewsTags = newsTagRepository;
			Tags = tagRepository;
		}

		public async Task<int> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}

}
