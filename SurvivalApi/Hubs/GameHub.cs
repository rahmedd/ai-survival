using Microsoft.AspNetCore.SignalR;
using api.Models;
using api.Services;
using Bogus;
using SurvivalApi.Models;

namespace api.Hubs;

public class GameHub : Hub
{
	private readonly RoomService _roomService;

	public GameHub(RoomService roomService)
	{
		_roomService = roomService;
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

	public override Task OnDisconnectedAsync(Exception? ex)
	{
		foreach (var item in Context.Items)
		{
			List<Player>? room = item.Value as List<Player>;
			if (room != null)
			{
				Player? player = room.First(p => p.Id == Context.ConnectionId);
				if (player != null)
				{
					room.Remove(player);
					Clients.Group((string)item.Key).SendAsync("Send", $"{Context.ConnectionId} has left the group {item.Key}.");
				}
			}
		}

		return base.OnDisconnectedAsync(ex);
	}
}