using System;

namespace HeadHunter.Models
{
    public class Respond
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime DateOfRespond { get; set; } = DateTime.Now;

        public string ResumeId { get; set; }
        public virtual Resume Resume { get; set; }

        public string VacancyId { get; set; }
        public virtual Vacancy Vacancy { get; set; }

    }
}