using SharpEnd.Collections;
using System.IO;
using System.Text;

namespace SharpEnd.Game.Data
{
    #region Data Classes
    public sealed class PetData
    {
        public int Identifier { get; set; }
        public bool IsMultiPet { get; set; }
        public bool IsPermanent { get; set; }
        public bool HasPickupItem { get; set; }
        public bool HasAutoBuff { get; set; }
        public int Life { get; set; }
        public int Hunger { get; set; }

        public void Load(BinaryReader reader)
        {
            Identifier = reader.ReadInt32();
            IsMultiPet = reader.ReadBoolean();
            IsPermanent = reader.ReadBoolean();
            HasPickupItem = reader.ReadBoolean();
            HasAutoBuff = reader.ReadBoolean();
            Life = reader.ReadInt32();
            Hunger = reader.ReadInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Identifier);
            writer.Write(IsMultiPet);
            writer.Write(IsPermanent);
            writer.Write(HasPickupItem);
            writer.Write(HasAutoBuff);
            writer.Write(Life);
            writer.Write(Hunger);
        }
    }
    #endregion

    internal sealed class PetDataProvider : SafeKeyedCollection<int, PetData>
    {
        private static PetDataProvider instance;

        public static PetDataProvider Instance
        {
            get
            {
                return instance ?? (instance = new PetDataProvider());
            }
        }

        private PetDataProvider() : base() { }

        protected override int GetKeyForItem(PetData item)
        {
            return item.Identifier;
        }

        public new PetData this[int identifier]
        {
            get
            {
                if (!Contains(identifier))
                {
                    using (FileStream stream = File.Open(Path.Combine("data", "pets", identifier.ToString() + ".shd"), FileMode.Open, FileAccess.Read))
                    {
                        using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII))
                        {
                            PetData pet = new PetData();

                            pet.Load(reader);

                            Add(pet);
                        }
                    }
                }

                return base[identifier];
            }
        }
    }
}
