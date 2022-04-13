
using AlemedalGameStore.Utility;
using AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository;
using AlmedalGameStore.Models;
using AlmedalGameStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace AlmedalGameStoreWeb.Controllers
{

    [Area("Customer")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Genre");

            return View(productList);
        }
        public IActionResult Details(int productId)
        {
            Cart cartObj = new()
            {
                //Shoppingcart
                Count = 1,
                ProductId = productId,
                Product = _unitOfWork.Product.GetFirstOrDefault
                (u => u.Id == productId, includeProperties: "Genre")
            };
            return View(cartObj);

        }
        //makes only loged in users to add items to cart
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]


        public IActionResult Details(Cart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            Cart cartFromDb = _unitOfWork.Cart.GetFirstOrDefault(
                   u => u.ApplicationUserId == claim.Value && u.ProductId == shoppingCart.ProductId);

            if (cartFromDb == null)
            {
                _unitOfWork.Cart.Add(shoppingCart);
            }
            else
            {
                _unitOfWork.Cart.PlusCount(cartFromDb, shoppingCart.Count);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));

            Cart cartObj = new()
            {
                //Shoppingcart
                Count = 1,
                //Course = _unitOfWork.Course.GetFirstOrDefault(u => u.Id == id, includeProperties: "Category,ClassType")
            };
            return View(cartObj);

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