using AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository;
using AlmedalGameStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AlemedalGameStore.Utility;
using AlmedalGameStore.Models;

namespace AlmedalGameStoreWeb.Areas.Guest.Controllers
{

   [Area("Customer")]
   [Authorize]
    //[AllowAnonymous]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        public int OrderTotal { get; set; }
        [BindProperty]
        public CartVM CartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

     
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            CartVM = new CartVM()
            {
                ListCart = _unitOfWork.Cart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product"),
                Order = new()
            };
            foreach (var cart in CartVM.ListCart)
            {
                cart.Price = GetPrice(cart.Count, cart.Product.Price);
                CartVM.Order.OrderTotal += (cart.Price * cart.Count);
            }
            return View(CartVM);
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            //Hämtar all data från användare till Checkout + Checkout view
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            CartVM = new CartVM()
            {
                ListCart = _unitOfWork.Cart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product"),
                Order = new()
            };
            CartVM.Order.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
            CartVM.Order.Name = CartVM.Order.ApplicationUser.Name;
            CartVM.Order.Address = CartVM.Order.ApplicationUser.StreetAddress;
            CartVM.Order.PostalCode = CartVM.Order.ApplicationUser.PostalCode;
            //Stad - Saknas i Order? (Har en address , en street, är det samma?)
            CartVM.Order.Street = CartVM.Order.ApplicationUser.City;
           //Fraktmetod saknas applicationUser?

            foreach (var cart in CartVM.ListCart)
            {
                cart.Price = GetPrice(cart.Count, cart.Product.Price);
                CartVM.Order.OrderTotal += (cart.Price * cart.Count);
            }


            return View(CartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StripeCheckoutPOST()
        {
            var orderId = Guid.NewGuid();
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var ListCart = _unitOfWork.Cart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product");

            foreach (var cart in ListCart)
            {
                Order order = new()
                {
                    OrderId = orderId,
                    ProductId = cart.ProductId,
                    Price = cart.Product.Price,
                    Amount = cart.Count,
                    Address = CartVM.Order.Address,
                    Name = CartVM.Order.Name,
                    PostalCode = CartVM.Order.PostalCode,
                    Street = CartVM.Order.Street,
                    OrderDate = DateTime.Now,
                    ApplicationUserId = claim.Value,
                    PaymentMethod = Enums.PaymentMethod.CreditCard,
                    Status = Enums.OrderStatus.Started
                };
                _unitOfWork.Order.Add(order);
                _unitOfWork.Cart.Remove(cart);
            }

            // ToDo: Implementera tömning av kundvagn
            
            _unitOfWork.Save();

            // ToDo: View funkar inte men det löser sig i och med stripe

            return View(CartVM);
        }

        [HttpPost]
        public IActionResult CheckoutPOST()
        {
            //Stripe inställningar

            //Swish inställningar

            //Fysisk inställningar


            return View();
        }




        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.Cart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.Cart.PlusCount(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.Cart.GetFirstOrDefault(u => u.Id == cartId);
            if (cart.Count <= 1)
            {
                _unitOfWork.Cart.Remove(cart);
            }
            else
            {
                _unitOfWork.Cart.MinusCount(cart, 1);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.Cart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.Cart.Remove(cart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private double GetPrice(double quantity, double price)
        {
            return price;
        }
    }
}
