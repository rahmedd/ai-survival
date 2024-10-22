
using StackExchange.Redis;
using Bogus;

namespace api.Services;

public class RoomService
{
	private readonly IConnectionMultiplexer _redis;

	public RoomService(IConnectionMultiplexer redis)
	{
		_redis = redis;
	}

	public async Task CreateRoom()
	{
		var db = _redis.GetDatabase();
		var faker = new Faker();
		string name = faker.Random.Words(3);
		// await db.HashSetAsync("room:");
	}

	
}