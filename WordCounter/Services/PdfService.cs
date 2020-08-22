using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WordCounter.Services
{
    public interface IPdfService
    {
        public Task<string> GetWordsCount(IFormFile body);
    }

    public class PdfService : IPdfService
    {
        public async Task<string> GetWordsCount(IFormFile body)
        {
            throw new System.NotImplementedException();
        }
    }
}