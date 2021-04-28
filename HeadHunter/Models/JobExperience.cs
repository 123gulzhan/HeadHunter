﻿using System;
using System.Collections.Generic;

namespace HeadHunter.Models
{
    public class JobExperience
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public DateTime DateOfBegining { get; set; }
        public DateTime DateOfEnd { get; set; }

        public string ApplicantId { get; set; }
        public virtual User Applicant { get; set; }
    }
}