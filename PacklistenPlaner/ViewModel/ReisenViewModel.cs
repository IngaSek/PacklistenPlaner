using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PacklistenPlaner;

public partial class ReisenViewModel : ObservableObject
{
    private IServiceProvider services;
    private NavigationStore navigation;
    private DataRepository repository;
    public ObservableCollection<Reise> Reisen { get; set; } = new ();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsReiseSelected))]
    [NotifyCanExecuteChangedFor(nameof(ReiseEntfernenCommand))]
    [NotifyCanExecuteChangedFor(nameof(ReiseBearbeitenCommand))]
    private Reise selectedReise = null;

    public bool IsReiseSelected { get { return SelectedReise != null; } }
    
    [RelayCommand(CanExecute = nameof(IsReiseSelected))]
    private void ReiseEntfernen()
    {
        repository.Reisen.RemoveById(SelectedReise.ReiseID);
        UpdateReisen();
    }

    [RelayCommand(CanExecute = nameof(IsReiseSelected))]
    private void ReiseBearbeiten()
    {
        var viewmodel = new ReiseViewModel(services, SelectedReise);
        navigation.NavigateTo(viewmodel);
    }
    [RelayCommand]
    public void ReiseHinzufuegen()
    {
        selectedReise = null;
        var newReise = new Reise();
        var viewmodel = new ReiseViewModel(services, newReise);
        navigation.NavigateTo(viewmodel);
    }
    public ReisenViewModel(IServiceProvider services)
    {
        this.services = services;
        navigation = services.GetRequiredService<NavigationStore>();
        repository = services.GetRequiredService<DataRepository>();
        UpdateReisen();
    }

    private void UpdateReisen()
    {
        Reisen.Clear();
            foreach (var reise in repository.Reisen.Elements)
            {
                Reisen.Add(reise);
            }
    }
}
