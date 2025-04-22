using AutoMapper;
using BusinessLogicLayer.Enum;
using BusinessLogicLayer.Model.CategoryModel;
using BusinessLogicLayer.Model.NewsArticleModel;
using BusinessLogicLayer.Service;
using BusinessLogicLayer.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using PresentationLayer.Models.CategoryViewModels;
using PresentationLayer.Models.NewsArticleModel;
using PresentationLayer.Models.TagViewModels;
using System.Drawing.Printing;
using System.Security.Claims;
using X.PagedList;
using X.PagedList.Extensions;


namespace PresentationLayer.Controllers
{
    public class NewsArticleController : Controller
    {
        private INewsArticleService _newArticleService;
        private ITagService _tagService;
        private ICategoryService _categoryService;
        private IMapper _mapper;

        public NewsArticleController(INewsArticleService newArticleService, IMapper mapper, ICategoryService categoryService, ITagService tagService)
        {
            _newArticleService = newArticleService;
            _mapper = mapper;
            _categoryService = categoryService;
            _tagService = tagService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 5, string searchTerm = null, int? categoryId = null)
        {
            var newsArticleListRaw = await _newArticleService.GetAllAsync();
            var newsArticleList = _mapper.Map<List<NewsArticleViewModel>>(newsArticleListRaw);
            newsArticleList = newsArticleList
                .Where(x => x.NewsStatus == true)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();
            // Apply filters if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                newsArticleList = newsArticleList.Where(n =>
                    n.NewsTitle?.ToLower().Contains(searchTerm) == true ||
                    n.Headline?.ToLower().Contains(searchTerm) == true ||
                    n.NewsContent?.ToLower().Contains(searchTerm) == true
                ).ToList();
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                newsArticleList = newsArticleList.Where(n => n.CategoryID == categoryId.Value).ToList();
            }

            // Load categories for the filter dropdown
            var categoryListRaw = await _categoryService.GetAllAsync();
            var categoryList = _mapper.Map<List<CategoryViewModel>>(categoryListRaw)
                                      .Where(x => x.IsActive)
                                      .ToList();
            ViewBag.Categories = new SelectList(categoryList, "CategoryID", "CategoryName", categoryId);

            // Store the search parameters in ViewBag for preserving the search when paging
            ViewBag.SearchTerm = searchTerm;
            ViewBag.CategoryId = categoryId;

            var pagedList = newsArticleList.ToPagedList(page, pageSize);
            return View("Default/GetAll", pagedList);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var newsArticleRaw = await _newArticleService.GetByIdAsync(id);
            var newsArticle = _mapper.Map<NewsArticleViewModel>(newsArticleRaw);
            return View("Default/Get", newsArticle);
        }

        [Authorize(Roles = nameof(Role.Admin) + "," + nameof(Role.Staff))]
        public async Task<IActionResult> GetDashboard(int id)
        {
            var newsArticleRaw = await _newArticleService.GetByIdAsync(id);
            var newsArticle = _mapper.Map<NewsArticleViewModel>(newsArticleRaw);
            return View("Dashboard/Get", newsArticle);
        }


        [HttpGet]
        [Authorize(Roles = nameof(Role.Admin) + "," + nameof(Role.Staff))]
        public async Task<IActionResult> GetAllDashboard(string? dateStart, string? dateEnd, string? filterByCreator)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.Sid); // Get logged-in user ID

            var tagListRaw = await _tagService.GetAllAsync();
            var tagList = _mapper.Map<List<TagViewModel>>(tagListRaw);
            ViewBag.Tags = new SelectList(tagList, "TagID", "TagName");

            var categoryListRaw = await _categoryService.GetAllAsync();
            var categoryList = _mapper.Map<List<CategoryViewModel>>(categoryListRaw)
                                      .Where(x => x.IsActive)
                                      .ToList();
            ViewBag.Categories = new SelectList(categoryList, "CategoryID", "CategoryName");

            // Convert string dates to DateTime
            DateTime? startDate = string.IsNullOrEmpty(dateStart)? null: DateTime.Parse(dateStart).Date; 
            DateTime? endDate = string.IsNullOrEmpty(dateEnd)
                ? null
                : DateTime.Parse(dateEnd).Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            // filter news articles
            var newsArticleListRaw = await _newArticleService.GetAllAsync();

            var filteredNewsArticles = newsArticleListRaw
                .Where(m => (!startDate.HasValue || m.CreatedDate >= startDate.Value) &&
                            (!endDate.HasValue || m.CreatedDate <= endDate.Value) &&
                            (filterByCreator.IsNullOrEmpty() || m.CreatedByID.ToString() == currentUserId))
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            var newsArticleList = _mapper.Map<List<NewsArticleViewModel>>(filteredNewsArticles);
            return View("Dashboard/GetAll", newsArticleList);
        }


        [HttpGet]
        [Authorize(Roles = nameof(Role.Staff))]
        public IActionResult Create()
        {

            return View("Dashboard/Partial/_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(Role.Staff))]
        public async Task<IActionResult> Create(NewsArticleCreateViewModel newsAriticle)
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
            var newsArticle = _mapper.Map<NewsArticleCreateDTO>(newsAriticle);
            await _newArticleService.CreateAsync(newsArticle);
            return Json(new { success = true });
        }

        [Authorize(Roles = nameof(Role.Staff))]
        public async Task<IActionResult> Update(int id)
        {

            var categoryListRaw = await _categoryService.GetAllAsync();
            var categoryList = _mapper.Map<List<CategoryViewModel>>(categoryListRaw).Where(x => x.IsActive).ToList();
            ViewBag.Categories = new SelectList(categoryList, "CategoryID", "CategoryName");

            var newsArticleRaw = await _newArticleService.GetByIdAsync(id);
            var newsArticle = _mapper.Map<NewsArticleUpdateViewModel>(newsArticleRaw);

            var tagListRaw = await _tagService.GetAllAsync();
            var tagList = _mapper.Map<List<TagViewModel>>(tagListRaw);
            ViewBag.Tags = new SelectList(tagList, "TagID", "TagName", newsArticle.TagIds.ToList());


            return View("Dashboard/Partial/_Update", newsArticle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(Role.Staff))]
        public async Task<IActionResult> Update(NewsArticleUpdateViewModel newsAriticle)
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
            var newsArticle = _mapper.Map<NewsArticleUpdateDTO>(newsAriticle);
            await _newArticleService.UpdateAsync(newsArticle);
            return Json(new { success = true });
        }

        [Authorize(Roles = nameof(Role.Staff))]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _newArticleService.DeleteAsync(id);
                return Json(new { success = true, message = "Article successfully deleted" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error deleting article: {ex.Message}" });
            }
        }

    }
}
