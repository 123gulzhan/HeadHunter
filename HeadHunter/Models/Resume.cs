using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HeadHunter.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HeadHunter.Models
{
    public class Resume
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required(ErrorMessage = "Заполните")]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "Длина от 5 до 40 знаков")]
        public string PositionName { get; set; }
        public decimal Salary { get; set; }
        public string Telegram {get; set;}
        public string Facebook { get; set; }
        public string LinkedIn { get; set; }
        public Status Status { get; set; } = Status.Неопубликованное;
        public DateTime? DateOfPublication { get; set; }
        public DateTime? DateOfUpdate { get; set; }

        public string ApplicantId { get; set; }
        public virtual User Applicant { get; set; }

        [Remote("CheckCategory", "Validation", ErrorMessage = "Выберите категорию")]
        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual List<JobExperience> JobExperiences { get; set; } 

        public virtual List<Qualification> Qualifications { get; set; } 

        
    }
}