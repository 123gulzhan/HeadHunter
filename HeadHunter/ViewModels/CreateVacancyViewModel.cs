using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HeadHunter.Models;

namespace HeadHunter.ViewModels
{
    public class CreateVacancyViewModel
    {
        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string EmployerId { get; set; }
        public virtual User Employer { get; set; }

        public List<Category> Categories { get; set; }
        public string Experience { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Название вакансии")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        
        [Display(Name = "Заработная плата")] 
        public decimal Salary { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name = "Описание вакансии")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        
        
    }
}