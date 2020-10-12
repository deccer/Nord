namespace SimplexStudio.Generators
{
    public class MapData
    {
        public int Width { get; }
        public int Height { get; }

        public float[] Data;

        public float Min { get; set; }

        public float Max { get; set; }

        public MapData(int width, int height)
        {
            Width = width;
            Height = height;
            Data = new float[width * height];
            Min = float.MaxValue;
            Max = float.MinValue;
        }
    }
}