using System.Collections.Generic;
using System.Linq;
using HeadHunter.Enums;
using HeadHunter.Models;

namespace HeadHunter.Services
{
    public class DataSeeder
    {

        public static void SeedCategories(HeadHunterContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category() {Id = "e242a04c-5d20-41ca-812a-b58e468cbfcd", Name = "Образование"},
                    new Category() {Id = "cd3bc604-41c5-41d3-8c58-2a585a6afa53", Name = "Строительство"},
                    new Category() {Id = "b1c6363f-75aa-40ef-ba8a-58201eb42637", Name = "Финансы"},
                    new Category() {Id = "5d6353a6-414c-4b2a-9eea-53d1223b4d8e", Name = "Гос.служба"},
                    new Category() {Id = "aba5c774-05ca-49ac-bcd9-3fa79583db98", Name = "Инф.технологии"}
                };
                context.AddRange(categories);
                context.SaveChanges();
            }
        }
    }
}