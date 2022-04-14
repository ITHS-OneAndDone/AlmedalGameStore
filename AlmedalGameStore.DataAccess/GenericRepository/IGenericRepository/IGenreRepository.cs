using AlmedalGameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//update eller custom message som har annan implementation i andra repositorys skrivs här inne

namespace AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository
{
    //specifierar att när vi använder genreRepos så är model Genre, ex : gör vi en GetAll så hämtar det alla Genres
    //Så här får GenreRepo får all methods som är implementerad i GenericRepository
    public interface IGenreRepository : IGenericRepository<Genre>
    {
        void Update(Genre obj);
        
        
    }
}
