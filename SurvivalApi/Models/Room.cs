using api.Models;

namespace SurvivalApi.Models;

public class Room(string id)
{
	public string Id { get; set; } = id;
	public List<Player> Players { get; set; } = [];
}
