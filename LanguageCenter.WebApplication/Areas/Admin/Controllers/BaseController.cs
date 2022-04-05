using LanguageCenter.Infrastructure.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanguageCenter.WebApplication.Areas.Admin.Controllers
{
    [Authorize(Roles = Constraints.Role.Admin)]
    [Area(Constraints.Role.Admin)]
    public class BaseController : Controller
    {
    }
}
