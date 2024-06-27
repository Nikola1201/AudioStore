using AudioStore.Models;
using AudioStore.Models.ViewModels;
using AudioStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AudioStore.Web.Controllers
{
    public class CategoryController : Controller
    {
        public ICategoryService CategoryService { get; }
        private readonly IWebHostEnvironment _webHost;

        public CategoryController(ICategoryService categoryService, IWebHostEnvironment webHost)
        {
            CategoryService = categoryService;
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
                category = await CategoryService.GetCategoryById(id);
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
                var obj = await CategoryService.GetCategoryById(category.SuperCategoryID);
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
                        //this is an edit and we need to remove old image
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
                    CategoryService.CreateCategory(category);
                    TempData["success"] = "Category created successfully!";
                }
                else
                {
                    CategoryService.UpdateCategory(category);
                    TempData["success"] = "Category updated successfully!";
                }
                RedirectToAction("Index");
            }
            return View(category);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = CategoryService.GetCategoriesVM().Result;
            return Json(new { data = categories });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = CategoryService.GetCategoryById(id).Result;
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            CategoryService.DeleteCategory(id);
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion

    }
}
