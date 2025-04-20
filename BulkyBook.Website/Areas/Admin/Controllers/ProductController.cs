using BulkyBook.Data.UnitOfWork;
using BulkyBook.Models;
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
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CatogeryList = unitOfWork.Categories.GetAll().Select(s => new SelectListItem
            {
                Value = s.Name,
                Text = s.ID.ToString()  // Ana Hna convert the list into SelectListItem 
            });
            ViewBag.Categories = CatogeryList;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Products.Add(product);
                unitOfWork.Save();
                TempData["success"] = "Products Created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        #endregion

        #region Update
        [HttpGet]
        public IActionResult Update(int id)
        {
            if (id == null || id == 0)
                return NotFound();
            var Product = unitOfWork.Products.GetById(id);
            if (Product == null)
                return NotFound();
            return View(Product);
        }
        [HttpPost]
        public IActionResult Update(int id, Product product)
        {
            //if (product.Id != id || product is null)
            //{
            //    return BadRequest();
            //}
            //if (product.Title == product.Id.ToString())
            //{
            //    ModelState.AddModelError(nameof(product.Title), "The Display Order cannot exactly match the Name");
            //}
            if (ModelState.IsValid)
            {
                if (product.Title == product.Id.ToString())
                {
                    ModelState.AddModelError(nameof(product.Title), "The Display Order cannot exactly match the Name");
                }
                unitOfWork.Products.Update(product);
                unitOfWork.Save();
                TempData["success"] = "product updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
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
