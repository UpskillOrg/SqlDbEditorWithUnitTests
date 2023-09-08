using Caliburn.Micro;
using SqlDbEditor.Messages;
using System.Threading.Tasks;

namespace SqlDbEditor.Services
{
    public class EventAggregatorService : IEventAggregatorService
    {
        private readonly IEventAggregator _eventAggregator;

        public EventAggregatorService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Publish the error message through the event aggregator
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public virtual Task PublishOnUIThreadAsync(ErrorMessage errorMessage)
        {
            return _eventAggregator.PublishOnUIThreadAsync(errorMessage);
        }
    }
}
