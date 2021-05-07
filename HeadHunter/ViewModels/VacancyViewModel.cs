using System.Collections.Generic;
using HeadHunter.Models;

namespace HeadHunter.ViewModels
{
    public class VacancyViewModel
    {
        public Vacancy Vacancy { get; set; }
        public List<Resume> Resumes { get; set; }
        public Respond Respond { get; set; }
    }
}