using System.Linq;
using HeadHunter.Models;
using Microsoft.AspNetCore.Mvc;

namespace HeadHunter.Controllers
{
    public class ValidationController : Controller
    {
        private HeadHunterContext _db;

        public ValidationController(HeadHunterContext db)
        {
            _db = db;
        }

        public bool CheckCategory(string categoryId)
        {
            if (categoryId != null)
            {
                Category category = _db.Categories.FirstOrDefault(c => c.Id == categoryId);
                if (category.Name.Contains("выбрано")) return false;
                else return true;
            }
            return false;
        }
    }
}