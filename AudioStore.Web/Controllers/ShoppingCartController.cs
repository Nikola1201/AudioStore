using AudioStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AudioStore.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartService _shoppingCartService;

        public ShoppingCartController(ShoppingCartService shoppingCartService)
        {

            _shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {

            var cartId = _shoppingCartService.GetOrCreateCartId();
            var cart = _shoppingCartService.GetCart(cartId);
            return View(cart);
        }
        public IActionResult Minus(int id)
        {
            var cartId = _shoppingCartService.GetOrCreateCartId();
            var cart = _shoppingCartService.GetCart(cartId);
            var cartItem = cart.Find(c => c.Id == id);  
            if (cartItem.Count <= 1)
            {
                cart.Remove(cartItem);
                _shoppingCartService.SetCart(cartId, cart);
            }
            else
            {
                cart.Find(c => c.Id == id).Count--;
                _shoppingCartService.SetCart(cartId, cart);
            }
            return RedirectToAction(nameof(Index));


        }
        public IActionResult Plus(int id)
        {
            var cartId = _shoppingCartService.GetOrCreateCartId();
            var cart = _shoppingCartService.GetCart(cartId);
            var cartItem = cart.Find(c => c.Id == id);
            if(cartItem != null)
            {
                cartItem.Count++;
                _shoppingCartService.SetCart(cartId, cart);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
