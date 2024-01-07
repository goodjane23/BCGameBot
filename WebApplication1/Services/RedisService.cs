using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using Telegram.Bot.Types;

namespace WebApplication1.Services;

public class RedisService
{
    private readonly IDatabase database;

    public RedisService(
        IOptions<RedisOptions> redisOptions)
    {
        Console.WriteLine($"{redisOptions.Value.Url} - connection string");
        var redis = ConnectionMultiplexer.Connect(redisOptions.Value.Url);        
        database = redis.GetDatabase();
    }

    public async Task<string?> GetUserQuiz(string userId)
    {
        var result = await database.StringGetAsync(userId);

        if (result.HasValue)
            return result.ToString();

        return null;
    }

    public async Task SetUserQuiz(string userId, string num)
    {
        await database.StringSetAsync(userId, num);
        Console.WriteLine($"{num} was added for user: {userId}");
    }

    public void CleanUserKey(string key)
    {
        database.KeyDelete(key);
        Console.WriteLine($"{key} was deleted");
    }

}
