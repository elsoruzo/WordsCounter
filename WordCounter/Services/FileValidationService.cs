using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WordCounter.Models;


namespace WordCounter.Services
{
    public interface IFileValidationService
    {
        public Task<bool> IsFileSupported(IFormFile body);
        public Task<bool> IsFileNullOrEmpty(IFormFile body);
        public string GetFileExtType(IFormFile body);
    }

    public class FileValidationService : IFileValidationService
    {
        private readonly SemaphoreSlim _mutex = new SemaphoreSlim(1);

        public string GetFileExtType(IFormFile body)
        {
            var delim = body.FileName.LastIndexOf(".",StringComparison.InvariantCultureIgnoreCase);
            var ext = body.FileName.Substring(delim >= 0 ? delim : 0).ToLower();
            return ext;
        }
        public async Task<bool> IsFileSupported(IFormFile body)
        {
            return await Verify(body, (bodyData) =>
            {   
                var ext = GetFileExtType(bodyData);
                return ext == Constants.FileExtTxt || ext == Constants.FileExtCsv;
            });
        }

        public async Task<bool> IsFileNullOrEmpty(IFormFile body)
        {
            return await Verify(body, (bodyData) => bodyData != null && bodyData.Length != 0);
        }
        
        private async Task<bool> Verify(IFormFile body, Func<IFormFile, bool> verify)
        {
            bool value;
            
            await _mutex.WaitAsync().ConfigureAwait(false);

            try

            {
                value = verify(body);
            }

            finally

            {
                _mutex.Release();
            }
            
            return  value;
        }

    }
}