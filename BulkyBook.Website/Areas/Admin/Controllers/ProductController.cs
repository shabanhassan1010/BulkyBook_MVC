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
        private readonly IUnitOfWork unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region Display All Categories
        [HttpGet]
        public IActionResult Index()
        {
            var Products = unitOfWork.Products.GetAll();          
            return View(Products);
        }
        #endregion

        #region Create
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
        public IActionResult Create(Product product, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                // Handle image upload
                if (file != null && file.Length > 0)
                {
                    // Validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(file.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ImageUrl", "Only image files (jpg, jpeg, png) are allowed");
                        return View(product);
                    }

                    // Validate file size (2MB max)
                    if (file.Length > 2 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ImageUrl", "File size cannot exceed 2MB");
                        return View(product);
                    }

                    // Create upload directory if not exists
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Generate unique filename
                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadPath, fileName);

                    // Save file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    // Store relative path
                    product.ImageUrl = $"/images/products/{fileName}";
                }

                unitOfWork.Products.Add(product);
                unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction(nameof(Index));
            }

            // Repopulate categories if validation fails
            ViewBag.Categories = unitOfWork.Categories.GetAll()
                .Select(s => new SelectListItem
                {
                    Value = s.ID.ToString(),
                    Text = s.Name
                });

            return View(product);
        }
        #endregion

        #region Update
        //[HttpGet]
        //public IActionResult Update(int id)
        //{
        //    ViewBag.Categories = unitOfWork.Categories.GetAll()
        //        .Select(s => new SelectListItem
        //        {
        //            Value = s.ID.ToString(),  // Use ID for value
        //            Text = s.Name             // Use Name for display
        //        });
        //    if (id == null || id == 0)
        //        return NotFound();
        //    var Product = unitOfWork.Products.GetById(id);
        //    if (Product == null)
        //        return NotFound();
        //    return View(Product);
        //}
        //[HttpPost]
        //public IActionResult Update(int id, Product product)
        //{
        //    //if (product.Id != id || product is null)
        //    //{
        //    //    return BadRequest();
        //    //}
        //    //if (product.Title == product.Id.ToString())
        //    //{
        //    //    ModelState.AddModelError(nameof(product.Title), "The Display Order cannot exactly match the Name");
        //    //}
        //    if (ModelState.IsValid)
        //    {
        //        if (product.Title == product.Id.ToString())
        //        {
        //            ModelState.AddModelError(nameof(product.Title), "The Display Order cannot exactly match the Name");
        //        }
        //        unitOfWork.Products.Update(product);
        //        unitOfWork.Save();
        //        TempData["success"] = "product updated successfully";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //}
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var DeleteProduct = unitOfWork.Products.GetById(id);
            if (DeleteProduct == null)
                return NotFound();
            return View(DeleteProduct);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            var DeleteProduct = unitOfWork.Products.GetById(id);
            if (DeleteProduct == null)
                return NotFound();
            unitOfWork.Products.Delete(DeleteProduct);
            unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
