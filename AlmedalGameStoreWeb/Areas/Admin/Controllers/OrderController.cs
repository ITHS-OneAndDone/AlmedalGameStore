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


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var orderList = _unitOfWork.Order.GetAll().AsQueryable().DistinctBy(o => o.OrderId);
            return Json(new { data = orderList });
        }

        #endregion

    }
}
