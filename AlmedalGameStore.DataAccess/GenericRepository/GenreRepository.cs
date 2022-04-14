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
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        private ApplicationDbContext _db;
        //pass db till base class
        public GenreRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
       
        public void Update(Genre obj)
        {
            _db.Genres.Update(obj);
        }
    }
}
