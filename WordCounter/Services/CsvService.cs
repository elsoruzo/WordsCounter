using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WordCounter.Services
{
    public interface ICsvService
    {
        public Task<string> GetWordsCount(IFormFile body);
    }

    public class CsvService : ICsvService
    {
        public async Task<string> GetWordsCount(IFormFile body)
        {
            throw new System.NotImplementedException();
        }
    }
}