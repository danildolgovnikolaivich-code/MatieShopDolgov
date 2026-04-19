using Avalonia.Controls;
using Kvalic_2.ViewModels;
using System;

namespace Kvalic_2.Views
{
    public partial class EditServiceWindow : Window
    {
        public EditServiceWindow()
        {
            InitializeComponent();
        }

        public EditServiceWindow(EditServiceViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            
            viewModel.OnSaveWithCollections += (service, collectionIds) => {
                this.Close(true);
            };
            
            viewModel.OnCancel += () => {
                this.Close(false);
            };
        }
    }
}