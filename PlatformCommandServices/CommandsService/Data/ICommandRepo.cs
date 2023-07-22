using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {
        Task<bool> SaveChangesAsync();

        // Platforms
        Task<IEnumerable<Platform>> GetAllPlatformsAsync();
        void CreatePlatform(Platform plat);
        Task<bool> PlaformExitsAsync(int platformId);
        Task<bool> ExternalPlatformExistsAsync(int externalPlatformId);

        // Commands
        Task<IEnumerable<Command>> GetCommandsForPlatformAsync(int platformId);
        Task<Command> GetCommandAsync(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);
    }
}
