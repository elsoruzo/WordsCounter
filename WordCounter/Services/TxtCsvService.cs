using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WordCounter.Models;

namespace WordCounter.Services
{
    public interface ITxtCsvService
    {
        public Task<int> GetWordsCount(IFormFile body);
    }

    public class TxtCsvCsvService : ITxtCsvService
    {

        public async Task<int> GetWordsCount(IFormFile body)
        {
            var result = Constants.WordsCountStartingPoint;

            using (var streamReader = new StreamReader(body.OpenReadStream()))
            {
                var bufferLines = new List<string>(Constants.LinesBufferSize);
                var counter = 0;
                string line;
                while ((line = await streamReader.ReadLineAsync()) != null)
                {
                    counter += 1;  
                    bufferLines.Add(line);
                    if (counter == Constants.LinesBufferSize)
                    {
                        result += GetWordsCountFromLines(bufferLines);
                        counter = 0;
                        bufferLines.Clear();
                    }
                }

                if (bufferLines.Count > 0)
                {
                    result += GetWordsCountFromLines(bufferLines);
                }
               
            }

            return result;
        }
        
        private static int GetWordsCountFromLines(IEnumerable<string> lines)
        {
            var separators = new [] {' ', '.', ',','?','!'};
            return lines. 
                AsParallel() 
                .WithDegreeOfParallelism(Environment.ProcessorCount)
                .Select(line => line.Split(separators, StringSplitOptions.RemoveEmptyEntries).Length).Sum();
 
        }
    }
}