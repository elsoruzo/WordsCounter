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
        private static List<string> _lines;
        private static ConcurrentDictionary<string, int> _countDictionary;
        public async Task<string> GetWordsCount(IFormFile body)
        {
            _lines = new List<string>();


            using (var streamReader = new StreamReader(body.OpenReadStream()))
            {
                string line;
                while ((line = await streamReader.ReadLineAsync()) != null)
                {
                    _lines.Add(line);
                }
            }

            ConcurrentDictionary<string, int> result = GetWordsCountFromLines(_lines);
            
            return result.Count == 0 ? Constants.ZeroWordsCount : result.First().Value.ToString();
        }
        
        private static ConcurrentDictionary<string, int> GetWordsCountFromLines(IEnumerable<string> lines)
        {
            _countDictionary = new ConcurrentDictionary<string, int>();
            Parallel.ForEach(lines, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },line =>
            {
                var words = line.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    _countDictionary.AddOrUpdate("amount", 1, (key, oldValue) => oldValue + 1);
                }
            });

            return _countDictionary;
        }
    }
}