using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PacklistenPlaner;

public partial class MainWindow : Window
{
    private NavigationStore navigation;
    public ObservableObject CurrentViewModel { get; set; }
    public MainWindow(IServiceProvider services)
    {
        InitializeComponent();

        DataContext = services.GetRequiredService<MainViewModel>();
        navigation = services.GetRequiredService<NavigationStore>();
        CurrentViewModel = navigation.CurrentViewModel;
        navigation.CurrentViewModelChanged += Navigation_CurrentViewModelChanged;
    }

    private void Navigation_CurrentViewModelChanged()
    {
        CurrentViewModel = navigation.CurrentViewModel;
    }
}