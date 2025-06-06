
using StackExchange.Redis;
using api.Models;
using SurvivalApi.Models;

namespace api.Services;

public class RoomService
{
	private readonly IConnectionMultiplexer _redis;

	public RoomService(IConnectionMultiplexer redis)
	{
		_redis = redis;
	}

	public async Task<Room?> GetRoom(string groupName)
	{
		var db = _redis.GetDatabase();
		var roomExists = await db.KeyExistsAsync($"room:{groupName}:players");

		if (!roomExists)
		{
			return null;
		}

		var players = await db.SetMembersAsync($"room:{groupName}:players");
		var playerList = players.Select(p => p.ToString()).ToList();


		var room = new Room(groupName);
		foreach (var playerId in playerList)
		{
			var playerData = await db.HashGetAllAsync($"player:{playerId}");
			var player = MapPlayerDataToPlayer(playerData);

			room.Players.Add(player);
		}

		return room;
	}

	public async Task<string> GetRoomAsJson(string groupName)
	{
		var room = await GetRoom(groupName);
		if (room == null)
		{
			return "";
		}

		return System.Text.Json.JsonSerializer.Serialize(room);
	}

	public async Task CreateRoom(string connectionId, string groupName, string username)
	{
		var db = _redis.GetDatabase();

		var room = await GetRoom(groupName);
		var playersExist = room?.Players.Count > 0;
		var gameExists = room != null;

		if (room != null && playersExist)
		{
			return;
		}

		var host = !gameExists && !playersExist;

		// create player and add to room
		await AddPlayer(connectionId, groupName, username, host);
		await db.StringSetAsync($"room:{groupName}:timer", -1); // add gamer timer
	}

	public async Task JoinRoom(string connectionId, string groupName, string username)
	{
		var db = _redis.GetDatabase();

		var room = await GetRoom(groupName);
		var playersExist = room?.Players.Count > 0;
		var gameExists = room != null;

		var host = !gameExists && !playersExist;

		// create player and add to room
		await AddPlayer(connectionId, groupName, username, host);
	}

	public async Task<Player?> GetPlayer(string connectionId)
	{
		var db = _redis.GetDatabase();
		var playerData = await db.HashGetAllAsync($"player:{connectionId}");
		if (playerData.Length == 0)
		{
			return null;
		}

		var player = MapPlayerDataToPlayer(playerData);

		return player;
	}

	public async Task AddPlayer(string connectionId, string groupName, string username, bool isHost = false)
	{
		var db = _redis.GetDatabase();

		// create player
		await db.HashSetAsync($"player:{connectionId}",
		[
			new HashEntry("id", connectionId),
			new HashEntry("username", username),
			new HashEntry("roomId", groupName),
			new HashEntry("health", 5),
			new HashEntry("host", isHost),
		]);
		
		// add player to room players set
		await db.SetAddAsync($"room:{groupName}:players", connectionId);
	}

	public async Task RemovePlayer(string connectionId, string? groupName = null)
	{
		var db = _redis.GetDatabase();

		if (groupName == null)
		{
			var player = await GetPlayer(connectionId);
			if (player == null || player.RoomId == null)
			{
				return;
			}

			groupName = player.RoomId;
		}

		await db.SetRemoveAsync($"room:{groupName}:players", connectionId);
		await db.KeyDeleteAsync($"player:{connectionId}");
	}

	public async Task SetRoomTimer(string groupName, int seconds)
	{
		var db = _redis.GetDatabase();
		var expirationTime = DateTime.UtcNow.AddSeconds(seconds);
		await db.StringSetAsync($"room:{groupName}:timer", expirationTime.ToString());
	}

	public async Task<DateTime> GetRoomTimer(string groupName)
	{
		var db = _redis.GetDatabase();
		var timer = await db.StringGetAsync($"room:{groupName}:timer");

		if (DateTime.TryParse(timer, out var expirationTime))
		{
			return expirationTime;
		}
		else
		{
			throw new Exception("Invalid timer format");
		}
	}

	public Player MapPlayerDataToPlayer(HashEntry[] data)
	{
		var playerDict = data.ToDictionary(
			entry => entry.Name.ToString(),
			entry => entry.Value.ToString()
		);

		var player = new Player(
			playerDict["id"],
			playerDict["username"],
			playerDict["roomId"],
			int.Parse(playerDict["health"]),
			int.Parse(playerDict["host"]) != 0
		);

		return player;
	}
}