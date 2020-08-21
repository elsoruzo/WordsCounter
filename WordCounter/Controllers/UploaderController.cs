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
        private static readonly char[] separators = {' '};
        private static List<string> lines;
        private static ConcurrentDictionary<string, int> freqeuncyDictionary;

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Post([FromRoute] Guid id, [FromForm] IFormFile body)
        {   
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            lines = new List<string>();


            using (var streamReader = new StreamReader(body.OpenReadStream()))
            {
                string line;
                while ((line = await streamReader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
            }

            ConcurrentDictionary<string, int> test = GetFrequencyFromLines(lines);

            stopwatch.Stop();
            var result = new StringBuilder("The amount of words in your file is: ")
                .Append(test.First().Value.ToString())
                .Append(" Count on the server was completed after: ")
                .Append(stopwatch.Elapsed.TotalSeconds)
                .ToString();
            return Ok(result);
        }

        public static ConcurrentDictionary<string, int> GetFrequencyFromLines(List<string> lines)
        {
            freqeuncyDictionary = new ConcurrentDictionary<string, int>();
            Parallel.ForEach(lines, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },line =>
            {
                var words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    freqeuncyDictionary.AddOrUpdate("amount", 1, (key, oldValue) => oldValue + 1);
                }
            });

            return freqeuncyDictionary;
        }
    }
}