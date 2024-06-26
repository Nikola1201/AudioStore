using Microsoft.AspNetCore.Mvc;

namespace AudioStore.Web.Controllers
{
    public class ProductCustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
