using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HeadHunter.Models;
using HeadHunter.Services;
using HeadHunter.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace HeadHunter.Controllers
{
    public class AccountController : Controller
    {
        private HeadHunterContext _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHostEnvironment _environment;
        private readonly FileUploadService _uploadService;

        public AccountController(HeadHunterContext db,
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager, 
            SignInManager<User> signInManager, 
            IHostEnvironment environment, 
            FileUploadService uploadService)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _environment = environment;
            _uploadService = uploadService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(_environment.ContentRootPath, "wwwroot\\Images\\Avatars");
                string avatarPath = $"\\Images\\Avatars\\defaultavatar.jpg";
                if (model.File != null)
                {
                    avatarPath = $"Images\\Avatars\\{model.File.FileName}";
                    _uploadService.Upload(path, model.File.FileName, model.File);
                }
                model.AvatarPath = avatarPath;

                    User user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    AvatarPath = model.AvatarPath
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (model.Role == "applicant")
                    {
                        await _userManager.AddToRoleAsync(user, "applicant");
                    }
                    else if (model.Role == "employer")
                    {
                        await _userManager.AddToRoleAsync(user, "employer");
                    }

                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Users");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel{ReturnUrl = returnUrl});
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByEmailAsync(model.Email);
                var result = await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.RememberMe,
                    false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("","Неверный логин или пароль");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(string id = null)
        {
            User user = _userManager.FindByIdAsync(id).Result;

            EditUserViewModel model = new EditUserViewModel
            {
                UserName = user.UserName,
                Phone = user.PhoneNumber,
                AvatarPath = user.AvatarPath,
                Id = user.Id
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.UserName = model.UserName;
                    user.PhoneNumber = model.Phone;
                    if (model.File != null)
                    {
                        string path = Path.Combine(_environment.ContentRootPath, "wwwroot\\Images\\Avatars");
                        string avatarPath = $"Images\\Avatars\\{model.File.FileName}";
                        _uploadService.Upload(path, model.File.FileName, model.File);
                        model.AvatarPath = avatarPath;

                        user.AvatarPath = model.AvatarPath;
                    }

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ChangePasswordViewModel model = new ChangePasswordViewModel
            {
                Id = user.Id,
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var passwordValidator = HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var passwordHasher = HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;
                    var result = await passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, model.NewPassword);
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("NewPassword", error.Description);
                }
                ModelState.AddModelError("", "Пользователь не существует");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}