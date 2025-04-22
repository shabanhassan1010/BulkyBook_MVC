using BulkyBook.Data.UnitOfWork;
using BulkyBook.Model;
using BulkyBook.Website.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        #region DBContext
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork unitOfWork;
        public ProductController(IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region Display All Categories
        [HttpGet]
        public IActionResult Index()
        {
            var Products = unitOfWork.Products.GetAllIncluding();          
            return View(Products);
        }
        #endregion

        #region Update & Create
        [HttpGet]
        public IActionResult Upsert(int id)
        {
            ProductVM productVM = new()
            {
                CategoryList = unitOfWork.Categories.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = unitOfWork.Products.GetById(id);
                return View(productVM);
            }

        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRoot = _webHostEnvironment.WebRootPath; // WebRootPath: give me the basic root folder
                if(file != null)       // then must uplaod the file and save it in -> Images\product File
                { 
                    string FileName = Guid.NewGuid().ToString()  + Path.GetExtension(file.FileName);  // give me Random name for the file (Final Image)
                    string ProductPath = Path.Combine(wwwRoot, @"Images\Products");  // give me the path inside product folder (Location)

                    // delete The old Image
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var OldImagePath = Path.Combine(wwwRoot,productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(OldImagePath))
                        {
                            System.IO.File.Delete(OldImagePath);
                        }
                    }

                    // Upload The Image
                    using (var fileStream = new FileStream(Path.Combine(ProductPath, FileName), FileMode.Create))  // SAve Image
                    {
                        file.CopyTo(fileStream);  // copy the file in the new location that add it
                    }

                    // Update The Image Url
                    productVM.Product.ImageUrl = @"\Images\Products\" + FileName;
                }
                if(productVM.Product.Id == 0)
                {
                    unitOfWork.Products.Add(productVM.Product);
                    TempData["success"] = "Product created successfully";
                }
                else
                {
                    unitOfWork.Products.Update(productVM.Product);
                    TempData["success"] = "Product Updated successfully";
                }
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = unitOfWork.Categories.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString()
                });
                return View(productVM);
            }
        }
        #endregion

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = unitOfWork.Products.GetAllIncluding().ToList();
            return Json(new { data = objProductList });
        }
        #endregion

        #region Delete
        //[HttpDelete]
        //public IActionResult Delete(int id)
        //{
        //    var DeleteProduct = unitOfWork.Products.GetById(id);
        //    if (DeleteProduct == null)
        //        return NotFound();
        //    return View(DeleteProduct);
        //}
        //[HttpPost, ActionName("Delete")]
        //public IActionResult ConfirmDelete(int id)
        //{
        //    var DeleteProduct = unitOfWork.Products.GetById(id);
        //    if (DeleteProduct == null)
        //        return NotFound();
        //    unitOfWork.Products.Delete(DeleteProduct);
        //    unitOfWork.Save();
        //    TempData["success"] = "Category deleted successfully";
        //    return RedirectToAction(nameof(Index));
        //}
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productToBeDeleted = unitOfWork.Products.GetById(id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                               productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            unitOfWork.Products.Delete(productToBeDeleted);
            unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
