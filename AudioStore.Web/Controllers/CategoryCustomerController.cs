using Microsoft.AspNetCore.Mvc;

namespace AudioStore.Web.Controllers
{
    public class CategoryCustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
