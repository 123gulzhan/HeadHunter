using System;
using System.Linq;
using HeadHunter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HeadHunter.Controllers
{
    public class RespondsController : Controller
    {
        private HeadHunterContext _db;
        private readonly UserManager<User> _userManager;

        public RespondsController(HeadHunterContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Create(string resumeId, string vacancyId)
        {
            Respond respond = new Respond
            {
                ResumeId = resumeId,
                Resume = _db.Resumes.FirstOrDefault(r => r.Id == resumeId),
                VacancyId = vacancyId,
                Vacancy = _db.Vacancies.FirstOrDefault(v => v.Id == vacancyId),
                DateOfRespond = DateTime.Now
            };
            _db.Responds.Add(respond);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        

    }
}