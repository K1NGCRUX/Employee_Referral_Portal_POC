using Microsoft.AspNetCore.Mvc;

namespace EmpRefPortal_POC.Controllers
{
    public class AdminAPIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
