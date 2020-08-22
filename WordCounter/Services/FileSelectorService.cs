using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WordCounter.Models;

namespace WordCounter.Services
{
    public interface IFileSelectorService
    {
        public Task<string> GetResult(IFormFile body);
    }

    public class FileSelectorService : IFileSelectorService
    {
        ICsvService _csvService;
        ITxtService _txtService;


        public FileSelectorService(ICsvService csvService, ITxtService txtService)
        {
            _csvService = csvService;
            _txtService = txtService;
        }
        public async Task<string> GetResult(IFormFile body)
        {
            return await SelectService(body);

        }
        
        public async Task<string> SelectService(IFormFile body)
        {
            int delim = body.FileName.LastIndexOf(".",StringComparison.InvariantCultureIgnoreCase);
            string ext = body.FileName.Substring(delim >= 0 ? delim : 0).ToLower();
            if (ext == Constants.FileExtTxt)
            {
                return await _txtService.GetWordsCount(body);
            }
            
            if (ext == Constants.FileExtCsv)
            {
                return await _csvService.GetWordsCount(body);
            }

            throw new NotImplementedException("FileSelectorService, the file extension is unsupported");

        }
    }
}