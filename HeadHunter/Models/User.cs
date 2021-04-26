using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace HeadHunter.Models
{
    public class User : IdentityUser
    {
        public string AvatarPath { get; set; }

        public virtual List<Resume> Resumes { get; set; }
        
        [NotMapped]
        public IFormFile File { get; set; }
    }
}