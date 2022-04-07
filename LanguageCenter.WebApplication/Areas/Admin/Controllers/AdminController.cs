using LanguageCenter.Core.Models.CourseModels;
using LanguageCenter.Core.Services.Contracts;
using LanguageCenter.Infrastructure.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    public class AdminController : BaseController
    {
        
        public AdminController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
