using System.IO;
using Microsoft.Xna.Framework;

namespace Nord.Client.Engine.Extensions
{
    public static class BinaryReaderExtensions
    {
        public static Point ReadPoint(this BinaryReader binaryReader)
        {
            var x = binaryReader.ReadInt32();
            var y = binaryReader.ReadInt32();
            return new Point(x, y);
        }

        public static Point3 ReadPoint3(this BinaryReader binaryReader)
        {
            var x = binaryReader.ReadSingle();
            var y = binaryReader.ReadSingle();
            var z = binaryReader.ReadSingle();
            return new Point3(x, y, z);
        }
    }
}
