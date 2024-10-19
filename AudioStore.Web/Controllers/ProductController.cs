using AudioStore.Models;
using AudioStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AudioStore.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _webHost;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHost = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        //GET
        public async Task<IActionResult> UpsertAsync(int? id)
        {

            Product product = new Product();

            if (id == null || id == 0)
            {
                return View(product);
            }
            else
            {
                product = await _unitOfWork.Product.GetSingleOrDefaultAsync(p => p.ProductID == id);
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
        public async Task<IActionResult> Upsert(Product obj, IFormFile file)
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
                   await _unitOfWork.Product.AddAsync(obj);
                    TempData["success"] = "Product created successfully!";
                }
                else
                {
                    _unitOfWork.Product.UpdateProduct(obj);
                    TempData["success"] = "Product updated successfully!";
                }
               await _unitOfWork.SaveAsync();

                return RedirectToAction("Index");

            }

            TempData["error"] = "Error updating product";

            return View(obj);
        }


        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Product> products = await _unitOfWork.Product.GetAllAsync();
            return Json(new { data = products });
        }

        [HttpGet]
        public async Task<IActionResult> GetSubCategories(int superCategoryId)
        {
            var subCategories = await _unitOfWork.Category.GetAllSubCategories(superCategoryId);
            return Json(new { data = subCategories });
        }
        [HttpGet]
        public async Task<IActionResult> GetManufacturers()
        {
            var manufacturers = await _unitOfWork.Manufacturer.GetAllAsync();
            return Json(new { data = manufacturers });
        }
        [HttpGet]
        public async Task<IActionResult> GetSuperCategories()
        {
            var superCategories = await _unitOfWork.Category.GetAllSuperCategories();
            return Json(new { data = superCategories });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var obj = await _unitOfWork.Product.GetSingleOrDefaultAsync(p => p.ProductID == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            var odlImagePath = Path.Combine(_webHost.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(odlImagePath))
            {
                System.IO.File.Delete(odlImagePath);
            }
            _unitOfWork.Product.Remove(obj);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion

    }
}

