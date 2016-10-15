using SharpEnd.Collections;
using System.IO;
using System.Text;

namespace SharpEnd.Game.Data
{
    #region Data Classes
    public sealed class ReactorData
    {
        public int Identifier { get; set; }

        public void Load(BinaryReader reader)
        {
            Identifier = reader.ReadInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Identifier);
        }
    }
    #endregion

    internal sealed class ReactorDataProvider : SafeKeyedCollection<int, ReactorData>
    {
        private static ReactorDataProvider instance;

        public static ReactorDataProvider Instance
        {
            get
            {
                return instance ?? (instance = new ReactorDataProvider());
            }
        }

        private ReactorDataProvider() : base() { }

        protected override int GetKeyForItem(ReactorData item)
        {
            return item.Identifier;
        }

        public new ReactorData this[int identifier]
        {
            get
            {
                if (!Contains(identifier))
                {
                    using (FileStream stream = File.Open(Path.Combine("data", "reactors", identifier.ToString() + ".shd"), FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII))
                        {
                            ReactorData reactor = new ReactorData();

                            reactor.Load(reader);

                            Add(reactor);
                        }
                    }
                }

                return base[identifier];
            }
        }
    }
}
