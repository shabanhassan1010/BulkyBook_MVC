using BulkyBook.Data.DBContext;
using BulkyBook.Data.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Website.Controllers
{
    public class CategoryController : Controller
    {
        #region DBContext
        private readonly IRepository<Category> CategoryRepository;

        public CategoryController(ApplicationDBContext context, IRepository<Category> CategoryRepository)
        {
            this.CategoryRepository = CategoryRepository;
        }
        #endregion

        #region Display All Categories
        [HttpGet]
        public IActionResult Index()
        {
            var categories = CategoryRepository.GetAll();
            return View(categories);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError(nameof(category.Name), "The Display Order cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                CategoryRepository.Add(category);
                CategoryRepository.Save();
                TempData["success"] = "Category Created successfully";
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
            var category = CategoryRepository.GetById(id);
            if (category == null)
                return NotFound();
            return View(category);
        }
        [HttpPost]
        public IActionResult Update(int id, Category category)
        {
            if (category.ID != id || category is null)
            {
                return BadRequest();
            }
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError(nameof(category.Name), "The Display Order cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                if (category.Name == category.DisplayOrder.ToString())
                {
                    ModelState.AddModelError(nameof(category.Name), "The Display Order cannot exactly match the Name");
                }
                CategoryRepository.Update(category);
                CategoryRepository.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var DeleteCategory = CategoryRepository.GetById(id);
            if (DeleteCategory == null)
                return NotFound();
            return View(DeleteCategory);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            var DeleteCategory = CategoryRepository.GetById(id);
            if (DeleteCategory == null)
                return NotFound();
            CategoryRepository.Delete(DeleteCategory);
            CategoryRepository.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
