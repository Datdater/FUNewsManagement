using DataAccessLayer.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.UnitOfWork.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        ISystemAccountRepository SystemAccounts { get; }
        ICategoryRepository Categories { get; }
        INewsArticleRepository NewsArticles { get; }
        INewsTagRepository NewsTags { get; }
        ITagRepository Tags { get; }
        Task<int> SaveChangesAsync();
    }
}
