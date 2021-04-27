using System;
using HeadHunter.Enums;

namespace HeadHunter.Models
{
    public class Vacancy
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public string Description { get; set; }
        public string Expirience { get; set; }
        public Status Status { get; set; }
        public DateTime? DateOfPublication { get; set; }

        public string EmployerId { get; set; }
        public virtual User Employer { get; set; }

        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}