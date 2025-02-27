using AutoMapper;
using BusinessLogicLayer.Enum;
using BusinessLogicLayer.Model.AccountModel;
using BusinessLogicLayer.Model.CategoryModel;
using BusinessLogicLayer.Service;
using BusinessLogicLayer.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Models.AccountViewModels;
using PresentationLayer.Models.CategoryViewModels;
using System.Diagnostics;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = nameof(Role.Staff))]
    public class CategoryController : Controller
    {
        private ICategoryService _categoryService;
        private IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }


        public async Task<IActionResult> GetAll()
        {
            

            var categoryListRaw = await _categoryService.GetAllAsync();
            var categoryList = _mapper.Map<List<CategoryViewModel>>(categoryListRaw);

            var categoryParent = categoryList?.Where(x => x.ParentCategoryID == null).ToList();
            ViewBag.ParentCategories = new SelectList(categoryParent, "CategoryID", "CategoryName");
            return View("GetAll", categoryList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel categoryModel)
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
            
            var category = _mapper.Map<CategoryCreateDTO>(categoryModel);
            await _categoryService.CreateAsync(category);
            return Json(new { success = true });
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
                return Json(new { success = true, message = "Category successfully deleted" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error deleting Category: {ex.Message}" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryUpdateViewModel categoryUpdate)
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
                var category = _mapper.Map<CategoryUpdateDTO>(categoryUpdate);
                await _categoryService.UpdateAsync(category);
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false, errors = e.Message });
            }

        }

        public async Task<IActionResult> Update(int id)
        {
            var categoryListRaw = await _categoryService.GetAllAsync();
            var categoryList = _mapper.Map<List<CategoryViewModel>>(categoryListRaw);
            var categoryParent = categoryList?.Where(x => x.ParentCategoryID == null && x.CategoryID != id).ToList();
            ViewBag.ParentCategories = new SelectList(categoryParent, "CategoryID", "CategoryName");

            var categoryRaw = await _categoryService.GetByIDAsync(id);
            var category = _mapper.Map<CategoryUpdateViewModel>(categoryRaw);
            return View("Partial/_Update", category);
        }
    }
}
