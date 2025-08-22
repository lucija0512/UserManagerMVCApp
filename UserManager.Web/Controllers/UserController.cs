using Microsoft.AspNetCore.Mvc;
using UserManager.Application.Interfaces;
using UserManager.Web.ViewModels;

namespace UserManager.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserManagerService _userService;
        public UserController(IUserManagerService userService)
        {
            _userService = userService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName, LastName, Email")] UserFormViewModel userForm)
        {
            var result = await _userService.SaveUserFormAsync(userForm.FirstName, userForm.LastName, userForm.Email);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = $"Korisnik {userForm.Email} uspješno upisan";
                return RedirectToAction();
            }

            foreach (var error in result.ErrorMessages)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            return View(userForm);
        }
    }
}
