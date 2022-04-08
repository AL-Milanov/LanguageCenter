using Microsoft.AspNetCore.Mvc;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    public class AdminController : BaseController
    {
        
        public IActionResult Index()
        {
            return View();
        }

    }
}
