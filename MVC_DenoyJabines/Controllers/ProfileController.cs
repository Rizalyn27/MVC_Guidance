using Microsoft.AspNetCore.Mvc;

namespace MVC_DenoyJabines.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
