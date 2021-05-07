using System;
using System.Collections.Generic;
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
        public IActionResult Index(string respondId)
        {
            if (respondId != null)
            {
                var respond = _db.Responds.FirstOrDefault(r => r.Id == respondId);
                if (respond != null 
                    && _userManager.GetUserId(User) == respond.Resume.ApplicantId 
                    || _userManager.GetUserId(User) == respond.Vacancy.EmployerId)
                {
                    return View(respond);
                }
                return NotFound();
            }
            return NotFound();
        }
        
        [HttpGet]
        public IActionResult CreateAjax(string resumeId, string vacancyId)
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

        public IActionResult AddMessage(string userId, string message, string respondId)
        {
            _db.Messages.Add(new Message
            {
                UserId = userId,
                UserMessage = message,
                RespondId = respondId
            });
            _db.SaveChanges();
            
            List<Message> messages = _db.Messages.Where(m => m.UserId == userId).ToList();
            messages = messages.TakeLast(1).ToList();
            messages[0].User = _db.Users.FirstOrDefault(u => u.Id == userId);
            return Json(messages);
        }
    }
}