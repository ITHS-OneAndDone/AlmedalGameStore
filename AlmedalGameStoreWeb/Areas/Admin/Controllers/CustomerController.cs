using AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository;
using Microsoft.AspNetCore.Mvc;

namespace AlmedalGameStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _hostEnviorment;

        public CustomerController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnviorment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _unitOfWork.ApplicationUser.GetAll();
            foreach (var grej in userList)
            {
                Console.WriteLine("grej");
            }

            return Json(new {data = userList});
        }
    }
}
