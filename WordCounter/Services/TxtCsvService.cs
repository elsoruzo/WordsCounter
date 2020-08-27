using System;
using System.Collections.Concurrent;
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
        public Task<string> GetWordsCount(IFormFile body);
    }

    public class TxtCsvCsvService : ITxtCsvService
    {
        private static readonly char[] Separators = {' ', '.', ',','?','!'};
        private static List<string> _bufferLines;
        private static  ConcurrentQueue<string> _accumBufferQueue;

        public async Task<string> GetWordsCount(IFormFile body)
        {
            var result = Constants.WordsCountStartingPoint;

            using (var streamReader = new StreamReader(body.OpenReadStream()))
            {
                _bufferLines = new List<string>(Constants.LinesBufferSize);
                var counter = 0;
                string line;
                while ((line = await streamReader.ReadLineAsync()) != null)
                {
                    counter += 1;  
                    _bufferLines.Add(line);
                    if (counter == Constants.LinesBufferSize)
                    {
                        result += GetWordsCountFromLines(_bufferLines);
                        counter = 0;
                        _bufferLines = new List<string>(Constants.LinesBufferSize);
                    }
                }
            }
            return result.ToString();
        }
        
        private static int GetWordsCountFromLines(IEnumerable<string> lines)
        {
            _accumBufferQueue = new ConcurrentQueue<string>();
            
             return lines.AsParallel().WithDegreeOfParallelism( Environment.ProcessorCount )
                 .Aggregate(_accumBufferQueue,
             (accum, line) =>
             {
                 foreach (var word in line.Split(Separators, StringSplitOptions.RemoveEmptyEntries))
                 {
                     accum.Enqueue(word);
                 }

                 return accum;
             },finalResult => finalResult.Count);
             
        }
    }
}