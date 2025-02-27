using AutoMapper;
using BusinessLogicLayer.Model.TagModel;
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
    public class TagService : ITagService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task CreateAsync(TagCreateDTO tagCreateDTO)
        {
            var tag = _mapper.Map<Tag>(tagCreateDTO);
            await _unitOfWork.Tags.AddAsync(tag);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(id);
            if(await _unitOfWork.NewsTags.FindAsync(x => x.TagID == id) != null)
            {
                throw new Exception("Can't delete because it has in News");
            };
            await _unitOfWork.Tags.RemoveAsync(tag);
        }

        public async Task<List<TagDTO>> GetAllAsync()
        {
            //throw new NotImplementedException();
            var tagRaw = await _unitOfWork.Tags.GetAllAsync();
            var tagList = tagRaw.ToList();
            return _mapper.Map<List<TagDTO>>(tagList);
        }

        public async Task<TagDTO> GetByIDAsync(int id)
        {
            var tag = await _unitOfWork.Tags.GetByIdAsync(id);
            return _mapper.Map<TagDTO>(tag);
        }

        public async Task UpdateAsync(TagUpdateDTO tagUpdateDTO)
        {
            var tag = _mapper.Map<Tag>(tagUpdateDTO);
            await _unitOfWork.Tags.UpdateAsync(tag);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
