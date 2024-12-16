using Microsoft.AspNetCore.Mvc;

namespace KadınKuaforu.Areas.Panel.Controllers
{
    [Area("Panel")]
    public class CompanyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
