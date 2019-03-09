using System.Collections.Generic;
using System.Threading.Tasks;
using CoreDemo.Models;

namespace CoreDemo.Services
{
    public interface ICinemaService
    {
        Task<IEnumerable<Cinema>> GetllAllAsync();
        Task<Cinema> GetByIdAsync(int id);
        Task AddAsync(Cinema model);
    }
}