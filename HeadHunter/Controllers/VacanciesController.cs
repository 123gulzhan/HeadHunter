using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeadHunter.Enums;
using HeadHunter.Models;
using HeadHunter.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

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
        public async Task<IActionResult> Index(VacancyViewModel model, string id)
        {
            model = new VacancyViewModel
            {
                Resumes = _db.Resumes.ToList(),
                Vacancy = _db.Vacancies.FirstOrDefault(v => v.Id == id)
            };
            
            if (model.Vacancy != null)
            {
                return View(model);
            }

            return NotFound();
        }

        [HttpGet]
        [Authorize(Roles = "employer")]
        public IActionResult Create()
        {
            CreateVacancyViewModel model = new CreateVacancyViewModel
            {
                Categories = _db.Categories.ToList()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "employer")]
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
        [Authorize(Roles = "employer")]
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
        [Authorize(Roles = "employer")]
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
        [Authorize(Roles = "employer")]
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
        [Authorize(Roles = "employer")]
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
        [Authorize(Roles = "employer")]
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


        [HttpGet]
        [Authorize]
        public IActionResult GetVacancies(string applicantId, string categoryId, 
            string positionPart, string salaryOrder, int? page)
        {
            int pageSize = 2; //20 - сколько элементов будет на странице отображаться
            int pageNumber = page ?? 1;

            var vacancies = _db.Vacancies.Where(v => v.Status == Status.Публичное)
                .OrderByDescending(r => r.DateOfUpdate);

            IEnumerable<Category> categories = _db.Categories;

            if (categoryId != null)
            {
                Category catFilter = _db.Categories.FirstOrDefault(c => c.Id == categoryId);
                if (catFilter != null && !catFilter.Name.Contains("Не выбрано"))
                {
                    vacancies = (IOrderedQueryable<Vacancy>)vacancies.Where(r => r.CategoryId == categoryId);
                }
            }

            if (positionPart != null)
            {
                positionPart = positionPart.ToLower();
                vacancies = (IOrderedQueryable<Vacancy>)vacancies.Where(r => r.Name.Contains(positionPart));
            }

            if (salaryOrder != null && salaryOrder == "asc")
            {
                vacancies = vacancies.OrderBy(r => r.Salary);
            }
            else if(salaryOrder != null && salaryOrder == "desc")
            {
                vacancies = vacancies.OrderByDescending(r => r.Salary);
            }
            
            AllVacanciesViewModel model = new AllVacanciesViewModel()
            {
                Vacancies = vacancies.ToPagedList(pageNumber, pageSize),
                Categories = categories,
                ApplicantId = applicantId
            };

            return View(model);
        }
        
    }
}