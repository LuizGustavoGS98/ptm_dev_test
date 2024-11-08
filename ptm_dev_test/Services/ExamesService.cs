using Microsoft.EntityFrameworkCore;
using ptm_dev_test.Data;
using ptm_dev_test.Dtos;
using ptm_dev_test.Models;
using ptm_dev_test.Services.IServices;

namespace ptm_dev_test.Services
{
    public class ExamesService : IExamesService
    {
        private readonly AppDbContext _dbContext;

        public ExamesService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ExamesModel> CreateExameAsync(ExamesDto exameDto)
        {
            try
            {
                var exame = new ExamesModel
                {
                    Id = new Random().NextInt64(),
                    Nome = exameDto.Nome,
                    Genero = exameDto.Genero,
                    Idade = exameDto.Idade,
                };

                _dbContext.Exames.Add(exame);
                await _dbContext.SaveChangesAsync();

                return exame;
            }
            catch (DbUpdateException dbEx)
            {
                throw new ApplicationException("Erro ao tentar salvar o exame no banco de dados.", dbEx);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro inesperado ao criar o exame.", ex);
            }
        }

        public async Task<ExamesModel> GetExameByIdAsync(long id)
        {
            var exame = await _dbContext.Exames.FirstOrDefaultAsync(e => e.Id == id);
            if (exame == null)
                throw new KeyNotFoundException("Exame não encontrado.");

            return exame;
        }

        public async Task<object> GetExamesAsync(string? nome, int? idade, string? genero, int pageNumber, int pageSize)
        {
            try
            {
                var query = _dbContext.Exames.AsQueryable();

                if (!string.IsNullOrEmpty(nome))
                    query = query.Where(e => e.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));

                if (idade.HasValue)
                    query = query.Where(e => e.Idade == idade.Value);

                if (!string.IsNullOrEmpty(genero))
                    query = query.Where(e => e.Genero.Equals(genero, StringComparison.OrdinalIgnoreCase));

                int totalItems = await query.CountAsync();
                int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var items = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new
                {
                    currentPage = pageNumber,
                    pageSize,
                    totalPages,
                    totalItems,
                    items
                };
            }
            catch (DbUpdateException dbEx)
            {
                throw new ApplicationException("Erro ao buscar exames no banco de dados.", dbEx);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro inesperado ao tentar buscar os exames.", ex);
            }
        }
    }
}
