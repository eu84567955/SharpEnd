using SharpEnd.Game.Players;

namespace SharpEnd.Game.Maps
{
    public interface IControllable
    {
        Player Controller { get; set; }
    }
}
