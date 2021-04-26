using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace HeadHunter.Models
{
    public class Company
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AvatarPath { get; set; }

        public virtual List<Vacancy> Vacancies { get; set; }
        
        [NotMapped] 
        public IFormFile File { get; set; }
    }
}