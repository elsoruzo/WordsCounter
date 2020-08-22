using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WordCounter.Models;

namespace WordCounter.Services
{
    public interface IMessageService
    {
        public Task<string> GetMessage(FileState fileState, string wordCount = null);
    }
    public class MessageService : IMessageService
    {
        SemaphoreSlim mutex = new SemaphoreSlim(1);
        public async Task<string> GetMessage(FileState fileState, string wordCount = null)
        {
            string result = null;

            await mutex.WaitAsync().ConfigureAwait(false);

            try

            {
                var basicMessage = new StringBuilder(Constants.Result);
                if (fileState == FileState.Unsupported)
                {
                    result = basicMessage.Append(Constants.FileTypeIsUnsupported)
                        .ToString();
                }
                
                if (fileState == FileState.NullOrEmpty)
                {
                    result = basicMessage.Append(Constants.FileIsNullOrempty)
                        .ToString();
                }
                
                if (fileState == FileState.Supported)
                {
                    result = basicMessage.Append(Constants.UserMessageOfCountResults)
                        .Append(wordCount)
                        .ToString();
                }

            }

            finally

            {

                mutex.Release();

            }


            return  result;
            
        }
    }
}