using ptm_dev_test.Dtos;
using ptm_dev_test.Models;

namespace ptm_dev_test.Services.IServices
{
    public interface IExamesService
    {
        Task<ExamesModel> CreateExameAsync(ExamesDto exame);
        Task<ExamesModel> GetExameByIdAsync(long id);
        Task<object> GetExamesAsync(string? nome, int? idade, string? genero, int pageNumber, int pageSize);
    }
}
