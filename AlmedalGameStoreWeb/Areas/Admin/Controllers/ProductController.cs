
using AlmedalGameStore.Models;
using AlmedalGameStore.DataAccess;
using Microsoft.AspNetCore.Mvc;
using AlmedalGameStore.DataAccess;
using AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using AlmedalGameStore.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace AlmedalGameStoreWeb.Controllers
{
    [Area("Admin")]
    //Dependecy Injections från program.cs använder vi databas objekt genom applicationDbContext
    //Vi vill att ApplicationDbContext ska arbeta med vår databas genom detta nedan

    public class ProductController : Controller
    {
        //Dependecy Injections från program.cs använder vi databas objekt genom applicationDbContext
        //Vi vill att ApplicationDbContext ska arbeta med vår databas genom detta nedan
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _hostEnviorment;
        //vi populerar vår lokala databas objekt med detta
        //använder dependency injections för att hämta unitofWork
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnviorment = hostEnvironment;
        }

        public IActionResult Index()
        {

            return View();
        }


        public IActionResult Upsert(int? id)
        {
            //Tack vare UnitOfWork kan jag komma åt alla repositorys (här är till en dropdown)
            //Selectlistitem behöver en text och value som behöver populeras
            //Vi hämtar alla genres och convertar dem till en selectlist item
            ProductVM productVM = new()
            {
                Product = new(),
                GenreList = _unitOfWork.Genre.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };


            //Om id = null eller id = 0 då vill vi skapa produkt
            if (id == null || id == 0)
            {
                //ViewBag.GenreList = GenreList;
                return View(productVM);
            }
            else
            {
                //hämtar proudk baserat på id
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
                return View(productVM);
            }

        }
        [HttpPost]
        //injektar en key som blir validated i post metod automatiskt
        //är till för att hålla det mer säkert, den måste vara valid för att förhindra forgery
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {

            //checks if create classtype is valid(Makes so you can´t create empty or false classtype to database)
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnviorment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        //if old imagepath exist we want to delete that so we can add new image
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    //copy the files to fileStreams
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                //If Course Id = 0 then we add
                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                }
                //else update
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }

                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            //hämtar alla produkter till productList och convertar dem till Json
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Genre");
            return Json(new { data = productList });
        }
        //POST
        [HttpDelete]

        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldImagePath = Path.Combine(_hostEnviorment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            //if old image exists delete it
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });




        }
        #endregion
    }
}
