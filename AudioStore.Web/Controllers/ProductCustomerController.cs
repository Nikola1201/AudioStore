using AudioStore.Models;
using AudioStore.Models.ViewModels;
using AudioStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AudioStore.Web.Controllers
{
    public class ProductCustomerController : Controller
    {
        private IProductService ProductService { get; }
        private List<ShoppingCartVM> Cart { get; } = new List<ShoppingCartVM>();
        public ProductCustomerController(IProductService productService, List<ShoppingCartVM> cart)
        {
            ProductService = productService;
            Cart = cart;
        }
        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<Product> products = await ProductService.GetAllProducts();
            return View(products);
        }

        //GET
        public async Task<IActionResult> Details(int id)
        {
            ShoppingCartVM obj = new ShoppingCartVM()
            {
                Count = 1,
                Product = await ProductService.GetProductById(id),
                ProductID = id
            };
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsAsync(ShoppingCartVM obj)
        {
            foreach (var c in Cart)
            {
                if (obj.Id == c.Id)
                {
                    c.Count += obj.Count;
                    return RedirectToAction(nameof(Index));
                }
            }
            obj.Product = await ProductService.GetProductById(obj.ProductID);

            Cart.Add(obj);

            return RedirectToAction(nameof(Index));


        }
    }
}
