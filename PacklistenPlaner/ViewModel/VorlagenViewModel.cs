using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PacklistenPlaner;

public partial class VorlagenViewModel : ObservableObject
{
    private IServiceProvider services;
    private NavigationStore navigation;
    private DataRepository repository;

    public ObservableCollection<Vorlage> Vorlagen { get; set; } = new ();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsVorlageSelected))]
    [NotifyCanExecuteChangedFor(nameof(VorlageEntfernenCommand))]
    [NotifyCanExecuteChangedFor(nameof(GegenstandHinzufuegenCommand))]
    private Vorlage selectedVorlage = null;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsGegenstandSelected))]
    [NotifyCanExecuteChangedFor(nameof(GegenstandEntfernenCommand))]
    private Gegenstand selectedGegenstand = null;

    public bool IsVorlageSelected { get { return SelectedVorlage != null; } }
    public bool IsGegenstandSelected { get { return SelectedGegenstand != null; } }

    [RelayCommand(CanExecute = nameof(IsVorlageSelected))]
    private void VorlageEntfernen()
    {
        repository.Vorlagen.RemoveById(SelectedVorlage.VorlageID);
        Vorlagen.Remove(SelectedVorlage);
        UpdateVorlagen();
    }

    [RelayCommand]
    public void VorlageHinzufuegen()
    {
        var neueVorlage = new Vorlage { Name = "Neue Vorlage" };
        repository.Vorlagen.Save(neueVorlage);
        Vorlagen.Add(neueVorlage);
        SelectedVorlage = neueVorlage;
    }

    [RelayCommand(CanExecute = nameof(IsVorlageSelected))]
    private void GegenstandHinzufuegen()
    {
        var neuerGegenstand = new Gegenstand { Name = "Neuer Gegenstand", Standardmenge = 1 };
        SelectedVorlage.Gegenstaende.Add(neuerGegenstand);
        SelectedGegenstand = neuerGegenstand;
        OnPropertyChanged(nameof(Vorlagen));
    }


    [RelayCommand(CanExecute = nameof(IsGegenstandSelected))]
    private void GegenstandEntfernen()
    {
        SelectedVorlage.Gegenstaende.Remove(SelectedGegenstand);
        SelectedGegenstand = null;
        OnPropertyChanged(nameof(Vorlagen));
    }

    public VorlagenViewModel(IServiceProvider services)
    {
        this.services = services;
        navigation = services.GetRequiredService<NavigationStore>();
        repository = services.GetRequiredService<DataRepository>();
        UpdateVorlagen();
    }

    private void UpdateVorlagen()
    {
        Vorlagen.Clear();
        foreach (var vorlage in repository.Vorlagen.Elements)
        {
            Vorlagen.Add(vorlage);
        }
    }
}
