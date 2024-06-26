using AudioStore.Models;
using AudioStore.Models.ViewModels;
using AudioStore.Services;
using AudioStore.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AudioStore.Web.Controllers
{
    public class ProductController : Controller
    {
        private IProductService ProductService { get; }
        private ICategoryService CategoryServices { get; }
        private IManufacturerService ManufacturerService { get; }

        private readonly IWebHostEnvironment _webHost;
        public ProductController(IProductService productService, ICategoryService categoryService, IManufacturerService manufacturerService, IWebHostEnvironment webHostEnvironment)
        {
            ProductService = productService;
            CategoryServices = categoryService;
            ManufacturerService = manufacturerService;
            _webHost = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        //GET
        public IActionResult Upsert(int? id)
        {

            Product product = new Product();

            if (id == null || id == 0)
            {
                return View(product);
            }
            else
            {
                product = ProductService.GetProductById(id).Result;
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Product obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHost.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);
                    if (obj.ImageUrl != null)
                    {
                        //this is an edit and we need to remove old image
                        var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.ImageUrl = @"\images\products\" + fileName + extension;
                }
                if (obj.ProductID == 0)
                {
                    ProductService.CreateProduct(obj);
                    TempData["success"] = "Product created successfully!";
                }
                else
                {
                    ProductService.UpdateProduct(obj);
                    TempData["success"] = "Product updated successfully!";
                }

                RedirectToAction("Index");

            }
            return View(obj);
        }


        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Product> products = await ProductService.GetAllProducts();
            return Json(new { data = products });
        }

        [HttpGet]
        public async Task<IActionResult> GetSubCategories(int superCategoryId)
        {
            var subCategories = await CategoryServices.GetAllSubCategories(superCategoryId);
            return Json(new { data = subCategories });
        }
        [HttpGet]
        public async Task<IActionResult> GetManufacturers()
        {
            var manufacturers = await ManufacturerService.GetAllManufacturers();
            return Json(new { data = manufacturers });
        }
        [HttpGet]
        public async Task<IActionResult> GetSuperCategories()
        {
            var superCategories = await CategoryServices.GetAllSuperCategories();
            return Json(new { data = superCategories });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = ProductService.GetProductById(id).Result;
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            var odlImagePath = Path.Combine(_webHost.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(odlImagePath))
            {
                System.IO.File.Delete(odlImagePath);
            }
            ProductService.DeleteProduct(id);
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion

    }
}

