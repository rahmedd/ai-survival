
namespace api.Models;

public class Player(string id, string username, string? roomId, int health = 5, bool host = false)
{
    public string Id { get; set; } = id;
    public string Username { get; set; } = username;
    public string? RoomId { get; set; } = roomId;
    public int Health { get; set; } = health;
    public bool Host { get; set; } = host;

    public void TakeDamage(int damage = 1)
	{
		Health -= damage;
	}
}