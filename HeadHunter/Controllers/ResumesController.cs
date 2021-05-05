using System;
using System.Linq;
using System.Threading.Tasks;
using HeadHunter.Enums;
using HeadHunter.Models;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult Index(string resumeId)
        {
            Resume resume = _db.Resumes.FirstOrDefault(r => r.Id == resumeId);
            if (resume != null)
            {
                return View(resume);
            }

            return NotFound();
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
                return RedirectToAction("Index", "Resumes", new{resumeId = resume.Id});
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
        public async Task<IActionResult> AddQualification(Qualification qualification)
        {
            if (qualification.ResumeId == null) return NotFound();
            if (ModelState.IsValid)
            {
                Resume resume = _db.Resumes.FirstOrDefault(r => r.Id == qualification.ResumeId);
                if (resume == null) return NotFound();
                
                resume.Qualifications.Add(qualification);
                await _db.Qualifications.AddAsync(qualification);
                _db.Resumes.Update(resume);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Resumes", new {resumeId = qualification.ResumeId});
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

        [HttpGet]
        public IActionResult EditResume(string id, string userid)
        {
            if (id == null) return NotFound();
            Resume resume = _db.Resumes.FirstOrDefault(r => r.Id == id);
            if (resume == null || userid == null) return NotFound();

            User user = _db.Users.FirstOrDefault(u => u.Id == userid);
            if (user == null) return NotFound();
            
            resume.ApplicantId = userid; // дебаг юзера !!!!!!!!!!!!!!
            resume.DateOfUpdate = DateTime.Now;
            ViewBag.Categories = _db.Categories.ToList();
            return View(resume);
        }

        [HttpPost]
        public IActionResult EditResume(Resume resume)
        {
            if (resume == null) return NotFound();
            if (ModelState.IsValid)
            {
                resume.DateOfUpdate = DateTime.Now;
                _db.Resumes.Update(resume);
                _db.SaveChanges();
                return RedirectToAction("Index", "Resumes", new {resumeId = resume.Id});
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Publish(string resumeId)
        {
            if (resumeId == null) return NotFound();
            Resume resume = _db.Resumes.FirstOrDefault(r => r.Id == resumeId);
            if (resume != null)
            {
                resume.DateOfPublication = DateTime.Now;
                resume.Status = Status.Публичное;
                _db.Resumes.Update(resume);
                _db.SaveChanges();
                return RedirectToAction("ApplicantProfile", "Users", new {userId = resume.ApplicantId});
            }
            return NotFound();
        }
        
        
        [HttpPost]
        public IActionResult UnPublish(string resumeId)
        {
            if (resumeId == null) return NotFound();
            Resume resume = _db.Resumes.FirstOrDefault(r => r.Id == resumeId);
            if (resume != null)
            {
                resume.DateOfPublication = null;
                resume.Status = Status.Неопубликованное;
                _db.Resumes.Update(resume);
                _db.SaveChanges();
                return RedirectToAction("ApplicantProfile", "Users", new {userId = resume.ApplicantId});
            }
            return NotFound();
        }
        
        
    }
}