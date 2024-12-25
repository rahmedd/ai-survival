using Microsoft.AspNetCore.SignalR;
using Quartz;
using api.Hubs;
using api.Services;

namespace SurvivalApi.Jobs;

public class EndGameJob : IJob
{
	private readonly IHubContext<GameHub> _hubContext;
	private readonly RoomService _roomService;

	public EndGameJob(IHubContext<GameHub> hubContext, RoomService roomService)
	{
		_hubContext = hubContext;
		_roomService = roomService;
	}

	public async Task Execute(IJobExecutionContext context)
	{
		var roomId = context.JobDetail.JobDataMap.GetString("roomId");
		if (string.IsNullOrEmpty(roomId)) return;
		
		await _hubContext.Clients.Group(roomId).SendAsync("JSON-timer-end", "ENDED");
	}
}