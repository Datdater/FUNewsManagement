using AutoMapper;
using BusinessLogicLayer.Model.TagModel;
using BusinessLogicLayer.Service;
using BusinessLogicLayer.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.TagViewModels;

namespace PresentationLayer.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public TagController(ITagService tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetAll()
        {
            var tagListRaw = await _tagService.GetAllAsync();
            var tagList = _mapper.Map<List<TagViewModel>>(tagListRaw);

            return View("GetAll", tagList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagCreateViewModel tagModel)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return Json(new { success = false, errors = errorList });
            }

            var tag = _mapper.Map<TagCreateDTO>(tagModel);
            await _tagService.CreateAsync(tag);
            return Json(new { success = true });
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _tagService.DeleteAsync(id);
                return Json(new { success = true, message = "Tag successfully deleted" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error deleting Tag: {ex.Message}" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TagUpdateViewModel tagUpdate)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                return Json(new { success = false, errors = errorList });
            }

            try
            {
                var tag = _mapper.Map<TagUpdateDTO>(tagUpdate);
                await _tagService.UpdateAsync(tag);
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false, errors = e.Message });
            }
        }

        public async Task<IActionResult> Update(int id)
        {
            var tagRaw = await _tagService.GetByIDAsync(id);
            var tag = _mapper.Map<TagUpdateViewModel>(tagRaw);
            return View("Partial/_Update", tag);
        }
    }

}
