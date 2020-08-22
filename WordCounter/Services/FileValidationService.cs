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
    }

    public class FileValidationService : IFileValidationService
    {
        SemaphoreSlim mutex = new SemaphoreSlim(1);


        public async Task<bool> IsFileSupported(IFormFile body)
        {
            return await Verify(body, (body) =>
            {   
                int delim = body.FileName.LastIndexOf(".",StringComparison.InvariantCultureIgnoreCase);
                string ext = body.FileName.Substring(delim >= 0 ? delim : 0).ToLower();
                return ext == Constants.FileExtTxt || ext == Constants.FileExtCsv;
            });
        }

        public async Task<bool> IsFileNullOrEmpty(IFormFile body)
        {
            return await Verify(body, (body) => body != null && body.Length != 0);
        }
        
        private async Task<bool> Verify(IFormFile body, Func<IFormFile, bool> verify)
        {
            bool value;
            
            await mutex.WaitAsync().ConfigureAwait(false);

            try

            {
                value = verify(body);
            }

            finally

            {
                mutex.Release();
            }
            
            return  value;
        }

    }
}