using Microsoft.AspNetCore.Mvc;

namespace mev_8.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
