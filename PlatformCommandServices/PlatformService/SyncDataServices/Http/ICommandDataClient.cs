using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public interface ICommandDataClient
    {
        public Task SendPlatformToCommand(PlatformReadDto platform);
    }
}
