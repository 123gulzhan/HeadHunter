using System;
using System.Collections.Generic;
using HeadHunter.Enums;

namespace HeadHunter.Models
{
    public class Resume
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public string Email { get; set; }
        public string Telegram {get; set;}
        public string Phone { get; set; }
        public string Facebook { get; set; }
        public string LinkedIn { get; set; }
        public Status Status { get; set; } = Status.Приватное;
        public DateTime? DateOfPublication { get; set; }
        public DateTime? DateOfUpdate { get; set; }

        public string ApplicantId { get; set; }
        public virtual User Applicant { get; set; }

        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual List<JobExperience> JobExperiences { get; set; } 

        public virtual List<Qualification> Qualifications { get; set; } 

        
    }
}