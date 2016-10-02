using SharpEnd.Utility;

namespace SharpEnd.Players
{
    internal sealed class Account
    {
        public int Identifier { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public Account(DatabaseQuery query)
        {
            Identifier = query.Get<int>("identifier");
            Username = query.Get<string>("username");
            Password = query.Get<string>("password");
        }
    }
}
