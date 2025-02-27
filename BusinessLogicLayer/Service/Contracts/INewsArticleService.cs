using BusinessLogicLayer.Model.NewsArticleModel;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service.Contracts
{
    public interface INewsArticleService
    {
        Task<NewsArticleDTO> GetByIdAsync(int id);
        Task CreateAsync(NewsArticleCreateDTO newsDto);
        Task<NewsArticleDTO> UpdateAsync(NewsArticleUpdateDTO newsDto);
        Task<List<NewsArticleDTO>> GetAllAsync();
        Task DeleteAsync(int id);

    }
}
