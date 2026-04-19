using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Threading.Tasks;
using System.Collections.Generic;
using Kvalic_2.ViewModels;
using Kvalic_2.Data;
using Kvalic_2.Views;
using System;

namespace Kvalic_2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            if (DataContext is MainViewModel vm)
            {
                vm.RequestEditWithCollectionsWindow += ShowEditServiceWithCollectionsWindow;
                vm.RequestCollectionManagementWindow += ShowCollectionManagementWindow;
            }
        }

        private async void ShowCollectionManagementWindow()
        {
            var collectionViewModel = new CollectionManagementViewModel();
            var collectionWindow = new CollectionManagementWindow(collectionViewModel);
            
            await collectionWindow.ShowDialog<bool>(this);
            
            if (DataContext is MainViewModel vm)
            {
                await vm.ReloadData();
            }
        }

        private async void ShowEditServiceWithCollectionsWindow(Service service, List<int> currentCollections, bool isNew)
        {
            var editViewModel = new EditServiceViewModel(service, isNew);
            var editWindow = new Kvalic_2.Views.EditServiceWindow(editViewModel);
            
            // Subscribe to the save event in the VM to get collection IDs
            List<int> selectedCollectionIds = new List<int>();
            editViewModel.OnSaveWithCollections += (s, ids) => {
                selectedCollectionIds = ids;
            };

            var result = await editWindow.ShowDialog<bool>(this);
            
            if (result && DataContext is MainViewModel vm)
            {
                await vm.SaveChangesWithCollections(service, selectedCollectionIds, isNew);
            }
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private async void CloseClick(object sender, RoutedEventArgs e)
        {
            await ShowExitDialog();
        }

        private async Task ShowExitDialog()
        {
            var dialog = new Window
            {
                Title = "Выход",
                Width = 300,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = false,
                Content = new StackPanel
                {
                    Spacing = 20,
                    Margin = new Avalonia.Thickness(20),
                    Children =
                    {
                        new TextBlock { Text = "Вы уверены, что хотите выйти?", HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center },
                        new StackPanel
                        {
                            Orientation = Avalonia.Layout.Orientation.Horizontal,
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                            Spacing = 10,
                            Children =
                            {
                                new Button 
                                { 
                                    Content = "Да", 
                                    Width = 60, 
                                    HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center 
                                },
                                new Button 
                                { 
                                    Content = "Нет", 
                                    Width = 60, 
                                    HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center 
                                }
                            }
                        }
                    }
                }
            };

            dialog.SystemDecorations = 0; 

            var stackPanel = (StackPanel)dialog.Content;
            var buttonPanel = (StackPanel)stackPanel.Children[1];
            var yesButton = (Button)buttonPanel.Children[0];
            var noButton = (Button)buttonPanel.Children[1];

            yesButton.Click += (s, e) => { dialog.Close(true); Close(); };
            noButton.Click += (s, e) => dialog.Close(false);

            await dialog.ShowDialog(this);
        }
    }
}