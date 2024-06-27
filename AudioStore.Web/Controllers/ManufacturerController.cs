using AudioStore.Models;
using AudioStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AudioStore.Web.Controllers
{
    public class ManufacturerController : Controller
    {
        public IManufacturerService ManufacturerService { get; }
        private readonly IWebHostEnvironment _webHost;
        public ManufacturerController(IManufacturerService manufacturerService, IWebHostEnvironment webHost)
        {
            ManufacturerService = manufacturerService;
            _webHost = webHost;
        }

        public IActionResult Index()
        {
            return View();
        }
        // GET
        public IActionResult Upsert(int? id)
        {
            Manufacturer manufacturer = new Manufacturer();
            if (id==0 || id == null)
            {
                return View(manufacturer);
            }
            else
            {
                manufacturer = ManufacturerService.GetManufacturerById(id).Result;
                if (manufacturer == null)
                {
                    return NotFound();
                }
                return View(manufacturer);
            }
        }
        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Manufacturer manufacturer,IFormFile file)
        {
            if (ModelState.IsValid)
            { 
                string wwwrootPath = _webHost.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwrootPath, @"images\manufacturers");
                    var extension = Path.GetExtension(file.FileName);
                    if(manufacturer.ImageUrl != null)
                    {
                        //this is an edit and we need to remove old image
                        var imagePath = Path.Combine(wwwrootPath, manufacturer.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using(var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    manufacturer.ImageUrl = @"\images\manufacturers\" + fileName + extension;
                }
                
                if (manufacturer.ManufacturerID== 0)
                {
                    ManufacturerService.CreateManufacturer(manufacturer);
                    TempData["success"] = "Manufacturer created successfully!";
                }
                else
                {
                    ManufacturerService.UpdateManufacturer(manufacturer);
                    TempData["success"] = "Manufacturer updated successfully!";
                }
                return RedirectToAction("Index");
            }
            return View(manufacturer);
        }


        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var manufacturers = await ManufacturerService.GetAllManufacturers();
            return Json(new { data = manufacturers });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = ManufacturerService.GetManufacturerById(id).Result;
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            ManufacturerService.DeleteManufacturer(id);
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion

    }
}

