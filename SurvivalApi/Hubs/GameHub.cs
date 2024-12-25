using Microsoft.AspNetCore.SignalR;
using api.Models;
using api.Services;
using Bogus;
using SurvivalApi.Models;
using Quartz;
using SurvivalApi.Jobs;

namespace api.Hubs;

public class GameHub : Hub
{
	private readonly RoomService _roomService;
	private readonly ISchedulerFactory _schedulerFactory;

	public GameHub(RoomService roomService, ISchedulerFactory schedulerFactory)
	{
		_roomService = roomService;
		_schedulerFactory = schedulerFactory;
	}
	
	public async Task NewMessage(long username, string message) =>
		await Clients.All.SendAsync("messageReceived", username, message);

	public Task SendMessageToGroup(string groupName, string message)
	{
		return Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId}: {message}");
	}

	public async Task CreateRoom(string groupName, string username)
	{
		// if (groupName == "" || groupName.Length < 6)
		// {
		// 	var faker = new Faker();
		// 	groupName = faker.Random.Words(4);
		// }

		// // if group exists, add player to group
		// if (Context.Items.Any(x => (string)x.Key == groupName))
		// {
		// 	List<Player>? room = Context.Items.First(i => (string)i.Key == groupName).Value as List<Player>;
		// 	if (room != null)
		// 	{
		// 		room.Add(new Player(Context.ConnectionId, username));
		// 	}
		// }
		// // if group does not exist, create group and add player to group
		// else
		// {
		// 	Context.Items[groupName] = new List<Player>(
		// 		[new Player(Context.ConnectionId, username)]
		// 	);
		// }

		await _roomService.CreateOrJoinRoom(Context.ConnectionId, groupName, username);
		await Groups.AddToGroupAsync(Context.ConnectionId, groupName); // automatically adds or creates event group
		await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");

		Room room = await _roomService.GetRoom(groupName);
		var roomJson = System.Text.Json.JsonSerializer.Serialize(room);
		await Clients.Group(groupName).SendAsync("JSON-room", roomJson);
	}

	public async Task RemoveFromGroup(string groupName)
	{
		await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

		await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
	}

	public Task SendPrivateMessage(string user, string message)
	{
		return Clients.User(user).SendAsync("ReceiveMessage", message);
	}

	public Task GetRoom(string groupName)
	{
		List<Player>? room = Context.Items[groupName] as List<Player>;
		if (room != null)
		{
			return Clients.Caller.SendAsync("Send", room);
		}
		else
		{
			return Clients.Caller.SendAsync("Send", "Room not found.");
		}
	}

	public override async Task OnDisconnectedAsync(Exception? ex)
	{
		var player = await _roomService.GetPlayer(Context.ConnectionId);
		await _roomService.RemoveFromRoom(Context.ConnectionId);

		if (player == null ||  player.RoomId == null)
		{
			return;
		}
		
		Room room = await _roomService.GetRoom(player.RoomId);
		var roomJson = System.Text.Json.JsonSerializer.Serialize(room);
		await Clients.Group(player.RoomId).SendAsync("JSON-room", roomJson);

		// return base.OnDisconnectedAsync(ex);
		return;
	}

	public async Task StartGameLoop(int gameLength)
	{
		var player = await _roomService.GetPlayer(Context.ConnectionId);
		if (player == null || !player.Host || player.RoomId == null || player.RoomId == "")
		{
			return;
		}

		await _roomService.SetRoomTimer(player.RoomId, gameLength);

		var ret = new
		{
			gameLength,
			expirationTime = await _roomService.GetRoomTimer(player.RoomId)
		};
		await Clients.Group(player.RoomId).SendAsync("JSON-timer-start", ret);

		var _scheduler = await _schedulerFactory.GetScheduler();
		await _scheduler.Start();

		var job = JobBuilder.Create<EndGameJob>()
			.WithIdentity($"endGame-{player.RoomId}", "endGameGroup")
			.UsingJobData("roomId", player.RoomId)
			.Build();

		var trigger = TriggerBuilder.Create()
			.WithIdentity($"endGameTrigger-{player.RoomId}", "endGameGroup")
			.StartAt(DateBuilder.FutureDate(gameLength, IntervalUnit.Second))
			.Build();

		await _scheduler.ScheduleJob(job, trigger);
	}

	public async Task StopGameLoop()
	{
		var player = await _roomService.GetPlayer(Context.ConnectionId);
		if (player == null || !player.Host || player.RoomId == null || player.RoomId == "")
		{
			return;
		}

		await _roomService.SetRoomTimer(player.RoomId, -9999);
		Console.WriteLine("Stopping game loop.");
	}
}