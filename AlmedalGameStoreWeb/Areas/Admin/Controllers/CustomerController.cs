using AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository;
using Microsoft.AspNetCore.Mvc;

namespace AlmedalGameStoreWeb.Areas.Admin.Controllers;
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
        var userList = _unitOfWork.ApplicationUser.GetAll(includeProperties: "Role");

        return Json(new { data = userList });
    }
}
