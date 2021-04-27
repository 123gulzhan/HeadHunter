using System.Collections.Generic;
using System.Linq;
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
        
        
        [HttpGet]
        [Authorize(Roles = "applicant")]
        public IActionResult Index(string userId)
        {
            User user = _db.Users.FirstOrDefault(u => u.Id == userId);
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
    }
}