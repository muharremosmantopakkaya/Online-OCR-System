using EkonLayer.Core.DbModels;
using EkonLayer.Core.Services;
using EkonLayer.Helpers.Models.Dtos.DbModelDtos;
using EkonLayer.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace EkonLayer.Service.Services
{
    public class StockService : IStockService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public StockService(AppDbContext context, HttpClient httpClient, IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Stock>> GetAllStocksAsync()
        {
            var stocks = await _context.Stocks.ToListAsync();
            return stocks;
        }

        public async Task<Stock> GetStockBySymbolAsync(string symbol)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);

            if (stock == null)
            {
                var apiKey = _configuration["Finnhub:ApiKey"];
                var response = await _httpClient.GetAsync($"https://finnhub.io/api/v1/quote?symbol={symbol}&token={apiKey}");
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var stockQuoteDto = JsonSerializer.Deserialize<StockQuoteDto>(responseBody);

                stock = new Stock
                {
                    Symbol = symbol,
                    Name = symbol, // This could be fetched from another API if needed
                    Price = stockQuoteDto.C,  // Current price
                    Change = stockQuoteDto.D  // Change
                };

                // Add stock to the database
                _context.Stocks.Add(stock);
                await _context.SaveChangesAsync();
            }

            return stock;
        }

        public async Task<Stock> GetStockByIdAsync(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            return stock;
        }
    }
}
