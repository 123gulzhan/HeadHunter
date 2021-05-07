using System.Collections.Generic;
using HeadHunter.Models;
using X.PagedList;

namespace HeadHunter.ViewModels
{
    public class AllVacanciesViewModel
    {
        public IPagedList<Vacancy> Vacancies { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public string Position { get; set; }
        public string CategoryId { get; set; }
        public string ApplicantId { get; set; }
    }
}