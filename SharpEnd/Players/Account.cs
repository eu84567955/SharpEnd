using SharpEnd.Utility;

namespace SharpEnd.Players
{
    internal sealed class Account
    {
        public int Identifier { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string PIC { get; private set; }
        public EAccountLevel Level { get; private set; }

        public EPICState PICState
        {
            get
            {
                if (false) // TODO: When we have configuration - change this to the actual configuration setting.
                {
                    return EPICState.Disable;
                }
                else if (string.IsNullOrEmpty(PIC))
                {
                    return EPICState.Register;
                }
                else
                {
                    return EPICState.Assigned;
                }
            }
        }

        public Account(DatabaseQuery query)
        {
            Identifier = query.Get<int>("identifier");
            Username = query.Get<string>("username");
            Password = query.Get<string>("password");
            PIC = query.Get<string>("pic");
            Level = (EAccountLevel)query.Get<byte>("level");
        }
    }
}
