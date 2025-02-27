using BusinessLogicLayer.Model.TagModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service.Contracts
{
    public interface ITagService
    {
        Task<TagDTO> GetByIDAsync(int id);
        Task CreateAsync(TagCreateDTO tagCreateDTO);
        Task UpdateAsync(TagUpdateDTO tagUpdateDTO);
        Task<List<TagDTO>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}
