using CommandsService.Models;

namespace CommandsService.SyncDataServices.Grpc
{
    public interface IPlatformDataClient
    {
        public Task<IEnumerable<Platform>?> ReturnAllPlatformsAsync();
    }
}
