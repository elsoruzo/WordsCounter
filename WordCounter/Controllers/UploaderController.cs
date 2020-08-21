using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    
            var temp = new ConcurrentDictionary<string, uint>(StringComparer.InvariantCultureIgnoreCase);
            using (var reader = new StreamReader(body.OpenReadStream()))
            {
                List<string> lines = new List<string>();
                while (reader.Peek() >= 0)
                {
                    lines.Add(await reader.ReadLineAsync()); 
                }
                //text = await reader.ReadToEndAsync();
                
                Parallel.ForEach(
                    lines,
                    new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                    (line, state, index) =>
                    {
                        foreach (var word in line.Split(" "))
                        {
                            temp.AddOrUpdate("amounth", 1, (key, oldVal) => oldVal + 1);
                        }
                    }
                );
            }

            var result = new StringBuilder("The amount of words in your file is: ")
                .Append(temp.First().Value.ToString())
                .ToString();
            return Ok(result);
        }
    }
}