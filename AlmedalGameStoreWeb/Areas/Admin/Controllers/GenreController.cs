
using AlmedalGameStore.Models;
using AlmedalGameStore.DataAccess;
using Microsoft.AspNetCore.Mvc;
using AlmedalGameStore.DataAccess;
using AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository;

namespace AlmedalGameStoreWeb.Controllers
{
    [Area("Admin")]
    //Dependecy Injections från program.cs använder vi databas objekt genom applicationDbContext
    //Vi vill att ApplicationDbContext ska arbeta med vår databas genom detta nedan
    
    public class GenreController : Controller
    {
        //Dependecy Injections från program.cs använder vi databas objekt genom applicationDbContext
        //Vi vill att ApplicationDbContext ska arbeta med vår databas genom detta nedan
        private readonly IUnitOfWork _unitOfWork;
        //vi populerar vår lokala databas objekt med detta
        //använder dependency injections för att hämta unitofWork
        public GenreController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //Går till databasen & hämtar alla genres och konvertar till en lista (objGenreList)
            IEnumerable<Genre> objGenreList = _unitOfWork.Genre.GetAll();
            //Vi kan skicka in vår lista till våran VY för att kunna displaya det
            return View(objGenreList);
        }
        //GET 
        public IActionResult Create()
        {
            //Skapa en model inuti vyn
            return View();
        }
        //POST
        //attrivbute
        [HttpPost]
        //injektar en key som blir validated i post metod automatiskt
        //är till för att hålla det mer säkert, den måste vara valid för att förhindra forgery
        [ValidateAntiForgeryToken]
        public IActionResult Create(Genre obj)
        {
           
            //för att se att ens model är valid med .net core
            if (ModelState.IsValid)
            {
                //för att spara detta till vår databas så gör vi så här
                _unitOfWork.Genre.Add(obj);
                // det "pushas" först till databasen när man kör SaveChanges
                _unitOfWork.Save();
                TempData["success"] = "Genre Skapad!";
                //Skapa en model inuti vyn
                //När man skapat sin genre typ så redirectar vi användaren till Index
                //Som den letar efter i samma controller man skirver det i
                //SKULLE man vilja gå till en annan Action Method i en annan controller så skriver man
                //return RedirectToAction("Index","SkrivControllerHär");
                return RedirectToAction("Index");
            }
            //om den inte är valid (inte uppfyller required i model)
            return View(obj);
        }
        //GET
        
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //finds category based of ID and assigns to a variable
            //var genreFromDb = _db.Genres.Find(id);
            //hittar där namn == id
            var genreFromDb = _unitOfWork.Genre.GetFirstOrDefault(u => u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);


            //if category is null return notfound
            if (genreFromDb == null)
            {
                return NotFound();
            }
            //returns category to view
            return View(genreFromDb);
        }
        [HttpPost]
        //injektar en key som blir validated i post metod automatiskt
        //är till för att hålla det mer säkert, den måste vara valid för att förhindra forgery
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Genre obj)
        {
          
            //för att se att ens model är valid med .net core
            if (ModelState.IsValid)
            {
                //Vid update kollar den på obj och hittar dess PK och hämta den från databasen och checkar alla properties där värdet har ändrats och updaterar det
                _unitOfWork.Genre.Update(obj);
                // det "pushas" först till databasen när man kör SaveChanges
                _unitOfWork.Save();
                TempData["success"] = "Genre Uppdaterad!";
                //Skapa en model inuti vyn
                //När man skapat sin genre typ så redirectar vi användaren till Index
                //Som den letar efter i samma controller man skirver det i
                //SKULLE man vilja gå till en annan Action Method i en annan controller så skriver man
                //return RedirectToAction("Index","SkrivControllerHär");
                return RedirectToAction("Index");
            }
            //om den inte är valid (inte uppfyller required i model)
            return View(obj);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //finds category based of ID and assigns to a variable
            //var genreFromDb = _db.Genres.Find(id);
            var genreFromDb = _unitOfWork.Genre.GetFirstOrDefault(u => u.Id == id);


            //if category is null return notfound
            if (genreFromDb == null)
            {
                return NotFound();
            }
            //returns category to view
            return View(genreFromDb);
        }
        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            //hittar obj baserad på ID
            var obj = _unitOfWork.Genre.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Genre.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Genre Raderat!";
            return RedirectToAction("Index");

           

        }
    }
}
