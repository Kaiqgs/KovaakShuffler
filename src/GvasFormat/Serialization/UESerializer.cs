using System;
using System.IO;
using System.Linq;
using System.Text;
using GvasFormat.Serialization.UETypes;
using GvasFormat.Utils;

namespace GvasFormat.Serialization
{
    public static partial class UESerializer
    {
        public static Gvas Read(Stream stream)
        {
            using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
            {
                var header = reader.ReadBytes(Gvas.Header.Length);
                if (!Gvas.Header.SequenceEqual(header))
                    throw new FormatException($"Invalid header, expected {Gvas.Header.AsHex()}");

                var result = new Gvas();
                result.SaveGameVersion = reader.ReadInt32();
                result.PackageVersion = reader.ReadInt32();
                result.EngineVersion.Major = reader.ReadInt16();
                result.EngineVersion.Minor = reader.ReadInt16();
                result.EngineVersion.Patch = reader.ReadInt16();
                result.EngineVersion.Build = reader.ReadInt32();
                result.EngineVersion.BuildId = reader.ReadUEString();
                result.CustomFormatVersion = reader.ReadInt32();
                result.CustomFormatData.Count = reader.ReadInt32();
                result.CustomFormatData.Entries = new CustomFormatDataEntry[result.CustomFormatData.Count];
                for (var i = 0; i < result.CustomFormatData.Count; i++)
                {
                    var entry = new CustomFormatDataEntry();
                    entry.Id = new Guid(reader.ReadBytes(16));
                    entry.Value = reader.ReadInt32();
                    result.CustomFormatData.Entries[i] = entry;
                }
                result.SaveGameType = reader.ReadUEString();

                while (UEProperty.Read(reader) is UEProperty prop)
                    result.Properties.Add(prop);

                return result;
            }
        }

        public static void Write(Gvas gvas, Stream stream)
        {
            using (var writer = new BinaryWriter(stream, Encoding.ASCII, true))
            {

                writer.Write(Gvas.Header);
                writer.Write(gvas.SaveGameVersion);
                writer.Write(gvas.PackageVersion);
                writer.Write(gvas.EngineVersion.Major);
                writer.Write(gvas.EngineVersion.Minor);
                writer.Write(gvas.EngineVersion.Patch);
                writer.Write(gvas.EngineVersion.Build);
                writer.WriteUEString(gvas.EngineVersion.BuildId);
                writer.Write(gvas.CustomFormatVersion);
                writer.Write(gvas.CustomFormatData.Count);

                foreach (var entry in gvas.CustomFormatData.Entries)
                {
                    writer.Write(entry.Id.ToByteArray());
                    writer.Write(entry.Value);
                }

                writer.WriteUEString(gvas.SaveGameType);

                foreach (var prop in gvas.Properties)
                {
                    Console.WriteLine($"GVAS Serializing {prop}");
                    
                    prop.Serialize(writer);
                }

                writer.Write((int)0);

            }
        }

        public static void WriteKovaakSpecific(Gvas gvas, Stream stream)
        {
            using (var writer = new BinaryWriter(stream, Encoding.ASCII, true))
            {

                writer.Write(Gvas.Header);
                writer.Write(gvas.SaveGameVersion);
                writer.Write(gvas.PackageVersion);
                writer.Write(gvas.EngineVersion.Major);
                writer.Write(gvas.EngineVersion.Minor);
                writer.Write(gvas.EngineVersion.Patch);
                writer.Write(gvas.EngineVersion.Build);
                writer.WriteUEString(gvas.EngineVersion.BuildId);
                writer.Write(gvas.CustomFormatVersion);
                writer.Write(gvas.CustomFormatData.Count);

                foreach (var entry in gvas.CustomFormatData.Entries)
                {
                    writer.Write(entry.Id.ToByteArray());
                    writer.Write(entry.Value);
                }

                writer.WriteUEString(gvas.SaveGameType);


                ((UEGenericStructProperty)gvas.Properties[0]).Serialize(writer);
                ((UENoneProperty)gvas.Properties[1]).Serialize(writer);
                    

                writer.Write((int)0);

            }
        }

    }
}
