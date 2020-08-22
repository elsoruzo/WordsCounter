using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WordCounter.Services
{
    public interface ICallVerificationService
    {
        public Task<bool> Verify(IFormFile body);
    }

    public class CallVerificationService : ICallVerificationService
    {
        SemaphoreSlim mutex = new SemaphoreSlim(1);
       
        public  async Task<bool> Verify(IFormFile body)
        {
            bool value;

            await mutex.WaitAsync().ConfigureAwait(false);

                try

                {
                    value = body != null && body.Length != 0;

                }

                finally

                {

                    mutex.Release();

                }


            return  value;
        }
    }
}