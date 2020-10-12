using SimplexStudio.Generators;

namespace SimplexStudio.ViewModels
{
    public class Normalizer
    {
        public static float[] NormalizeMapData(MapData mapData)
        {
            var normalizedValues = new float[mapData.Width * mapData.Height];
            for (var y = 0; y < mapData.Height; y++)
            {
                for (var x = 0; x < mapData.Width; x++)
                {
                    var value = mapData.Data[y * mapData.Width + x];

                    value = (value - mapData.Min) / (mapData.Max - mapData.Min);
                    
                    normalizedValues[y * mapData.Width + x] = value;
                }
            }

            return normalizedValues;
        }
    }
}