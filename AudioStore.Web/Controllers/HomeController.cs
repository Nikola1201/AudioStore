
using AudioStore.Models;
using AudioStore.Services.Interfaces;
using AudioStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AudioStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IProductService ProductService { get; }
        private readonly ShoppingCartService _shoppingCartService;


        public HomeController(ILogger<HomeController> logger, IProductService productService, ShoppingCartService shoppingCartService)
        {
            _logger = logger;
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
    

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
