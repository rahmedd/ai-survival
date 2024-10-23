
namespace api.Models;

public class Player
{
	public string ConnectionId { get; set; }
	public string Username { get; set; }
	public int Health { get; set; }

	public Player(string connectionId, string username, int health = 5)
	{
		ConnectionId = connectionId;
		Username = username;
		Health = health;
	}
}