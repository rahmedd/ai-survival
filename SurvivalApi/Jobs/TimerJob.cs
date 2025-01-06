using Microsoft.AspNetCore.SignalR;
using Quartz;
using api.Hubs;
using api.Services;

namespace SurvivalApi.Jobs;

public class TimerJob : IJob
{
	private readonly IHubContext<GameHub> _hubContext;
	private readonly RoomService _roomService;

	public TimerJob(IHubContext<GameHub> hubContext, RoomService roomService)
	{
		_hubContext = hubContext;
		_roomService = roomService;
	}

	public async Task Execute(IJobExecutionContext context)
	{
		var roomId = context.JobDetail.JobDataMap.GetString("roomId");
		if (string.IsNullOrEmpty(roomId)) return;
		
		await _hubContext.Clients.Group(roomId).SendAsync("JSON-timer", "ENDED");
	}
}