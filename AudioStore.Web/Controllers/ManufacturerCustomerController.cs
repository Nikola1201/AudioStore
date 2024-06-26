using Microsoft.AspNetCore.Mvc;

namespace AudioStore.Web.Controllers
{
    public class ManufacturerCustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
