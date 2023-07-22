namespace CommandsService.EventProcessing
{
    public interface IEventProcessor
    {
        public Task ProcessEventAsync(string message);
    }
}
