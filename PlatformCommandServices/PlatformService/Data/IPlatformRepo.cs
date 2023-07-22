using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatformRepo
    {
        Task<bool> SaveChangesAsync();

        Task<IEnumerable<Platform>> GetAllPlatformsAsync();

        Task<Platform?> GetPlatformByIdAsync(int id);

        void CreatePlatform(Platform platform);
    }
}
