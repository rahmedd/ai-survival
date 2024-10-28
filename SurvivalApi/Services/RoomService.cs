
using StackExchange.Redis;
using Bogus;
using SurvivalApi.Models;
using api.Models;

namespace api.Services;

public class RoomService
{
	private readonly IConnectionMultiplexer _redis;

	public RoomService(IConnectionMultiplexer redis)
	{
		_redis = redis;
	}

	public async Task CreateOrJoinRoom(string connectionId, string groupName, string username)
	{
		var db = _redis.GetDatabase();

		// var playerExists = await db.KeyExistsAsync($"player:{connectionId}");
		var gameExists = await db.KeyExistsAsync($"room:{groupName}");
		var playersExist = await db.SetLengthAsync($"room:{groupName}:players") > 0;
		var host = !gameExists && !playersExist;
		
		if (groupName == "" || groupName.Length < 6)
		{
			var faker = new Faker();
			groupName = faker.Random.Words(4);
		}

		// rmeove from existing room and delete player
		var existingPlayerRoom = await db.HashGetAsync($"player:{connectionId}", "roomId");
		var existingPlayerRoomString = existingPlayerRoom.ToString();
		// if (playerExists && existingPlayerRoomString != groupName)
		if (existingPlayerRoomString.Length > 0)
		{
			await RemoveFromRoom(connectionId, existingPlayerRoomString);
		}

		var roomKey = $"room:{groupName}";
		await db.SetAddAsync($"{roomKey}:players", connectionId);

		var playerKey = $"player:{connectionId}";
		await db.HashSetAsync(playerKey,
        [
			new HashEntry("id", connectionId),
			new HashEntry("username", username),
			new HashEntry("roomId", roomKey),
			new HashEntry("health", 5),
			new HashEntry("host", host),
		]);
	}

	public async Task RemoveFromRoom(string connectionId, string groupName)
	{
		var db = _redis.GetDatabase();

		var roomKey = $"room:{groupName}";
		await db.SetRemoveAsync($"{roomKey}:players", connectionId);

		var playerKey = $"player:{connectionId}";
		await db.KeyDeleteAsync(playerKey);
	}
}