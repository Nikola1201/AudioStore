using AudioStore.Models;
using AudioStore.Models.ViewModels;
using AudioStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AudioStore.Web.Controllers
{
    public class CategoryController : Controller
    {
        public ICategoryService CategoryService { get; }
        public CategoryController( ICategoryService categoryService)
        {
            CategoryService = categoryService;
        }
        public IActionResult Index()
        {
          

            return View();
        }

        // GET
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                var obj = CategoryService.GetAllSuperCategories().Result.FirstOrDefault(x => x.Name == category.SuperCategory.Name);
                if (obj != null)
                {
                    category.SuperCategory = obj;
                    category.SuperCategoryID = obj.CategoryID;
                }
                CategoryService.CreateCategory(category);
                TempData["success"]= "Category created successfully!";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) return NotFound();

            var category = CategoryService.GetCategoryById(id).Result;
            if (category == null)
            {
                return NotFound();
            }
            else
            {
                category.SuperCategory = CategoryService.GetCategoryById(category.SuperCategoryID).Result;

            }
            return View(category);

        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
           
            if (ModelState.IsValid)
            {
                CategoryService.UpdateCategory(category);
                TempData["success"] = "Category updated successfully!";
                return RedirectToAction("Index");
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
                return Json(new {success=false, message = "Error while deleting!" });
            }
            CategoryService.DeleteCategory(id);
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion

    }
}
