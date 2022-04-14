using AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository;
using AlmedalGameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlmedalGameStore.DataAccess.GenericRepository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        //pass db till base class
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
      

        public void Update(Product obj)
        {
            //när man hämtar så trackar entity framework detta objFromDb om de inte är tomt kan man 
            //updata vissa propertys, ex title osv.
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id==obj.Id);
            if(objFromDb != null)
            {
                //hämtar alla propertys förutom image, för att då måste man välja bild varje gång man ska edita något
                //Annars blir bilden tom 
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.Publisher = obj.Publisher;
                objFromDb.SystemReq = obj.SystemReq;
                objFromDb.PgRating = obj.PgRating;
                objFromDb.ReleaseDate = obj.ReleaseDate;
                objFromDb.Price = obj.Price;
                objFromDb.GenreId = obj.GenreId;
                if(obj.ImageUrl !=null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }

            }
        }
    }
}
