using System.Collections.ObjectModel;
using System.Windows.Input;
using AtlasStudio.Commands;

namespace AtlasStudio.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            QuitCommand = new DelegateCommand(QuitCommandHandler);
            AddSourceAtlasCommand = new DelegateCommand(AddSourceAtlasCommandHandler);
            RemoveSourceAtlasCommand = new DelegateCommand(RemoveSourceAtlasCommandHandler);
            SourceAtlases = new ObservableCollection<SourceAtlasViewModel>();
        }

        public ICommand AddSourceAtlasCommand { get; }

        public ICommand RemoveSourceAtlasCommand { get; }

        public ICommand QuitCommand { get; }

        private ObservableCollection<SourceAtlasViewModel> _sourceAtlases;

        public ObservableCollection<SourceAtlasViewModel> SourceAtlases
        {
            get => _sourceAtlases;
            set => SetValue(ref _sourceAtlases, value);
        }

        private SourceAtlasViewModel _selectedSourceAtlas;

        public SourceAtlasViewModel SelectedSourceAtlas
        {
            get => _selectedSourceAtlas;
            set => SetValue(ref _selectedSourceAtlas, value);
        }

        private void QuitCommandHandler()
        {
            App.Current?.MainWindow?.Close();
        }

        private void AddSourceAtlasCommandHandler()
        {
            SourceAtlases.Add(new SourceAtlasViewModel());
        }

        private void RemoveSourceAtlasCommandHandler()
        {
            if (SelectedSourceAtlas != null)
            {
                SourceAtlases.Remove(SelectedSourceAtlas);
            }
        }
    }
}
