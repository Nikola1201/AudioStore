using AudioStore.Models;
using AudioStore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AudioStore.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManufacturerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHost;
        public ManufacturerController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost)
        {
            _unitOfWork = unitOfWork;
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
            if (id == 0 || id == null)
            {
                return View(manufacturer);
            }
            else
            {
                manufacturer = _unitOfWork.Manufacturer.GetSingleOrDefaultAsync(m => m.ManufacturerID == id).Result;
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
        public IActionResult Upsert(Manufacturer manufacturer, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _webHost.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwrootPath, @"images\manufacturers");
                    var extension = Path.GetExtension(file.FileName);
                    if (manufacturer.ImageUrl != null)
                    {
                        //this is an edit and we need to remove old image
                        var imagePath = Path.Combine(wwwrootPath, manufacturer.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    manufacturer.ImageUrl = @"\images\manufacturers\" + fileName + extension;
                }

                if (manufacturer.ManufacturerID == 0)
                {
                    _unitOfWork.Manufacturer.AddAsync(manufacturer);
                    _unitOfWork.Save();
                    TempData["success"] = "Manufacturer created successfully!";
                }
                else
                {
                    _unitOfWork.Manufacturer.UpdateManufacturer(manufacturer);
                    TempData["success"] = "Manufacturer updated successfully!";
                }
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error updating manufacturer";
            return View(manufacturer);
        }


        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var manufacturers = await _unitOfWork.Manufacturer.GetAllAsync();
            return Json(new { data = manufacturers });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Manufacturer.GetSingleOrDefaultAsync(m => m.ManufacturerID == id).Result;
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }
            _unitOfWork.Manufacturer.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful!" });
        }
        #endregion

    }
}

