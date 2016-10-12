using SharpEnd.Players;

namespace SharpEnd.Maps
{
    internal interface IControllable
    {
        Player Controller { get; set; }
    }
}
