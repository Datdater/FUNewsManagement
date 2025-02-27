using BusinessLogicLayer.Model.AccountModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BusinessLogicLayer.Service.Contracts;
using PresentationLayer.Models;
using AutoMapper;
using BusinessLogicLayer.Enum;
using PresentationLayer.Models.AccountViewModels;
using BusinessLogicLayer.Model.NewsArticleModel;
using BusinessLogicLayer.Service;
using PresentationLayer.Models.NewsArticleModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Models.CategoryViewModels;
using PresentationLayer.Models.TagViewModels;

namespace PresentationLayer.Controllers
{
	public class AccountController : Controller
	{
		private IAccountService _accountService;
		private IMapper _mapper;
		public AccountController(IAccountService accountService, IMapper mapper) 
		{
			_accountService = accountService;
			_mapper = mapper;
		}
		[Authorize(Roles = nameof(Role.Admin))]
		public async Task<IActionResult> GetAll()
		{
            var roles = Enum.GetValues(typeof(Role))
                        .Cast<Role>()
                        .Select(r => new { Id = (int)r, Name = r.ToString() }).Where(x => x.Name != "Admin")
                        .ToList();

            ViewBag.Roles = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(roles, "Id", "Name");

            var accountListRaw = await _accountService.GetAllAsync();
			var accountList = _mapper.Map<List<AccountViewModel>>(accountListRaw);
			return View("GetAll", accountList);
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);
			var loginViewModel =  _mapper.Map<LoginDTO>(model);	
			var result = await _accountService.AuthenticateAsync(loginViewModel);

			if (result != null)
			{
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, result.AccountName),
					new Claim(ClaimTypes.Sid, result.AccountID),
					new Claim(ClaimTypes.Email, result.AccountEmail),
					new Claim(ClaimTypes.Role, result.AccountRole.ToString()),

				};


				var claimsIdentity = new ClaimsIdentity(claims,
					CookieAuthenticationDefaults.AuthenticationScheme);

				var authProperties = new AuthenticationProperties
				{
					ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
				};

				await HttpContext.SignInAsync(
					CookieAuthenticationDefaults.AuthenticationScheme,
					new ClaimsPrincipal(claimsIdentity),
					authProperties);
                if (result.AccountRole == BusinessLogicLayer.Enum.Role.Lecturer)
                {
                    return RedirectToAction("GetAll", "NewsArticle");

                }
                else
                {
                    return RedirectToAction("GetAllDashboard", "NewsArticle");

                }
            }
            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
		}
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("GetAll", "NewsArticle");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> Create(AccountCreateViewModel accountModel)
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
            var account = _mapper.Map<AccountCreateDTO>(accountModel);
            await _accountService.CreateAccount(account);
            return Json(new { success = true });
        }

        [Authorize(Roles = nameof(Role.Admin))]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _accountService.DeleteAccount(id);
                return Json(new { success = true, message = "Account successfully deleted" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error deleting Account: {ex.Message}" });
            }
        }

        public async Task<IActionResult> Update(int id)
        {
            var roles = Enum.GetValues(typeof(Role))
                        .Cast<Role>()
                        .Select(r => new { Id = (int)r, Name = r.ToString() }).Where(x => x.Name != "Admin")
                        .ToList();

            ViewBag.Roles = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(roles, "Id", "Name");
            var newsArticleRaw = await _accountService.GetByIdAsync(id);
            var newsArticle = _mapper.Map<AccountUpdateViewModel>(newsArticleRaw);
            return View("Partial/_Update", newsArticle);
        }

        public async Task<IActionResult> Profile()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.Sid); // Get logged-in user ID
            if (currentUserId == null)
            {
                return View("Login");
            } else
            {
                var roles = Enum.GetValues(typeof(Role))
                       .Cast<Role>()
                       .Select(r => new { Id = (int)r, Name = r.ToString() }).Where(x => x.Name != "Admin")
                       .ToList();

                ViewBag.Roles = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(roles, "Id", "Name");
                var accountRaw = await _accountService.GetByIdAsync(int.Parse(currentUserId));
                var account = _mapper.Map<AccountUpdateViewModel>(accountRaw);
                return View("Profile", account);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(string password)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.Sid); // Get logged-in user ID
            if (currentUserId == null)
            {
                return Json(new { success = false, message = "Not login" }); ;
            }
            else
            {
                try
                {
                    await _accountService.UpdatePassword(password, currentUserId);
                    var accountRaw = await _accountService.GetByIdAsync(int.Parse(currentUserId));
                    return Json(new { success = true, message = "Update password success" }); 
                }
                catch (Exception e)
                {

                    return Json(new { success = false, message = e.Message });

                }

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AccountUpdateViewModel accountUpdate)
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
                var account = _mapper.Map<AccountUpdateDTO>(accountUpdate);
                await _accountService.UpdateAccount(account);
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false, errors = e.Message });
            }
            
        }
    }
}
