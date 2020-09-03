using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WordCounter.Models;

namespace WordCounter.Services
{
    public interface IFileSelectorService
    {
        public Task<int> GetResult(IFormFile body);
    }

    public class FileSelectorService : IFileSelectorService
    {
        private readonly IPdfService _pdfService;
        private readonly ITxtCsvService _txtCsvService;
        private readonly IFileValidationService _fileValidationService;
        public FileSelectorService(IPdfService pdfService, ITxtCsvService txtCsvService, IFileValidationService fileValidationService)
        {
            _pdfService = pdfService;
            _txtCsvService = txtCsvService;
            _fileValidationService = fileValidationService;
        }
        public async Task<int> GetResult(IFormFile body)
        {
            return await SelectService(body);

        }
        
        private async Task<int> SelectService(IFormFile body)
        {
            var ext = _fileValidationService.GetFileExtType(body);
            if (ext == Constants.FileExtTxt||ext == Constants.FileExtCsv)
            {
                return await _txtCsvService.GetWordsCount(body);
            }
            
            throw new NotImplementedException("FileSelectorService, the file extension is unsupported");
        }
    }
}