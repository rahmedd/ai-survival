using Microsoft.AspNetCore.SignalR;

namespace SurvivalApi.Hubs;

public class GameHub : Hub
{
	public async Task NewMessage(long username, string message) =>
		await Clients.All.SendAsync("messageReceived", username, message);
}