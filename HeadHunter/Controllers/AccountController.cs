using HeadHunter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HeadHunter.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
    }
}