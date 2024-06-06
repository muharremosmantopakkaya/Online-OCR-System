using EkonLayer.Core.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EkonLayer.Core.Services
{
    public interface IStockService
    {
        Task<IEnumerable<Stock>> GetAllStocksAsync();
        Task<Stock> GetStockBySymbolAsync(string symbol); // Arayüze bu metodu ekleyin
        Task<Stock> GetStockByIdAsync(int id);
    }
}
