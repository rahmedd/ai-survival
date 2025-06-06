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
		await _roomService.CreateRoom(Context.ConnectionId, groupName, username);
		await Groups.AddToGroupAsync(Context.ConnectionId, groupName); // automatically adds or creates event group
		await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");

		var roomJson = await _roomService.GetRoomAsJson(groupName);
		await Clients.Group(groupName).SendAsync("JSON-room", roomJson);
	}

	public async Task JoinRoom(string groupName, string username)
	{
		await _roomService.JoinRoom(Context.ConnectionId, groupName, username);
		await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
		await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");

		var roomJson = await _roomService.GetRoomAsJson(groupName);
		await Clients.Group(groupName).SendAsync("JSON-room", roomJson);
	}

	public async Task RemoveFromGroup(string groupName)
	{
		await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
		await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");

		var roomJson = await _roomService.GetRoomAsJson(groupName);
		await Clients.Group(groupName).SendAsync("JSON-room", roomJson);
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
		await _roomService.RemovePlayer(Context.ConnectionId);

		if (player == null ||  player.RoomId == null)
		{
			return;
		}

		var roomJson = await _roomService.GetRoomAsJson(player.RoomId);
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

		// var trigger = TriggerBuilder.Create()
		// 	.WithIdentity($"endGameTrigger-{player.RoomId}", "endGameGroup")
		// 	.StartAt(DateBuilder.FutureDate(gameLength, IntervalUnit.Second))
		// 	.Build();

		ITrigger trigger = TriggerBuilder.Create()
			.WithIdentity($"TimerTrigger-{player.RoomId}", "TimerGroup")
			.StartNow()
			.WithSimpleSchedule(x => x
				.WithIntervalInSeconds(1)
				.RepeatForever()
			)
			.EndAt(DateBuilder.FutureDate(gameLength, IntervalUnit.Second))
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