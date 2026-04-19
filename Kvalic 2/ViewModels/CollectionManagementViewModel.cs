using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Kvalic_2.Data;
using System;
using System.Windows.Input;

namespace Kvalic_2.ViewModels
{
    public class CollectionManagementViewModel : ViewModelBase
    {
        private readonly AppDbContext _context;
        private ObservableCollection<Collection> _collections;
        private Collection? _selectedCollection;
        private string _newCollectionName = string.Empty;

        public CollectionManagementViewModel()
        {
            _context = new AppDbContext();
            _collections = new ObservableCollection<Collection>();
            
            AddCommand = new RelayCommand(AddCollection, () => !string.IsNullOrWhiteSpace(NewCollectionName));
            DeleteCommand = new RelayCommand(DeleteCollection, () => SelectedCollection != null);
            SaveCommand = new RelayCommand(SaveAndClose);
            CancelCommand = new RelayCommand(Cancel);

            _ = LoadCollections();
        }

        public ObservableCollection<Collection> Collections
        {
            get => _collections;
            set => SetProperty(ref _collections, value);
        }

        public Collection? SelectedCollection
        {
            get => _selectedCollection;
            set
            {
                if (SetProperty(ref _selectedCollection, value))
                {
                    (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string NewCollectionName
        {
            get => _newCollectionName;
            set
            {
                if (SetProperty(ref _newCollectionName, value))
                {
                    (AddCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action? OnSave;
        public event Action? OnCancel;

        private async Task LoadCollections()
        {
            var list = await _context.Collections.ToListAsync();
            Collections = new ObservableCollection<Collection>(list);
        }

        private void AddCollection()
        {
            if (string.IsNullOrWhiteSpace(NewCollectionName)) return;

            var collection = new Collection { Name = NewCollectionName };
            _context.Collections.Add(collection);
            Collections.Add(collection);
            NewCollectionName = string.Empty;
        }

        private void DeleteCollection()
        {
            if (SelectedCollection == null) return;

            _context.Collections.Remove(SelectedCollection);
            Collections.Remove(SelectedCollection);
            SelectedCollection = null;
        }

        private async void SaveAndClose()
        {
            try
            {
                await _context.SaveChangesAsync();
                OnSave?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении коллекций: {ex.Message}");
            }
        }

        private void Cancel()
        {
            OnCancel?.Invoke();
        }
    }
}