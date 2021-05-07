using System;
using HeadHunter.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HeadHunter.Models
{
    public class Vacancy
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public string Description { get; set; }
        public string Experience { get; set; }
        public Status Status { get; set; }
        public DateTime? DateOfPublication { get; set; }
        public DateTime? DateOfUpdate { get; set; }

        public string EmployerId { get; set; }
        public virtual User Employer { get; set; }
        
        [Remote("CheckCategory", "Validation", ErrorMessage = "Выберите категорию")]
        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}