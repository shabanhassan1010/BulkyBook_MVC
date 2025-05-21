using BulkyBook.Data.UnitOfWork;
using BulkyBook.Model;
using BulkyBook.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBook.Website.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Cart()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = unitOfWork.shoppingCartRepository.GetAll(x => x.ApplicationUserId == userId , includeProperties:"Product")
            };
            foreach(var cart in ShoppingCartVM.ShoppingCartList)
            {
                 cart.Price = GetPriceBasedInQuntity(cart);
                ShoppingCartVM.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }
        public IActionResult Plus(int cartId)
        {
            var cartFromDb = unitOfWork.shoppingCartRepository.GetById(cartId);
            cartFromDb.Count += 1;
            unitOfWork.shoppingCartRepository.Update(cartFromDb);
            unitOfWork.Save();
            return RedirectToAction(nameof(Cart));
        }
        public IActionResult Minus(int cartId)
        {
            var cartFromDb = unitOfWork.shoppingCartRepository.GetById(cartId);

            if (cartFromDb.Count <= 1)
            {
                //remove that from cart
                unitOfWork.shoppingCartRepository.Delete(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                unitOfWork.shoppingCartRepository.Update(cartFromDb);
            }

            unitOfWork.Save();
            return RedirectToAction(nameof(Cart));
        }
        public IActionResult Remove(int cartId)
        {
            var cartFromDb = unitOfWork.shoppingCartRepository.GetById(cartId);
            unitOfWork.shoppingCartRepository.Delete(cartFromDb);
            unitOfWork.Save();
            return RedirectToAction(nameof(Cart));
        }
        private double GetPriceBasedInQuntity(ShopingCart shopingCart)
        {
            if (shopingCart.Count <= 50)
                return shopingCart.Product.Price;
            else
            {
                if(shopingCart.Count<= 100)
                    return shopingCart.Product.Price50;
                else
                    return shopingCart.Product.Price100;
            }

        }
        public IActionResult Summary()
        {
            return View();
        }
    }
}
