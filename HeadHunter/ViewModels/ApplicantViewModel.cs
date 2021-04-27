using System.Collections.Generic;
using HeadHunter.Models;

namespace HeadHunter.ViewModels
{
    public class ApplicantViewModel
    {
        public User User { get; set; }
        public virtual List<Resume> Resumes { get; set; }
        public virtual List<JobExperience> JobExperiences { get; set; }
        public virtual List<Qualification> Qualifications { get; set; }
        public virtual List<Respond> Responds { get; set; }
    }
}