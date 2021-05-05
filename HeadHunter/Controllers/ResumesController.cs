using System;
using System.Linq;
using HeadHunter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HeadHunter.Controllers
{
    public class ResumesController : Controller
    {
        private HeadHunterContext _db;
        private UserManager<User> _userManager;

        public ResumesController(HeadHunterContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        
        [HttpGet]
        public IActionResult AddResume()
        {
            ViewBag.Categories = _db.Categories.ToList();
            Resume resume = new Resume();
            return View(resume);
        }

        [HttpPost]
        public IActionResult AddResume(Resume resume)
        {
            if (ModelState.IsValid)
            {
                _db.Resumes.Add(resume);
                _db.SaveChanges();
                return RedirectToAction("ApplicantProfile", "Users", new{userId = resume.ApplicantId});
            }

            return NotFound();
        }
        
        [HttpPost]
        public IActionResult AddJobExperience(JobExperience experience)
        {
            if (experience.ResumeId == null) return NotFound();
            if (ModelState.IsValid)
            {
                _db.JobExperiences.Add(experience);
                _db.SaveChanges();
                return RedirectToAction("Index", "Resumes", new {resumeId = experience.ResumeId});
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult AddQualification(Qualification qualification)
        {
            if (qualification.ResumeId == null) return NotFound();
            if (ModelState.IsValid)
            {
                _db.Qualifications.Add(qualification);
                _db.SaveChanges();
                return RedirectToAction("Index", "Resumes", new {resumeId = qualification.ResumeId});
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Index(string resumeId)
        {
            Resume resume = _db.Resumes.FirstOrDefault(r => r.Id == resumeId);
            if (resume != null)
            {
                return View(resume);
            }

            return NotFound();
        }

        public IActionResult Update(string id)
        {
            if (id == null) return NotFound();
            Resume resume = _db.Resumes.FirstOrDefault(r => r.Id == id);
            if (resume == null) return NotFound();
            resume.DateOfUpdate = DateTime.Now;
            _db.Resumes.Update(resume);
            _db.SaveChanges();
            return RedirectToAction("ApplicantProfile", "Users", new{userId = resume.ApplicantId});
        }
    }
}