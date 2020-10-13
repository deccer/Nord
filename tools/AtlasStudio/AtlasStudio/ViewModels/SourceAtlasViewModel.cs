using System.Drawing;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AtlasStudio.Commands;
using AtlasStudio.Converters;
using Microsoft.Win32;

namespace AtlasStudio.ViewModels
{
    public class SourceAtlasViewModel : ViewModel
    {
        private Bitmap _atlasBitmap;

        public SourceAtlasViewModel()
        {
            LoadSourceAtlasCommand = new DelegateCommand(LoadSourceAtlasCommandHandler);
            Name = "unnamed";
        }

        public ICommand LoadSourceAtlasCommand { get; }

        public BitmapSource AtlasBitmapSource { get; private set; }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetValue(ref _name, value);
        }

        public void LoadSourceAtlasCommandHandler()
        {
            var ofd = new OpenFileDialog();
            var result = ofd.ShowDialog();

            if (result == true)
            {
                _atlasBitmap?.Dispose();
                _atlasBitmap = Image.FromFile(ofd.FileName) as Bitmap;
                AtlasBitmapSource = BitmapConverter.Convert(_atlasBitmap);
                OnPropertyChanged(nameof(AtlasBitmapSource));

                Name = Path.GetFileName(ofd.FileName);
            }
        }
    }
}
