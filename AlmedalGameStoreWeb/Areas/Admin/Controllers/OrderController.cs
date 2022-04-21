using AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository;
using AlmedalGameStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlmedalGameStoreWeb.Controllers
{
    [Area("Admin")]

    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _hostEnviorment;

        public OrderController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnviorment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ListOrder = _unitOfWork.Order.GetAll(o => o.OrderId == id, includeProperties: "Product");

            if (ListOrder == null)
            {
                return NotFound();
            }
            double totalSum = 0;
            foreach(Order o in ListOrder)
            {
                totalSum += o.Amount * o.Price;
            }
            ListOrder.ToList()[0].OrderTotal = totalSum;
            return View(ListOrder.ToList());
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var orderList = _unitOfWork.Order.GetAll().AsQueryable().DistinctBy(o => o.OrderId);

            foreach (var order in orderList)
            {
                order.OrderStatus = (int)order.Status switch
                {
                    0 => "Mottagen",
                    1 => "Påbörjad",
                    2 => "Skickad",
                    _ => "Felstatus",
                };
            }
            
            return Json(new { data = orderList });
        }

        #endregion

    }
}
