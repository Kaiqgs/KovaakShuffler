using System;
using System.Diagnostics;
using System.IO;

namespace GvasFormat.Serialization.UETypes
{
    [DebuggerDisplay("Count = {Items.Length}", Name = "{Name}")]
    public sealed class UEArrayProperty : UEProperty
    {
        public UEArrayProperty() { }

        public UEArrayProperty(BinaryReader reader, long valueLength)
        {
            ItemType = reader.ReadUEString();
            var terminator = reader.ReadByte();
            if (terminator != 0)
                throw new FormatException($"Offset: 0x{reader.BaseStream.Position - 1:x8}. Expected terminator (0x00), but was (0x{terminator:x2})");

            // valueLength starts here
            var count = reader.ReadInt32();
            Items = new UEProperty[count];

            switch (ItemType)
            {
                case "StructProperty":
                    Items = Read(reader, count);
                    break;
                case "ByteProperty":
                    Items = UEByteProperty.Read(reader, valueLength, count);
                    break;
                default:
                {
                    for (var i = 0; i < count; i++)
                        Items[i] = UESerializer.Deserialize(null, ItemType, -1, reader);
                    break;
                }
            }
        }
        public override void Serialize(BinaryWriter writer) {
            NotifySerialize();
            writer.WriteUEString(Name); // Name (string)
            writer.WriteUEString(Type); // Type (string)
            // Size (long);
            
            var mstream = new MemoryStream(0);
            using(var bwriter = new BinaryWriter(mstream)){
                bwriter.WriteUEString(ItemType);//ItemType (string);
                bwriter.Write((byte)0);//Terminator (byte)0x00;
                bwriter.Write((int)Items.Length);//Length (int32);

                bool foundFirstStruct = false;
                foreach(var item in Items)
                {
                    if (item.Type == "StructProperty" && foundFirstStruct)
                    {
                        Console.WriteLine("Found Struct!!!");
                        var strc = (UEGenericStructProperty)item;
                        foreach (var strItem in strc.Properties)
                        {
                            Console.WriteLine("Serializing In Array = " + strItem.Type);
                            strItem.Serialize(bwriter);
                        }

                    }
                    else
                    {
                        item.Serialize(bwriter);
                        foundFirstStruct = true;
                    }
                    }
                
                writer.Write(mstream.Length);
                mstream.Position = 0;
                mstream.CopyTo(writer.BaseStream);
            }

            
            
            
        }

        public string ItemType;
        public UEProperty[] Items;
    }
}