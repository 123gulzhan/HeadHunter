using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HeadHunter.Models
{
    public class JobExperience
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required(ErrorMessage = "Заполните")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Длина от 5 до 15 знаков")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Заполните")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Длина от 6 до 20 знаков")]
        public string CompanyName { get; set; }
        
        
        [Required(ErrorMessage = "Заполните")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DateOfBegin { get; set; }
        
        [Required(ErrorMessage = "Заполните")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DateOfEnd { get; set; }

        public string ApplicantId { get; set; }
        public virtual User Applicant { get; set; }
    }
}