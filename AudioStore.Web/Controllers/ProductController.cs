using AudioStore.Services;
using AudioStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AudioStore.Web.Controllers
{
    public class ProductController : Controller
    {
        private IProductService ProductService { get; }
        public ProductController(IProductService productService)
        {
            ProductService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }


        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productVMs = await ProductService.GetProductVMs();
            return Json(new { data = productVMs });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = ProductService.GetProductById(id).Result;
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            ProductService.DeleteProduct(id);
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion

    }
}

