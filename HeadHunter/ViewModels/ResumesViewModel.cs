﻿using System.Collections.Generic;
using System.Linq;
using HeadHunter.Models;
using X.PagedList;

namespace HeadHunter.ViewModels
{
    public class ResumesViewModel
    {
        public IPagedList<Resume> Resumes { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public string Position { get; set; }
        public string CategoryId { get; set; }
        public string EmployerId { get; set; }
    }
}