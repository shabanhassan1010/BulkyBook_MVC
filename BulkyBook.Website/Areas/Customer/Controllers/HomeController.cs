using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.Model;
using BulkyBook.Data.UnitOfWork;

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
        public IActionResult Details(int productId)
        {
            var product = unitOfWork.Products.GetByIdIncluding(productId);
            return View(product);
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
