
using BulkyBook.Data.UnitOfWork;
using BulkyBook.Model;
using BulkyBook.Utility;
using BulkyBook.Website.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Website.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(SD.Role_Admin)]
    public class CompanyController : Controller
    {
        #region DBContext
        private readonly IUnitOfWork unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        #endregion

        #region Display All Categories
        [HttpGet]
        public IActionResult Index()
        {
            var Companys = unitOfWork.Companies.GetAll();
            return View(Companys);
        }
        #endregion

        #region Update & Create
        [HttpGet]
        public IActionResult Upsert(int id)
        {
            if (id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company companyObj = unitOfWork.Companies.GetById(id);
                return View(companyObj);
            }

        }

        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            if (ModelState.IsValid)
            {
                if (companyObj.Id == 0)
                {
                    unitOfWork.Companies.Add(companyObj);
                    TempData["success"] = "Company created successfully";
                }
                else
                {
                    unitOfWork.Companies.Update(companyObj);
                    TempData["success"] = "Company Updated successfully";
                }
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(companyObj);
            }
        }
        #endregion

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = unitOfWork.Companies.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }
        #endregion

        #region Delete
        //[HttpDelete]
        //public IActionResult Delete(int id)
        //{
        //    var DeleteCompany = unitOfWork.Companys.GetById(id);
        //    if (DeleteCompany == null)
        //        return NotFound();
        //    return View(DeleteCompany);
        //}
        //[HttpPost, ActionName("Delete")]
        //public IActionResult ConfirmDelete(int id)
        //{
        //    var DeleteCompany = unitOfWork.Companys.GetById(id);
        //    if (DeleteCompany == null)
        //        return NotFound();
        //    unitOfWork.Companys.Delete(DeleteCompany);
        //    unitOfWork.Save();
        //    TempData["success"] = "Category deleted successfully";
        //    return RedirectToAction(nameof(Index));
        //}
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var CompanyToBeDeleted = unitOfWork.Companies.GetById(id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            unitOfWork.Companies.Delete(CompanyToBeDeleted);
            unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}  
