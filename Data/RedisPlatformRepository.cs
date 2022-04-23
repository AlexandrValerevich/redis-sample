using System.Text.Json;
using StackExchange.Redis;

namespace ReadiAPI.Data;

public class RedisPlatformRepository : IPlatformRepository
{
    private readonly IConnectionMultiplexer _redis;
    private IDatabase DataBase => _redis.GetDatabase();

    public RedisPlatformRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task CreatePlatformAsync(Platform platform)
    {
        if (platform is null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        string serializedPlatform = Serialize(platform);

        await DataBase.HashSetAsync("hashplatform",
            new HashEntry[] { new HashEntry(platform.Id, serializedPlatform) });

        // await DataBase.StringSetAsync(platform.Id, serialplatform);
        // await DataBase.SetAddAsync("PlatformSet", serialplatform);
    }

    public async Task<IEnumerable<Platform>> ReadAllAsync()
    {
        HashEntry[] completeSet = await DataBase.HashGetAllAsync("hashplatform");

        if (completeSet.Length > 0)
        {
            List<Platform> platforms = Array.ConvertAll(completeSet, val => Deserialize(val.Value)).ToList();
            return platforms;
        }

        return new List<Platform>();
    }

    public async Task<Platform> ReadByIdAsync(string id)
    {
        // RedisValue platform = await DataBase.StringGetAsync(id);
        RedisValue platform = await DataBase.HashGetAsync("hashplatform", id);

        if (!string.IsNullOrEmpty(platform))
        {
            return Deserialize(platform);
        }

        return null;
    }

    private static string Serialize(Platform platform)
    {
        return JsonSerializer.Serialize(platform);
    }

    private static Platform Deserialize(string platform)
    {
        return JsonSerializer.Deserialize<Platform>(platform);
    }
}
