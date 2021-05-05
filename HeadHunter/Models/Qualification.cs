using System;
using System.ComponentModel.DataAnnotations;

namespace HeadHunter.Models
{
    public class Qualification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required(ErrorMessage = "Заполните")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Длина от 5 до 15 знаков")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Заполните")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "Длина от 6 до 40 знаков")]
        public string CompanyName { get; set; }
        
        [Required(ErrorMessage = "Заполните")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DateOfBegin { get; set; }
        
        [Required(ErrorMessage = "Заполните")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime DateOfEnd { get; set; }

        public string ResumeId { get; set; }
        public virtual Resume Resume { get; set; }
    }
}