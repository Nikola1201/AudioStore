using AudioStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AudioStore.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private List<ShoppingCartVM> Cart { get; } = new List<ShoppingCartVM>();
        public ShoppingCartController(List<ShoppingCartVM> cart)
        {
            Cart = cart;
        }

        public IActionResult Index()
        {
            return View(Cart);
        }
       
    }
}
