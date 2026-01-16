using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace PacklistenPlaner;

public partial class MainViewModel : ObservableObject
{
    private IServiceProvider services;
    private NavigationStore navigation;
    private DataRepository repository;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsReiseView))]
    [NotifyPropertyChangedFor(nameof(IsReisenView))]
    [NotifyPropertyChangedFor(nameof(IsPersonenView))]
    private ObservableObject currentViewModel;

    public bool IsReiseView { get { return CurrentViewModel.GetType().Equals(typeof(ReiseViewModel)); } }
    public bool IsReisenView { get { return CurrentViewModel.GetType().Equals(typeof(ReisenViewModel)); } }
    public bool IsPersonenView { get { return CurrentViewModel.GetType().Equals(typeof(PersonenViewModel)); } }
    public bool IsVorlagenView { get { return CurrentViewModel.GetType().Equals(typeof(VorlagenViewModel)); } }

    public MainViewModel(IServiceProvider services)
    {
        this.services = services;
        navigation = services.GetRequiredService<NavigationStore>();
        repository = services.GetRequiredService<DataRepository>();
        
        CurrentViewModel = navigation.CurrentViewModel;
        navigation.CurrentViewModelChanged += () => { CurrentViewModel = navigation.CurrentViewModel; };
    }

    [RelayCommand]
    public void ExitApplication()
    {
        Application.Current.Shutdown();
    }

    [RelayCommand]
    public void NavigateToReisen()
    { 
        var viewModel = services.GetRequiredService<ReisenViewModel>();
        navigation.NavigateTo(viewModel);
    }
    [RelayCommand]
    public void NavigateToPersonen()
    {
        var viewModel = services.GetRequiredService<PersonenViewModel>();
        navigation.NavigateTo(viewModel);
    }
    [RelayCommand]
    public void NavigateToVorlagen()
    {
        var viewModel = services.GetRequiredService<VorlagenViewModel>();
        navigation.NavigateTo(viewModel);
    }

    [RelayCommand]
    public void Speichern()
    {
        Json_Serializer.SaveToFile(repository, "daten.json");
    }

    [RelayCommand]
    public void Laden()
    {
        Json_Serializer.LoadFromFile(repository, "daten.json");

        var viewModel = services.GetRequiredService<ReisenViewModel>();
        navigation.NavigateTo(viewModel);
    }

}
