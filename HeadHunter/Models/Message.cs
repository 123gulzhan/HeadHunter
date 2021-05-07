using System;

namespace HeadHunter.Models
{
    public class Message
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserMessage { get; set; }
        public DateTime SendDate { get; set; } = DateTime.Now;
        
        public string RespondId { get; set; }
        public virtual Respond Respond { get; set; }
        
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}