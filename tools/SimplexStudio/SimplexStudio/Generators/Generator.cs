using ImTools;
using RcherNZ.AccidentalNoise;

namespace SimplexStudio.Generators
{
    public class Generator
    {
        private readonly int _width;
        private readonly int _height;

        public Generator(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public MapData GetCombinerData(ImplicitCombiner combiner)
        {
            var mapData = new MapData(_width, _height);

            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var s = 0 + x / (float)_width;
                    var t = 0 + y / (float)_height;

                    var heightValue = (float)combiner.Get(s, t);
                    if (heightValue > mapData.Max)
                    {
                        mapData.Max = heightValue;
                    }

                    if (heightValue < mapData.Min)
                    {
                        mapData.Min = heightValue;
                    }

                    mapData.Data[y * _width + x] = heightValue;
                }
            }

            return mapData;
        }

        public MapData GetFractalData(ImplicitFractal fractal)
        {
            var mapData = new MapData(_width, _height);

            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var s = 0 + x / (float)_width;
                    var t = 0 + y / (float)_height;

                    var heightValue = (float)fractal.Get(s, t);
                    if (heightValue > mapData.Max)
                    {
                        mapData.Max = heightValue;
                    }

                    if (heightValue < mapData.Min)
                    {
                        mapData.Min = heightValue;
                    }

                    mapData.Data[y * _width + x] = heightValue;
                }
            }

            return mapData;
        }
    }
}