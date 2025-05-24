
namespace api.Models.DTOs;
public class CreateGameRequest
{
	public string ConnectionId { get; set; } = string.Empty;
	public string GroupName { get; set; } = string.Empty;
	public string Username { get; set; } = string.Empty;
}