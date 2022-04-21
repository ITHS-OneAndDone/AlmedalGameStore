using AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository;
using AlmedalGameStore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AlemedalGameStore.Utility;
using AlmedalGameStore.Models;
using Stripe.Checkout;
using AlmedalGameStore.DataAccess.GenericRepository;


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
        [ActionName("Checkout")]
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
                    Status = Enums.OrderStatus.Received
                };
                _unitOfWork.Order.Add(order);
                _unitOfWork.Save();
                //_unitOfWork.Cart.Remove(cart);
            }
            //STRIPE
            var domian = "https://" + HttpContext.Request.Host.Value + "/";
            var options = new SessionCreateOptions
            {
                //representerar alla items i cart (lineitems)
                LineItems = new List<SessionLineItemOptions>()
              ,
                Mode = "payment",
                SuccessUrl = domian+$"customer/cart/StripeOrderConfirmation?id={orderId}",
                CancelUrl = domian+$"customer/cart/index" ,
            };
            foreach(var item in ListCart)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "SEK",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title,
                            Description = item.Product.Description,

                        },
                       
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);

            }  
            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.Order.UpdateStripeId(orderId, session.Id);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult StripeOrderConfirmation(Guid id)
        {

            Order order = _unitOfWork.Order.GetFirstOrDefault(u => u.OrderId == id);
            var service = new SessionService();
            Session session = service.Get(order.SessionId);
            List<Cart> carts = _unitOfWork.Cart.GetAll(u => u.ApplicationUserId == order.ApplicationUserId).ToList();
            _unitOfWork.Cart.RemoveRange(carts);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return View(id);
        }



        //Betala i butik vy
        public IActionResult CashCheckoutViewGet()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            CartVM = new CartVM()
            {
                ListCart = _unitOfWork.Cart.GetAll(u => u.ApplicationUserId == claim.Value,
                    includeProperties: "ApplicationUser,Product"),
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
        public IActionResult CashCeckoutPOST()
        {
            var orderId = Guid.NewGuid();
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var ListCart = _unitOfWork.Cart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product,ApplicationUser");

            foreach (var cart in ListCart)
            {
                Order order = new()
                {
                    OrderId = orderId,
                    ProductId = cart.ProductId,
                    Price = cart.Product.Price,
                    Amount = cart.Count,
                    Address = "Hämta i butik",
                    Name = cart.ApplicationUser.Name,
                    PostalCode = "-",
                    Street = "-",
                    OrderDate = DateTime.Now,
                    ApplicationUserId = claim.Value,
                    PaymentMethod = Enums.PaymentMethod.InStore,
                    Status = Enums.OrderStatus.Started
                };
                _unitOfWork.Order.Add(order);
                _unitOfWork.Cart.Remove(cart);
                _unitOfWork.Save();                
            }

            return View("CashCheckoutPOST");
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
