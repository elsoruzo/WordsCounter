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
        private static readonly char[] separators = {' ', '.', ',','?','!'};
        private static List<string> lines;
        private static ConcurrentDictionary<string, int> countDictionary;
        public async Task<string> GetWordsCount(IFormFile body)
        {
            lines = new List<string>();


            using (var streamReader = new StreamReader(body.OpenReadStream()))
            {
                string line;
                while ((line = await streamReader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
            }

            ConcurrentDictionary<string, int> result = GetWordsCountFromLines(lines);
            
            return result.Count == 0 ? Constants.ZeroWordsCount : result.First().Value.ToString();
        }
        
        private static ConcurrentDictionary<string, int> GetWordsCountFromLines(List<string> lines)
        {
            countDictionary = new ConcurrentDictionary<string, int>();
            Parallel.ForEach(lines, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },line =>
            {
                var words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    countDictionary.AddOrUpdate("amount", 1, (key, oldValue) => oldValue + 1);
                }
            });

            return countDictionary;
        }
    }
}