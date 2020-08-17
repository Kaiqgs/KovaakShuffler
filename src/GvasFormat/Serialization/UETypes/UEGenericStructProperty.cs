using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System;
using System.Linq;

namespace GvasFormat.Serialization.UETypes
{
    [DebuggerDisplay("Count = {Properties.Count}", Name = "{Name}")]
    public sealed class UEGenericStructProperty : UEStructProperty
    {
        public List<UEProperty> Properties = new List<UEProperty>();
        public string Header;
        public bool GUID;

        public override void Serialize(BinaryWriter writer)
        {
            NotifySerialize();
            writer.WriteUEString(Name);
            writer.WriteUEString(Type);
            Console.WriteLine(Header);
            var mstream = new MemoryStream();
            using(var bwriter = new BinaryWriter(mstream)){
                bwriter.WriteUEString(Header);
                bwriter.Write(Enumerable.Repeat((byte)0, 17).ToArray());
                foreach (var prop in Properties)
                {
                    
                    prop.Serialize(bwriter);
                    if (prop is UENoneProperty) break;
                }
                Console.WriteLine($"GenericStructProperty Length = {mstream.Length}");
                
                writer.Write(mstream.Length);
                mstream.Position = 0;
                mstream.CopyTo(writer.BaseStream);                
            }
        }


        public override string ToString()
        {
            return $"[{Name} {Type} {Header} {Properties.Count}]";
        }


    }
}