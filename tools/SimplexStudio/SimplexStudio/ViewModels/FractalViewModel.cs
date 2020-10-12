using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Imaging;
using RcherNZ.AccidentalNoise;
using SimplexStudio.Generators;

namespace SimplexStudio.ViewModels
{
    public class FractalViewModel : ViewModel
    {
        private ImplicitFractal _fractal;

        public FractalViewModel()
        {
            Seed = 0;
            Lacunarity = 2.0;
            Frequency = 1.0;
            Gain = 0;
            Octaves = 8;

            FractalType = FractalType.Billow;
            InterpolationType = InterpolationType.Linear;
        }

        private BitmapSource _bitmap;
        public BitmapSource Bitmap
        {
            get => _bitmap;
            set => SetValue(ref _bitmap, value);
        }

        public ObservableCollection<FractalType> FractalTypes
        {
            get;
        } = new ObservableCollection<FractalType>(Enum.GetValues(typeof(FractalType)).Cast<FractalType>());

        public ObservableCollection<BasisType> BasisTypes
        {
            get;
        } = new ObservableCollection<BasisType>(Enum.GetValues(typeof(BasisType)).Cast<BasisType>());

        public ObservableCollection<InterpolationType> InterpolationTypes
        {
            get;
        } = new ObservableCollection<InterpolationType>(Enum.GetValues(typeof(InterpolationType)).Cast<InterpolationType>());

        private FractalType _fractalType;
        public FractalType FractalType
        {
            get => _fractalType;
            set => SetValue(ref _fractalType, value, OnChanged);
        }

        private BasisType _basisType;

        public BasisType BasisType
        {
            get => _basisType;
            set => SetValue(ref _basisType, value, OnChanged);
        }

        private InterpolationType _interpolationType;

        public InterpolationType InterpolationType
        {
            get => _interpolationType;
            set => SetValue(ref _interpolationType, value, OnChanged);
        }

        private int _seed;

        public int Seed
        {
            get => _seed;
            set => SetValue(ref _seed, value, OnChanged);
        }

        private double _frequency;

        public double Frequency
        {
            get => _frequency;
            set => SetValue(ref _frequency, value, OnChanged);
        }

        private double _gain;

        public double Gain
        {
            get => _gain;
            set => SetValue(ref _gain, value, OnChanged);
        }

        private int _octaves;

        public int Octaves
        {
            get => _octaves;
            set => SetValue(ref _octaves, value, OnChanged);
        }

        private double _lacunarity;

        public double Lacunarity
        {
            get => _lacunarity;
            set => SetValue(ref _lacunarity, value, OnChanged);
        }

        public void AddToCombiner(ImplicitCombiner combiner)
        {
            combiner.AddSource(_fractal);
        }

        private void OnChanged()
        {
            _fractal = new ImplicitFractal(_fractalType, _basisType, _interpolationType)
            {
                Seed = _seed,
                Frequency = _frequency,
                Gain = _gain,
                Octaves = _octaves,
                Lacunarity = _lacunarity
            };

            UpdateBitmapPreview();
        }

        private void UpdateBitmapPreview()
        {
            var generator = new Generator(128, 128);
            var mapData = generator.GetFractalData(_fractal);
            var heightData = Normalizer.NormalizeMapData(mapData);
            using var bitmap = BitmapGenerator.GetBitmap(128, 128, heightData);
            Bitmap = BitmapConverter.Convert(bitmap.Bitmap);
        }
    }
}