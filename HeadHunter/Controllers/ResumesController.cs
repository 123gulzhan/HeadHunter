using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using HeadHunter.Enums;
using HeadHunter.Models;
using HeadHunter.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

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
        [Authorize(Roles = "applicant")]
        public IActionResult AddResume()
        {
            ViewBag.Categories = _db.Categories.ToList();
            Resume resume = new Resume();
            return View(resume);
        }

        [HttpPost]
        [Authorize(Roles = "applicant")]
        public IActionResult AddResume(Resume resume)
        {
            if (ModelState.IsValid)
            {
                _db.Resumes.Add(resume);
                _db.SaveChanges();
                return RedirectToAction("Index", "Resumes", new {resumeId = resume.Id});
            }

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "applicant")]
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
        [Authorize(Roles = "applicant")]
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


        [Authorize(Roles = "applicant")]
        public IActionResult Update(string id)
        {
            if (id == null) return NotFound();
            Resume resume = _db.Resumes.FirstOrDefault(r => r.Id == id);
            if (resume == null) return NotFound();
            resume.DateOfUpdate = DateTime.Now;
            _db.Resumes.Update(resume);
            _db.SaveChanges();
            return RedirectToAction("ApplicantProfile", "Users", new {userId = resume.ApplicantId});
        }

        [HttpGet]
        [Authorize(Roles = "applicant")]
        public IActionResult EditResume(string id)
        {
            if (id == null) return NotFound();
            Resume resume = _db.Resumes.FirstOrDefault(r => r.Id == id);
            if (resume == null) return NotFound();

            resume.DateOfUpdate = DateTime.Now;
            ViewBag.Categories = _db.Categories.ToList();
            return View(resume);
        }

        [HttpPost]
        [Authorize(Roles = "applicant")]
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
        [Authorize(Roles = "applicant")]
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
        [Authorize(Roles = "applicant")]
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

        [HttpGet]
        [Authorize(Roles = "employer")]
        public IActionResult GetResumes(string employerId, string categoryId, string positionPart, string salaryOrder, int? page)
        {
            int pageSize = 2; //20 - сколько элементов будет на странице отображаться
            int pageNumber = page ?? 1;

            var resumes = _db.Resumes.Where(r => r.Status == Status.Публичное)
                .OrderByDescending(r => r.DateOfUpdate);

            IEnumerable<Category> categories = _db.Categories;

            if (categoryId != null)
            {
                Category catFilter = _db.Categories.FirstOrDefault(c => c.Id == categoryId);
                if (catFilter != null && !catFilter.Name.Contains("Не выбрано"))
                {
                    resumes = (IOrderedQueryable<Resume>) resumes.Where(r => r.CategoryId == categoryId);
                }
            }

            if (positionPart != null)
            {
                positionPart = positionPart.ToLower();
                resumes = (IOrderedQueryable<Resume>) resumes.Where(r => r.PositionName.Contains(positionPart));
            }

            if (salaryOrder != null && salaryOrder == "asc")
            {
                resumes = resumes.OrderBy(r => r.Salary);
            }
            else if(salaryOrder != null && salaryOrder == "desc")
            {
                resumes = resumes.OrderByDescending(r => r.Salary);
            }

            AllResumesViewModel model = new AllResumesViewModel
            {
                Resumes = resumes.ToPagedList(pageNumber, pageSize),
                Categories = categories,
                EmployerId = employerId
            };

            return View(model);
        }
    }
}