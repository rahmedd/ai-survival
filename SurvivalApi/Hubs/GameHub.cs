using Microsoft.AspNetCore.SignalR;

namespace api.Hubs;

public class GameHub : Hub
{
	public async Task NewMessage(long username, string message) =>
		await Clients.All.SendAsync("messageReceived", username, message);

	public Task SendMessageToGroup(string groupName, string message)
	{
		return Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId}: {message}");
	}

	public async Task AddToGroup(string groupName)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

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
}