using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WordCounter.Services;

namespace WordCounter.Controllers
{
    [Route("api/FileApi")]
    public class ApiController : Controller
    {
        IFileProcessingService _fileProcessingService;

        public ApiController(IFileProcessingService fileProcessingService)
        {
            _fileProcessingService = fileProcessingService;
        }
        
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Post([FromRoute] Guid id, [FromForm] IFormFile body)
        {
            // the stop watch is for debug purposes only this is why is not in a separate component 
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            var result = await _fileProcessingService.ProccessFile(body);

            // the stop watch is for debug purposes only this is why is not in a separate component 
            stopwatch.Stop();
            
            // this StringBuilder is for debug purposes only this is why is not in a separate component 
            result = new StringBuilder(result)
                .Append(" Count on the server was completed after: ")
                .Append(stopwatch.Elapsed.TotalSeconds)
                .ToString();
            return Ok(result);
        }


    }
}