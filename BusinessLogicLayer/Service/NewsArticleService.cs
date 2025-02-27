using AutoMapper;
using BusinessLogicLayer.Model.NewsArticleModel;
using BusinessLogicLayer.Service.Contracts;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interface;
using DataAccessLayer.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public NewsArticleService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(NewsArticleCreateDTO newsDto)
        {
            var newsArticle = _mapper.Map<NewsArticle>(newsDto);
            newsArticle.NewsStatus = true;
            newsArticle.CreatedDate = DateTime.UtcNow;
            newsArticle.ModifiedDate = DateTime.UtcNow;
            newsArticle.UpdatedBy = null;
            //add all tags
            List<NewsTag> newsTags = new List<NewsTag>();
            foreach (var item in newsDto.TagIds)
            {
                var tag = await _unitOfWork.Tags.GetByIdAsync(item);
                var newsTag = new NewsTag() { NewsArticle = newsArticle, Tag = tag };
                newsTags.Add(newsTag);
            }
            newsArticle.NewsTags = newsTags;

            //save changes
            await _unitOfWork.NewsArticles.AddAsync(newsArticle);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existingArticle = await _unitOfWork.NewsArticles.GetByIdAsync(id);
            await _unitOfWork.NewsArticles.RemoveAsync(existingArticle);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<NewsArticleDTO>> GetAllAsync()
        {
            var listNewsArticle = await _unitOfWork.NewsArticles.Include(x => x.CreatedBy).Include(x => x.UpdatedBy).Include(x =>x.Category).GetAllAsync();
            var listNewsArticleList = listNewsArticle.ToList();
            return _mapper.Map<List<NewsArticleDTO>>(listNewsArticleList);
        }

        public async Task<NewsArticleDTO> GetByIdAsync(int id)
        {
            var existingNews = await _unitOfWork.NewsArticles
                .Include(m => m.Category)
                .Include(n => n.CreatedBy)
                .GetByIdAsync(id);
            var newsTags = await _unitOfWork.NewsTags.FindAllAsync(x => x.NewsArticleID == id); // Should return a collection
            var tagIds = newsTags.Select(nt => nt.TagID).ToList(); // Extract the Tag IDs
            var tagsList = await _unitOfWork.Tags.FindAllAsync(t => tagIds.Contains(t.TagID));

            var news = _mapper.Map<NewsArticleDTO>(existingNews);
            news.Tags = tagsList.Select(x => x.TagName).ToList();
            news.TagIds = tagsList.Select(x => x.TagID).ToList();
            return news;
        }

        public async Task<NewsArticleDTO> UpdateAsync(NewsArticleUpdateDTO updateDto)
        {
            var existingArticle = await _unitOfWork.NewsArticles.Include(n => n.NewsTags).GetByIdAsync(updateDto.NewsArticleID);
            _mapper.Map(updateDto, existingArticle);

            // current 
            var currentTagIds = existingArticle.NewsTags.Select(nt => nt.TagID).ToList();

            //  remove tag
            var tagsToRemove = existingArticle.NewsTags.Where(nt => !updateDto.TagIds.Contains(nt.TagID)).ToList();
            foreach (var newsTag in tagsToRemove)
            {
                existingArticle.NewsTags.Remove(newsTag);
            }

            // Add tag
            foreach (var tagId in updateDto.TagIds)
            {
                if (!currentTagIds.Contains(tagId))
                {
                    var tag = await _unitOfWork.Tags.GetByIdAsync(tagId);
                    if (tag != null)
                    {
                        existingArticle.NewsTags.Add(new NewsTag { NewsArticle = existingArticle, Tag = tag });
                    }
                }
            }

            await _unitOfWork.NewsArticles.UpdateAsync(existingArticle);
            await _unitOfWork.SaveChangesAsync();
            var updatedNewsArticle = await GetByIdAsync(existingArticle.NewsArticleID);
            return _mapper.Map<NewsArticleDTO>(updatedNewsArticle);
        }

       
    }
}
