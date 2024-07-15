using AudioStore.Models;

using AudioStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AudioStore.Web.Controllers
{
    public class ProductCustomerController : Controller
    {
        private IProductService ProductService { get; }
        private readonly ShoppingCartService _shoppingCartService;
        public ProductCustomerController(IProductService productService, ShoppingCartService shoppingCartService)
        {
            ProductService = productService;
            _shoppingCartService = shoppingCartService;

        }
        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<Product> products = await ProductService.GetAllProducts();
            return View(products);
        }

        //GET
        public async Task<IActionResult> Details(int id)
        {
            ShoppingCart obj = new ShoppingCart()
            {
                Count = 1,
                Product = await ProductService.GetProductById(id),
                ProductID = id
            };
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsAsync(ShoppingCart obj)
        {
            var cartId = _shoppingCartService.GetOrCreateCartId();
            var cart = _shoppingCartService.GetCart(cartId) ?? new List<ShoppingCart>();

            foreach (var c in cart)
                {
                    if (obj.Id == c.Id)
                    {
                        c.Count += obj.Count;
                        return RedirectToAction(nameof(Index));
                    }
                }
           
            obj.Product = await ProductService.GetProductById(obj.ProductID);
            obj.Price = obj.Product.Price;
            _shoppingCartService.AddToCart(obj);

            return RedirectToAction(nameof(Index));


        }
    }
}
