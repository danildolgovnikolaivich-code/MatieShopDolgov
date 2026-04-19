using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Kvalic_2.Data;
using System;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace Kvalic_2.ViewModels
{
    public class EditServiceViewModel : ViewModelBase
    {
        private string _name;
        private string? _description;
        private decimal _price;
        private Service _service;
        private bool _isNew;
        private ObservableCollection<CollectionSelection> _collections;

        public EditServiceViewModel(Service service, bool isNew = false)
        {
            _service = service;
            _isNew = isNew;
            _name = service.Name ?? string.Empty;
            _description = service.Description;
            _price = service.Price;
            _collections = new ObservableCollection<CollectionSelection>();

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);

            _ = LoadCollections();
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public decimal Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public ObservableCollection<CollectionSelection> Collections
        {
            get => _collections;
            set => SetProperty(ref _collections, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action<Service, List<int>>? OnSaveWithCollections;
        public event Action? OnCancel;

        private async Task LoadCollections()
        {
            using var context = new AppDbContext();
            var allCollections = await context.Collections.ToListAsync();
            
            var serviceCollectionIds = _isNew ? new List<int>() : 
                await context.Servicecollections
                    .Where(sc => sc.Serviceid == _service.Serviceid)
                    .Select(sc => sc.Collectionid)
                    .ToListAsync();

            var selections = allCollections.Select(c => new CollectionSelection
            {
                Collection = c,
                IsSelected = serviceCollectionIds.Contains(c.Collectionid)
            });

            Collections = new ObservableCollection<CollectionSelection>(selections);
        }

        private void Save()
        {
            _service.Name = Name;
            _service.Description = Description;
            _service.Price = Price;
            _service.Lastupdated = DateTime.Now;

            var selectedIds = Collections
                .Where(c => c.IsSelected)
                .Select(c => c.Collection.Collectionid)
                .ToList();

            OnSaveWithCollections?.Invoke(_service, selectedIds);
        }

        private void Cancel()
        {
            OnCancel?.Invoke();
        }
    }

    public class CollectionSelection : ViewModelBase
    {
        private bool _isSelected;
        public Collection Collection { get; set; } = null!;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}