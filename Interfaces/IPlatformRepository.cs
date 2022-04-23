namespace ReadiAPI.Interfaces;

public interface IPlatformRepository
{
    Task CreatePlatformAsync(Platform platform);
    Task<Platform> ReadByIdAsync(string id);
    Task<IEnumerable<Platform>> ReadAllAsync();
}
