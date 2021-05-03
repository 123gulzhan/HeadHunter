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
        //[Authorize(Roles = "applicant")]
        public async Task<IActionResult> IndexAsync(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            List<Respond> responds = _db.Responds.Include(respond => respond.Resume)
                .ThenInclude(r => r.Applicant)
                .Join(_db.Resumes.Where(resume => resume.ApplicantId == userId),
                    respond => respond.ResumeId,
                    resume => resume.ApplicantId,
                    (respond, resume) => new Respond
                    {
                        Id = respond.Id,
                        DateOfRespond = respond.DateOfRespond,
                        ResumeId = resume.Id,
                        VacancyId = respond.VacancyId
                    })
                .OrderByDescending(r => r.DateOfRespond)
                .ToList();
            
            ApplicantViewModel model = new ApplicantViewModel
            {
                User = user,
                Resumes = _db.Resumes.Where(r => r.ApplicantId == userId).ToList(),
                JobExperiences = _db.JobExperiences.Where(j => j.ApplicantId == userId).ToList(),
                Qualifications = _db.Qualifications.Where(q => q.ApplicantId == userId).ToList(),
                Responds = responds
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EmployerProfile(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            List<Respond> responds = _db.Responds.Include(r => r.Vacancy).ThenInclude(r=>r.Employer)
                .Include(r => r.Resume).ToList();

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