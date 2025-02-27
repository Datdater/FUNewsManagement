using AutoMapper;
using BusinessLogicLayer.Model.CategoryModel;
using BusinessLogicLayer.Service.Contracts;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(CategoryCreateDTO categpryCreateDTO)
        {
            var category = _mapper.Map<Category>(categpryCreateDTO);
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if(await CheckCategoryInNews(id))
            {
                await _unitOfWork.Categories.RemoveAsync(category);
                await _unitOfWork.SaveChangesAsync();
            } else
            {
                throw new Exception("Can't delete category because it has news");
            }
        }

        private async Task<bool> CheckCategoryInNews(int id)
        {
            return await _unitOfWork.NewsArticles.FindAsync(x => x.CategoryID == id) == null;
        }

        public async Task<List<CategoryDTO>> GetAllAsync()
        {
            var categoryListRaw = await _unitOfWork.Categories.Include(m => m.ParentCategory).GetAllAsync();
            var categoryList = categoryListRaw.ToList();
            return _mapper.Map<List<CategoryDTO>>(categoryList);

        }

        public async Task<CategoryDTO> GetByIDAsync(int id)
        {
            var categoryRaw = await _unitOfWork.Categories.GetByIdAsync(id);
            return _mapper.Map<CategoryDTO>(categoryRaw);
        }

        public async Task UpdateAsync(CategoryUpdateDTO categoryUpdateDTO)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryUpdateDTO.CategoryID);
            _mapper.Map(categoryUpdateDTO, category);
            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
