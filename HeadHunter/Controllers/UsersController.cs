using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadHunter.Models;
using HeadHunter.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeadHunter.Controllers
{
    public class UsersController : Controller
    {
        private HeadHunterContext _db;
        private UserManager<User> _userManager;

        public UsersController(HeadHunterContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ApplicantProfileAsync(string userId, string resumeId)
        {
            if (userId != null)
            {
                User user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return NotFound();

                List<Respond> responds = _db.Responds.ToList();

                ApplicantViewModel model = new ApplicantViewModel
                {
                    User = user,
                    Resumes = _db.Resumes.Where(r => r.ApplicantId == userId).ToList(),
                    JobExperiences = _db.JobExperiences.ToList(),
                    Qualifications = _db.Qualifications.ToList(),
                    Responds = responds
                };
                if (resumeId != null) ViewBag.ResumeId = resumeId;
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> EmployerProfile(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            List<Respond> responds = _db.Responds.ToList();

            EmployerViewModel model = new EmployerViewModel
            {
                User = user,
                Vacancies = _db.Vacancies.Where(v => v.EmployerId == userId).ToList(),
                Responds = responds
            };

            return View(model);
        }
    }
}