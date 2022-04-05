using Microsoft.AspNetCore.Mvc;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
