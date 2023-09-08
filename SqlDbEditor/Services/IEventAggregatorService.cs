using SqlDbEditor.Messages;
using System.Threading.Tasks;

namespace SqlDbEditor.Services
{
    public interface IEventAggregatorService
    {
        /// <summary>
        /// Publish the error message through the event aggregator
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task PublishOnUIThreadAsync(ErrorMessage errorMessage);
    }
}