using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlmedalGameStore.DataAccess.GenericRepository.IGenericRepository
{
    //spelar ingen roll vilken class det är, det måste bara vara en class
    public interface IGenericRepository<T> where T : class
    {
        // T - Genre 
        //När vi bara hämtar en record behöver man allitd ett filter condition
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        //implementera en void av entity som ska adderas
        void Add(T entity);
        void Remove(T entity);
        //Om man vill radera flera entitys
        void RemoveRange(IEnumerable<T> entity);
    }
}
