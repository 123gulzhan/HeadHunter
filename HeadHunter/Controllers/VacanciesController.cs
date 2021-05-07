using System;
using System.Linq;
using System.Threading.Tasks;
using HeadHunter.Enums;
using HeadHunter.Models;
using HeadHunter.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HeadHunter.Controllers
{
    public class VacanciesController : Controller
    {
        private HeadHunterContext _db;
        private UserManager<User> _userManager;

        public VacanciesController(HeadHunterContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            Vacancy vacancy = _db.Vacancies.FirstOrDefault(v => v.Id == id);
            if (vacancy != null)
            {
                return View(vacancy);
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateVacancyViewModel model = new CreateVacancyViewModel
            {
                Categories = _db.Categories.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateVacancyViewModel model)
        {
            if (ModelState.IsValid)
            {
                Vacancy vacancy = new Vacancy
                {
                    CategoryId = model.CategoryId,
                    Category = _db.Categories.FirstOrDefault(c => c.Id == model.CategoryId),
                    Name = model.Name,
                    Salary = model.Salary,
                    Experience = model.Experience,
                    Status = Status.Неопубликованное,
                    Description = model.Description,
                    EmployerId = model.EmployerId,
                    Employer = await _userManager.FindByIdAsync(model.EmployerId)
                };
                var result = _db.Vacancies.AddAsync(vacancy);
                if (result.IsCompleted)
                {
                    await _db.SaveChangesAsync();
                    return RedirectToAction("EmployerProfile", "Users", new{userId = model.EmployerId});
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (id != null)
            {
                Vacancy vacancy = _db.Vacancies.FirstOrDefault(v => v.Id == id);
                if (vacancy != null)
                {
                    var model = new EditVacancyViewModel
                    {
                        Id = vacancy.Id,
                        Categories = _db.Categories.ToList(),
                        CategoryId = vacancy.CategoryId,
                        Category = vacancy.Category,
                        Name = vacancy.Name,
                        Salary = vacancy.Salary,
                        Experience = vacancy.Experience,
                        Status = Status.Неопубликованное,
                        Description = vacancy.Description,
                        EmployerId = vacancy.EmployerId,
                        Employer = vacancy.Employer
                    };
                    return View(model);
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditVacancyViewModel model)
        {
            Vacancy vacancy = _db.Vacancies.FirstOrDefault(v => v.Id == model.Id);
            if (ModelState.IsValid)
            {
                if (vacancy != null)
                {
                    vacancy.CategoryId = model.CategoryId;
                    vacancy.Category = _db.Categories.FirstOrDefault(c => c.Id == model.CategoryId);
                    vacancy.Name = model.Name;
                    vacancy.Salary = model.Salary;
                    vacancy.Experience = model.Experience;
                    vacancy.Description = model.Description;
                    vacancy.EmployerId = model.EmployerId;
                    vacancy.Employer = _db.Users.FirstOrDefault(u=>u.Id == model.EmployerId);
                    
                    _db.Vacancies.Update(vacancy);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index", "Vacancies", new {id = model.Id});
                }
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Update(string id)
        {
            if (id != null)
            {
                Vacancy vacancy = _db.Vacancies.FirstOrDefault(v => v.Id == id);
                if (vacancy != null)
                {
                    vacancy.DateOfUpdate = DateTime.Now;
                    _db.Vacancies.Update(vacancy);
                    _db.SaveChanges();
                    return RedirectToAction("EmployerProfile", "Users", new {userId = vacancy.EmployerId});
                }
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult PublicationVacancy(string id)
        {
            if (id != null)
            {
                Vacancy vacancy = _db.Vacancies.FirstOrDefault(v => v.Id == id);
                if (vacancy != null)
                {
                    vacancy.Status = Status.Публичное;
                    vacancy.DateOfPublication = DateTime.Now;
                    _db.Vacancies.Update(vacancy);
                    _db.SaveChanges();
                    return RedirectToAction("EmployerProfile", "Users", new {userId = vacancy.EmployerId});
                }
            }
            return NotFound();
        }
        
        [HttpGet]
        public IActionResult UnpublicationVacancy(string id)
        {
            if (id != null)
            {
                Vacancy vacancy = _db.Vacancies.FirstOrDefault(v => v.Id == id);
                if (vacancy != null)
                {
                    vacancy.Status = Status.Неопубликованное;
                    vacancy.DateOfPublication = null;
                    _db.Vacancies.Update(vacancy);
                    _db.SaveChanges();
                    return RedirectToAction("EmployerProfile", "Users", new {userId = vacancy.EmployerId});
                }
            }
            return NotFound();
        }
    }
}