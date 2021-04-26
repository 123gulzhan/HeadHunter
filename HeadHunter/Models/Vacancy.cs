using System;
using HeadHunter.Enums;

namespace HeadHunter.Models
{
    public class Vacancy
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public double Salary { get; set; }
        public string Description { get; set; }
        public int Expirience { get; set; }
        public Status Status { get; set; }
        public DateTime DateOfPublication { get; set; }

        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}