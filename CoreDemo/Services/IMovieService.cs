using System.Collections.Generic;
using System.Threading.Tasks;
using CoreDemo.Models;

namespace CoreDemo.Services
{
    public interface IMovieService
    {
        Task AddAsync(Movie model);
        Task<IEnumerable<Movie>> GetByCinemaAsync(int cinemaId);
    }
}
