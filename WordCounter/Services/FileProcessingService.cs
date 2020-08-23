using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WordCounter.Models;

namespace WordCounter.Services
{
    public interface IFileProcessingService
    {
        public Task<string> ProcessFile(IFormFile body);

    }

    public class FileProcessingService : IFileProcessingService
    {
        public async Task<string> ProcessFile(IFormFile body)
        {
            return await GetResult(body);
        }
        
        private readonly IFileValidationService _fileValidationService;
        private readonly IMessageService _messageService;
        private readonly IFileSelectorService _fileSelectorService;

        public FileProcessingService(IFileValidationService fileValidationService, IMessageService messageService, IFileSelectorService fileSelectorService)
        {
            _fileValidationService = fileValidationService;
            _messageService = messageService;
            _fileSelectorService = fileSelectorService;
        }
        private async Task<string> GetResult(IFormFile body)
        {
            var isFileSupported = await _fileValidationService.IsFileSupported(body);
            if (!isFileSupported)
            {
                return await _messageService.GetMessage(FileState.Unsupported);
            }
            
            var isFileNullOrEmpty = await _fileValidationService.IsFileNullOrEmpty(body);
            if (!isFileNullOrEmpty)
            {
                return await _messageService.GetMessage(FileState.NullOrEmpty);
            }

            var wordCount = await _fileSelectorService.GetResult(body);
            return await _messageService.GetMessage(FileState.Supported, wordCount);
        }
    }
}