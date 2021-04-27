using System;
using System.Collections.Generic;

namespace HeadHunter.Models
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
    }
}