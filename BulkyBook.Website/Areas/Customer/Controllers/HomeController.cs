using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.Model;
using BulkyBook.Data.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BulkyBook.Website.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;
        public HomeController(ILogger<HomeController> logger ,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var product = unitOfWork.Products.GetAllIncluding();
            return View(product);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            var product = unitOfWork.Products.GetByIdIncluding(Id);

            if (product == null)
                return NotFound();

            var shoppingCart = new ShopingCart() 
            {
                Product = product,
                ProductId = Id,
                Count = 1
            };

            return View(shoppingCart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShopingCart model)
        {
            if (!ModelState.IsValid)
            {
                // Reload product data if validation fails
                model.Product = unitOfWork.Products.GetByIdIncluding(model.ProductId);
                return View(model);
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            model.ApplicationUserId = userId;

            ShopingCart CartFromDb = unitOfWork.shoppingCartRepository.GetFirstOrDefault(x=> x.ApplicationUserId == userId && x.ProductId == model.ProductId);

            if (CartFromDb != null)
            {
                CartFromDb.Count += model.Count;
                unitOfWork.shoppingCartRepository.Update(CartFromDb);
                TempData["success"] = "Cart Updated Successfully";
            }
            else
            {
                model.Id = 0;
                unitOfWork.shoppingCartRepository.Add(model);
                TempData["success"] = "Cart Added Successfully";
            }


            unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
