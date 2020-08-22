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
        IPdfService _pdfService;
        ITxtCsvService _txtCsvService;


        public FileSelectorService(IPdfService pdfService, ITxtCsvService txtCsvService)
        {
            _pdfService = pdfService;
            _txtCsvService = txtCsvService;
        }
        public async Task<string> GetResult(IFormFile body)
        {
            return await SelectService(body);

        }
        
        public async Task<string> SelectService(IFormFile body)
        {
            int delim = body.FileName.LastIndexOf(".",StringComparison.InvariantCultureIgnoreCase);
            string ext = body.FileName.Substring(delim >= 0 ? delim : 0).ToLower();
            if (ext == Constants.FileExtTxt||ext == Constants.FileExtCsv)
            {
                return await _txtCsvService.GetWordsCount(body);
            }
            
            throw new NotImplementedException("FileSelectorService, the file extension is unsupported");
        }
    }
}