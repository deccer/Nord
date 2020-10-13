using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using RcherNZ.AccidentalNoise;
using Shared.Mvvm.Commands;
using Shared.Mvvm.Converters;
using Shared.Mvvm.ViewModels;
using SimplexStudio.Generators;

namespace SimplexStudio.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public ICommand AddFractalCommand { get; }
        public ICommand RemoveFractalCommand { get; }
        public ICommand CombineFractalCommand { get; }

        public ObservableCollection<FractalViewModel> Fractals { get; }

        public ObservableCollection<CombinerType> CombinerTypes
        {
            get;
        } = new ObservableCollection<CombinerType>(Enum.GetValues(typeof(CombinerType)).Cast<CombinerType>());

        private CombinerType _combinerType;

        public CombinerType CombinerType
        {
            get => _combinerType;
            set => SetValue(ref _combinerType, value, CombineFractal);
        }

        private BitmapSource _combinedBitmap;

        public BitmapSource CombinedBitmap
        {
            get => _combinedBitmap;
            set => SetValue(ref _combinedBitmap, value);
        }

        public MainViewModel()
        {
            AddFractalCommand = new DelegateCommand(AddFractal);
            RemoveFractalCommand = new DelegateCommand(RemoveFractal);
            CombineFractalCommand = new DelegateCommand(CombineFractal);
            Fractals = new ObservableCollection<FractalViewModel>();
            _combinerType = CombinerType.Multiply;
        }

        private void AddFractal()
        {
            Fractals.Add(new FractalViewModel());
        }

        private void RemoveFractal()
        {
            if (Fractals.Count > 0)
            {
                Fractals.Remove(Fractals.Last());
            }
        }

        private void CombineFractal()
        {
            var combiner = new ImplicitCombiner(_combinerType);
            foreach (var fractal in Fractals)
            {
                fractal.AddToCombiner(combiner);
            }

            var generator = new Generator(128, 128);
            var mapData = generator.GetCombinerData(combiner);
            var normalizedData = Normalizer.NormalizeMapData(mapData);
            using var bitmap = BitmapGenerator.GetBitmap(128, 128, normalizedData);
            CombinedBitmap = BitmapConverter.Convert(bitmap.Bitmap);
            OnPropertyChanged(nameof(CombinedBitmap));
        }
    }
}
