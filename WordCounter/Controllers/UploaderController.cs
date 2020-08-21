using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WordCounter.Controllers
{

    [Route("api/FileApi")]
    public class ApiController : Controller
    {


        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Post([FromRoute]Guid id, [FromForm]IFormFile body)
        {
            byte[] fileBytes;
            using(var memoryStream = new MemoryStream())
            {
                await body.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            var filename = body.FileName;
            var contentType = body.ContentType;
            
            //SaveFileToDatabase(id, fileBytes, filename, contentType);

            return Ok("BlaBla");
        }
    }
}