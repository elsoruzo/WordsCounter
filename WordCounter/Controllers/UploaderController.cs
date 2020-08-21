using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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
            string text;
            using(var memoryStream = new MemoryStream())
            {
                await body.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            var filename = body.FileName;
            var contentType = body.ContentType;
            
            if (body == null || body.Length == 0)
            {
                 await Task.FromResult((string)null);
            }
    
            using (var reader = new StreamReader(body.OpenReadStream()))
            {
                text = await reader.ReadToEndAsync();
            }
            
            int count = text.Split(' ').Length;

            var result = new StringBuilder("The amount of words in your file is: ")
                .Append(count.ToString())
                .ToString();
            return Ok(result);
        }
    }
}