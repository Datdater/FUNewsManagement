using BusinessLogicLayer.Model.CategoryModel;
using BusinessLogicLayer.Model.TagModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service.Contracts
{
    public interface ICategoryService
    {
        Task<CategoryDTO> GetByIDAsync(int id);
        Task CreateAsync(CategoryCreateDTO tagCreateDTO);
        Task UpdateAsync(CategoryUpdateDTO tagUpdateDTO);
        Task<List<CategoryDTO>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}
