using System.Collections.Generic;
using HeadHunter.Models;

namespace HeadHunter.ViewModels
{
    public class EmployerViewModel
    {
        public User User { get; set; }
        public virtual List<Vacancy> Vacancies { get; set; }
        public virtual List<Respond> Responds { get; set; }
    }
}