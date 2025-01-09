using Microsoft.AspNetCore.Mvc;

namespace StripsRest.Controllers
{
    public class StripsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
