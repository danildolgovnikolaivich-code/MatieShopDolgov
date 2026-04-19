using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Kvalic_2.Data;
using System;

namespace Kvalic_2.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly AppDbContext? _context;
        private ObservableCollection<Service> _services;
        private int _currentPage = 1;
        private int _itemsPerPage = 3;
        private int _totalItems;
        private string _searchText = string.Empty;
        private Collection? _selectedCollection;
        private ObservableCollection<Collection> _collections;
        private bool _isSortAscending = true;

        public MainViewModel()
        {
            try
            {
                _context = new AppDbContext();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка инициализации контекста БД: {ex.Message}");
                _context = null!; 
            }
            
            _services = new ObservableCollection<Service>();
            _collections = new ObservableCollection<Collection>();
            
            NextPageCommand = new RelayCommand(NextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);
            ToggleSortCommand = new RelayCommand(ToggleSort);
            EditServiceCommand = new RelayCommand<Service>(EditService);
            AddServiceCommand = new RelayCommand(AddService);
            ManageCollectionsCommand = new RelayCommand(ManageCollections);

            if (_context != null)
            {
                _ = LoadData();
            }
        }

        public Action<Service, List<int>, bool>? RequestEditWithCollectionsWindow;
        public Action? RequestCollectionManagementWindow;

        private void EditService(Service service)
        {
            RequestEditWithCollectionsWindow?.Invoke(service, null!, false);
        }

        private void AddService()
        {
            var newService = new Service { Name = string.Empty, Price = 0 };
            RequestEditWithCollectionsWindow?.Invoke(newService, null!, true);
        }

        private void ManageCollections()
        {
            RequestCollectionManagementWindow?.Invoke();
        }

        public async Task SaveChangesWithCollections(Service service, List<int> collectionIds, bool isNew)
        {
            if (_context == null) return;

            try
            {
                if (isNew)
                {
                    _context.Services.Add(service);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.Entry(service).State = EntityState.Modified;

                    var oldRelations = _context.Servicecollections.Where(sc => sc.Serviceid == service.Serviceid);
                    _context.Servicecollections.RemoveRange(oldRelations);
                }

                foreach (var collectionId in collectionIds)
                {
                    _context.Servicecollections.Add(new Servicecollection 
                    { 
                        Serviceid = service.Serviceid, 
                        Collectionid = collectionId 
                    });
                }

                await _context.SaveChangesAsync();
                await LoadServices();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении услуги: {ex.Message}");
            }
        }

        public RelayCommand NextPageCommand { get; }
        public RelayCommand PreviousPageCommand { get; }
        public RelayCommand ToggleSortCommand { get; }
        public RelayCommand<Service> EditServiceCommand { get; }
        public RelayCommand AddServiceCommand { get; }
        public RelayCommand ManageCollectionsCommand { get; }

        public ObservableCollection<Service> Services
        {
            get => _services;
            set => SetProperty(ref _services, value);
        }

        public ObservableCollection<Collection> Collections
        {
            get => _collections;
            set => SetProperty(ref _collections, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    CurrentPage = 1;
                    _ = LoadServices();
                }
            }
        }

        public Collection? SelectedCollection
        {
            get => _selectedCollection;
            set
            {
                if (SetProperty(ref _selectedCollection, value))
                {
                    CurrentPage = 1;
                    _ = LoadServices();
                }
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (SetProperty(ref _currentPage, value))
                {
                    _ = LoadServices();
                    OnPropertyChanged(nameof(PaginationInfo));
                }
            }
        }

        public string PaginationInfo
        {
            get
            {
                int start = _totalItems == 0 ? 0 : (_currentPage - 1) * _itemsPerPage + 1;
                int end = Math.Min(_currentPage * _itemsPerPage, _totalItems);
                return $"{start}-{end} из {_totalItems}";
            }
        }

        public bool IsSortAscending
        {
            get => _isSortAscending;
            set
            {
                if (SetProperty(ref _isSortAscending, value))
                {
                    _ = LoadServices();
                }
            }
        }

        public void ToggleSort()
        {
            IsSortAscending = !IsSortAscending;
        }

        public async Task ReloadData()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            if (_context == null) return;

            try
            {
                var collectionsList = await _context.Collections.ToListAsync();
                Collections = new ObservableCollection<Collection>(collectionsList);
                await LoadServices();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        private async Task LoadServices()
        {
            if (_context == null) return;
            
            try
            {
                IQueryable<Service> query = _context.Services;

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    query = query.Where(s => s.Name.ToLower().Contains(SearchText.ToLower()));
                }

                if (SelectedCollection != null)
                {
                    query = query.Where(s => s.Servicecollections.Any(sc => sc.Collectionid == SelectedCollection.Collectionid));
                }

                _totalItems = await query.CountAsync();
                
                if (IsSortAscending)
                    query = query.OrderBy(s => s.Name);
                else
                    query = query.OrderByDescending(s => s.Name);

                var servicesList = await query
                    .Skip((CurrentPage - 1) * _itemsPerPage)
                    .Take(_itemsPerPage)
                    .ToListAsync();

                Services = new ObservableCollection<Service>(servicesList);
                OnPropertyChanged(nameof(PaginationInfo));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке услуг: {ex.Message}");
            }
        }

        public void NextPage()
        {
            if (CurrentPage * _itemsPerPage < _totalItems)
            {
                CurrentPage++;
            }
        }

        public void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
        }
    }
}