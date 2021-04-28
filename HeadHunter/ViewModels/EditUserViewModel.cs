using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HeadHunter.ViewModels
{
    public class EditUserViewModel
    {
        [Required(ErrorMessage = "Укажите Имя")]
        [Display(Name = "Имя")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Укажите телефон")]
        [Display(Name = "Телефон")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public string AvatarPath { get; set; }
        public IFormFile File { get; set; }

        public string Id { get; set; }
    }
}