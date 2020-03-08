using System.Linq;

namespace OrkadWeb.Services
{
    public interface IDataService
    {
        IQueryable<T> Query<T>();
    }
}