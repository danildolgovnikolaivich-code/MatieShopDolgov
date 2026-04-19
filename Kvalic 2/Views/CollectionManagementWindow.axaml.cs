using Avalonia.Controls;
using Kvalic_2.ViewModels;
using System;

namespace Kvalic_2.Views
{
    public partial class CollectionManagementWindow : Window
    {
        public CollectionManagementWindow()
        {
            InitializeComponent();
        }

        public CollectionManagementWindow(CollectionManagementViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            
            viewModel.OnSave += () => {
                this.Close(true);
            };
            
            viewModel.OnCancel += () => {
                this.Close(false);
            };
        }
    }
}