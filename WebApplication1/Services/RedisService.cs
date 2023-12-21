using StackExchange.Redis;

namespace WebApplication1.Services;

public class RedisService
{
    private readonly IDatabase database;

    public RedisService()
    {
        var redis = ConnectionMultiplexer.Connect("localhost");
        database = redis.GetDatabase();
    }

    public async Task<string>? GetData(string userId)
    {
        var result = await database.StringGetAsync(userId);

        if (result.HasValue)
            return result.ToString();

        return null;
    }

    public async Task SetData(string userId, string num)
    {
        await database.StringSetAsync(userId, num);
    }



}
