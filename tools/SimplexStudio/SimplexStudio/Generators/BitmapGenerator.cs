using System.Drawing;

namespace SimplexStudio.Generators
{
    public class BitmapGenerator
    {
        private const float DeepWater = 0.2f;
        private const float ShallowWater = 0.4f;
        private const float Sand = 0.5f;
        private const float Grass = 0.7f;
        private const float Forest = 0.8f;
        private const float Rock = 0.9f;
        private const float Snow = 1;

        private static Color DeepColor = Color.FromArgb(0, 0, 128);
        private static Color ShallowColor = Color.FromArgb(25, 25, 150);
        private static Color SandColor = Color.FromArgb(240, 240, 64);
        private static Color GrassColor = Color.FromArgb(50, 220, 20);
        private static Color ForestColor = Color.FromArgb(16, 160, 0 );
        private static Color RockColor = Color.FromArgb(128, 128, 128);
        private static Color SnowColor = Color.FromArgb(255, 255, 255);

        public static DirectBitmap GetBitmap(int width, int height, float[] heightValues)
        {
            var texture = new DirectBitmap(width, height);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    Color color;
                    var value = heightValues[y * width + x];
                    if (value < DeepWater)
                    {
                        color = DeepColor;
                    }
                    else if (value < ShallowWater)
                    {
                        color = ShallowColor;
                    }
                    else if (value < Sand)
                    {
                        color = SandColor;
                    }
                    else if (value < Grass)
                    {
                        color = GrassColor;
                    }
                    else if (value < Forest)
                    {
                        color = ForestColor;
                    }
                    else if (value < Rock)
                    {
                        color = RockColor;
                    }
                    else
                    {
                        color = SnowColor;
                    }

                    texture.SetPixel(x, y, color);
                }
            }

            return texture;
        }
    }
}