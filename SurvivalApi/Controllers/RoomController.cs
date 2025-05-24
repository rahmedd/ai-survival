using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.SignalR;
using api.Hubs;
using Bogus;
using api.Services;
using api.Models.DTOs;

// using api.DTOs;
// using api.Services;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class RoomController : ControllerBase
{
	private readonly IHubContext<GameHub> _hubContext;
	private readonly RoomService _roomService;

	public RoomController(IHubContext<GameHub> hubContext, RoomService roomService)
	{
		_hubContext = hubContext;
		_roomService = roomService;
	}

	[HttpGet("InitUser")]
	public async Task<Results<Ok, ValidationProblem>> InitUser()
	{
		// _hubContext
		
		return TypedResults.Ok();
	}

	// [HttpGet("CheckRoomAvailability")]
	// public async Task<Results<Ok<BaseResponseEmpty>, ValidationProblem>> CheckRoomAvailability(string roomId)
	// {
	// 	bool isAvailable = await _roomService.GetRoom(roomId) is null;
	// 	var ret = new BaseResponseEmpty(isAvailable, isAvailable ? "Room is available" : "Room is not available");
	// 	return TypedResults.Ok(ret);
	// }

	[HttpPost("CreateGame")]
	public async Task<Results<Ok<BaseResponseEmpty>, ValidationProblem>> CreateGame(
		[FromBody] CreateGameRequest req
	)
	{
		var room = await _roomService.GetRoom(req.GroupName);
		if (room != null)
		{
			return TypedResults.Ok(new BaseResponseEmpty(false, "Room already exists"));
		}

		await _roomService.CreateRoom(req.ConnectionId, req.GroupName, req.Username);
		await _hubContext.Groups.AddToGroupAsync(req.ConnectionId, req.GroupName); // automatically adds or creates event group
		await _hubContext.Clients.Group(req.GroupName).SendAsync("Send", $"{req.ConnectionId} has joined the group {req.GroupName}.");

		var roomJson = await _roomService.GetRoomAsJson(req.GroupName);
		await _hubContext.Clients.Group(req.GroupName).SendAsync("JSON-room", roomJson);
		
		return TypedResults.Ok(new BaseResponseEmpty(true, "Game created"));
	}

	// [HttpPost("Register")]
	// public async Task<Results<Ok, ValidationProblem>> Register([FromBody] RegisterRequestDto req)
	// {
	// 	var res = await _authService.RegisterAsync(req.Email, req.Password);
	// 	if (!res.Succeeded)
	// 	{
	// 		return _authService.CreateValidationProblem(res);
	// 	}

	// 	await _authService.LoginAsync(req.Email, req.Password);
		
	// 	return TypedResults.Ok();
	// }

	// [HttpPost("Login")]
	// public async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> Login([FromBody] RegisterRequestDto req)
	// {
	// 	var res = await _authService.LoginAsync(req.Email, req.Password);

	// 	if(res.Succeeded)
	// 	{
	// 		return TypedResults.Empty;
	// 	}
	// 	else
	// 	{
	// 		return TypedResults.Problem(res.ToString(), statusCode: StatusCodes.Status401Unauthorized);
		
	// 	}
	// }

	// [HttpPost("Logout")]
	// public async Task<Results<Ok, EmptyHttpResult, ProblemHttpResult>> Logout()
	// {
	// 	await _authService.LogoutAsync();
	// 	return TypedResults.Ok();
	// }

	// [HttpGet("User")]
	// public async Task<Results<Ok<BaseResponse<GetUserRequestDto>>, EmptyHttpResult, ProblemHttpResult>> GetUser()
	// {
	// 	var iuser = this.User;

	// 	if (iuser == null || iuser.Identity == null || iuser.Identity.Name == null || !iuser.Identity.IsAuthenticated)
	// 	{
	// 		return TypedResults.Problem("User not found", statusCode: StatusCodes.Status404NotFound);
	// 	}


	// 	var user = await _authService.GetUserAsync(iuser.Identity.Name);

	// 	if (user == null)
	// 	{
	// 		return TypedResults.Problem("User not found", statusCode: StatusCodes.Status404NotFound);
	// 	}

	// 	var roles = await _authService.GetRolesAsync(user);

	// 	var ret = new GetUserRequestDto {
	// 		Email = iuser.Identity.Name,
	// 		Roles = roles,
	// 	};

	// 	return TypedResults.Ok(new BaseResponse<GetUserRequestDto>(true, "", ret));
	// }

	// [HttpGet("CheckUsername")]
	// public async Task<Results<Ok<BaseResponseEmpty>, EmptyHttpResult, ProblemHttpResult>> CheckUsername(string email)
	// {
	// 	var user = await _authService.GetUserAsync(email);

	// 	if (user == null)
	// 	{
	// 		return TypedResults.Ok(new BaseResponseEmpty(false, "User not found"));
	// 	}

	// 	return TypedResults.Ok(new BaseResponseEmpty(true, "User found"));
	// }
}