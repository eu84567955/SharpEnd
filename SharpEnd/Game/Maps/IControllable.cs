using SharpEnd.Players;

namespace SharpEnd.Game.Maps
{
    internal interface IControllable
    {
        Player Controller { get; set; }
    }
}
