using AudioStore.Models;
using AudioStore.Models.ViewModels;
using AudioStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AudioStore.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHost;

        public CategoryController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost)
        {
            _unitOfWork = unitOfWork;
            _webHost = webHost;

        }
        public IActionResult Index()
        {


            return View();
        }

        // GET
        public async Task<IActionResult> UpsertAsync(int? id)
        {
            Category category = new Category();
            if (id == null || id == 0)
            {
                return View(category);
            }
            else
            {
                category = await _unitOfWork.Category.GetCategoryById(id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertAsync(Category category, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var obj = await _unitOfWork.Category.GetCategoryById(category.SuperCategoryID);
                if (obj != null)
                {
                    category.SuperCategory = obj;
                }
                string wwwRootPath = _webHost.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\categories");
                    var extension = Path.GetExtension(file.FileName);
                    if (category.ImageUrl != null)
                    {
                        // This is an edit and we need to remove old image
                        var imagePath = Path.Combine(wwwRootPath, category.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    category.ImageUrl = @"\images\categories\" + fileName + extension;
                }
                if (category.CategoryID == 0)
                {
                    _unitOfWork.Category.CreateCategory(category);
                    TempData["success"] = "Category created successfully!";

                }
                else
                {
                    _unitOfWork.Category.UpdateCategory(category);
                    TempData["success"] = "Category updated successfully!";
                }
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error updating category";
            return View(category);
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var categories = await _unitOfWork.Category.GetCategoriesVM();
            return Json(new { data = categories });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            var obj = await _unitOfWork.Category.GetCategoryById(id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            await _unitOfWork.Category.DeleteCategory(id);
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion

    }
}
