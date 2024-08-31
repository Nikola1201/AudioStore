
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ShoppingCartService _shoppingCartService;


        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, ShoppingCartService shoppingCartService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _shoppingCartService = shoppingCartService;
        }
     
        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<Product> products = await _unitOfWork.Product.GetAllProducts();
            return View(products);
        }

        //GET
        public async Task<IActionResult> Details(int id)
        {
            ShoppingCartItem obj = new ShoppingCartItem()
            {
                Count = 1,
                Product = await _unitOfWork.Product.GetProductById(id),
                ProductID = id
            };
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsAsync(ShoppingCartItem obj)
        {
            var cartId = _shoppingCartService.GetOrCreateCartId();
            var cart = _shoppingCartService.GetCart(cartId) ?? new List<ShoppingCartItem>();

            foreach (var c in cart)
            {
                if (obj.Id == c.Id)
                {
                    c.Count += obj.Count;
                    return RedirectToAction(nameof(Index));
                }
            }

            obj.Product = await _unitOfWork.Product.GetProductById(obj.ProductID);
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
