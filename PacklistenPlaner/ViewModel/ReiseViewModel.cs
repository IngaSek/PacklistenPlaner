using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PacklistenPlaner;

public partial class ReiseViewModel : ObservableValidator
{
    private IServiceProvider services;
    private NavigationStore navigation;
    private DataRepository repository;
    private Reise reise;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [MinLength(3)]
    [NotifyCanExecuteChangedFor(nameof(OkCommand))]
    private string titel = string.Empty;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [NotifyCanExecuteChangedFor(nameof(OkCommand))]
    [NotifyPropertyChangedFor(nameof(Tage))]
    private DateTime startdatum = DateTime.Now;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [NotifyCanExecuteChangedFor(nameof(OkCommand))]
    [NotifyPropertyChangedFor(nameof(Tage))]
    private DateTime enddatum = DateTime.Now;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(OkCommand))]
    private string beschreibung = string.Empty;

    [ObservableProperty]
    private Person? planender = null;

    [ObservableProperty]
    private string planenderName = string.Empty;

    public ObservableCollection<Packliste> Packlisten { get; private set; } = new ObservableCollection<Packliste>();
    public ObservableCollection<Person> AllePersonen { get; set; } = new ObservableCollection<Person>();
    public ObservableCollection<Vorlage> Vorlagen { get; private set; } = new ObservableCollection<Vorlage>();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPacklisteSelected))]
    [NotifyCanExecuteChangedFor(nameof(PacklisteEntfernenCommand))]
    [NotifyCanExecuteChangedFor(nameof(EintragHinzufuegenCommand))]
    private Packliste selectedPackliste = null;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEintragSelected))]
    [NotifyCanExecuteChangedFor(nameof(EintragEntfernenCommand))]
    private Eintrag selectedEintrag = null;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsVorlageSelected))]
    [NotifyCanExecuteChangedFor(nameof(VorlageNutzenCommand))]
    private Vorlage selectedVorlage = null;

    public bool IsPacklisteSelected { get { return SelectedPackliste != null; } }
    public bool IsEintragSelected { get { return SelectedEintrag != null; } }
    public bool IsVorlageSelected { get { return SelectedVorlage != null; } }

    public int Tage { get { return (Enddatum - Startdatum).Days + 1; } }

    public bool IsValid { get { return !HasErrors; } }

    [RelayCommand]
    public void PacklisteHinzufuegen()
    {
        var newPackliste = new Packliste { Name = "Neue Packliste"};
        Packlisten.Add(newPackliste);
        SelectedPackliste = newPackliste;
    }

    [RelayCommand(CanExecute = nameof(IsPacklisteSelected))]
    private void PacklisteEntfernen()
    {
        Packlisten.Remove(SelectedPackliste);
        SelectedPackliste = null;
    }

    [RelayCommand(CanExecute = nameof(IsVorlageSelected))]
    public void VorlageNutzen()
    {
        var newPackliste = new Packliste { Name = SelectedVorlage.Name };

        foreach (var gegenstand in SelectedVorlage.Gegenstaende)
        {
            var newEintrag = new Eintrag();

            newEintrag.Name = gegenstand.Name;
            if (gegenstand.StandardmengeProTag)
            {
                newEintrag.Menge = Convert.ToInt32(gegenstand.Standardmenge * Tage);
            }
            else
            {
                newEintrag.Menge = Convert.ToInt32(gegenstand.Standardmenge);
            }
            newPackliste.Eintraege.Add(newEintrag);
        }
        Packlisten.Add(newPackliste);
        SelectedPackliste = newPackliste;
    }

    [RelayCommand(CanExecute = nameof(IsPacklisteSelected))]
    private void EintragHinzufuegen()
    {
        var newEintrag = new Eintrag { Name = "Neuer Eintrag", Menge = 1};
        SelectedPackliste.Eintraege.Add(newEintrag);
        SelectedEintrag = newEintrag;
        OnPropertyChanged(nameof(Packlisten));
    }

    [RelayCommand(CanExecute = nameof(IsEintragSelected))]
    public void EintragEntfernen()
    {
        SelectedPackliste.Eintraege.Remove(SelectedEintrag);
        SelectedEintrag = null;
        OnPropertyChanged(nameof(Packlisten));
    }

    [RelayCommand]
    public void FortschrittAktualisieren()
    {
        OnPropertyChanged(nameof(Packlisten));
    }

    [RelayCommand(CanExecute = nameof(IsValid))]
    public void Ok()
    {
        if (Startdatum > Enddatum)
        {
            MessageBox.Show("Enddatum darf nicht vor Startdatum liegen.");
            return;
        }

        foreach (var packliste in Packlisten)
        {
            foreach (var eintrag in packliste.Eintraege)
            {
                if (eintrag.Verantwortlicher != null)
                {
                    var person = AllePersonen.FirstOrDefault(p => p.Name == eintrag.Verantwortlicher.Name);
                    if (person != null)
                    {
                        eintrag.Verantwortlicher = person;
                    }
                }
            }
        }

        reise.Titel = Titel;
        reise.Startdatum = Startdatum;
        reise.Enddatum = Enddatum;
        reise.Beschreibung = Beschreibung;
        reise.Planender = FindeOderErzeugePerson(PlanenderName, Planender);

        reise.Packlisten.Clear();
        foreach (var packliste in Packlisten)
        {
            reise.Packlisten.Add(packliste);
        }

        repository.Reisen.Save(reise);

        var viewModel = services.GetRequiredService<ReisenViewModel>();
        navigation.NavigateTo(viewModel);
    }

    [RelayCommand]
    public void Abbrechen()
    {
        var viewModel = services.GetRequiredService<ReisenViewModel>();
        navigation.NavigateTo(viewModel);
    }
    private Person FindeOderErzeugePerson(string name, Person person)
    {
        if (person != null)
        {
            return person;
        }
        if (!string.IsNullOrWhiteSpace(name))
        {
            foreach (var per in AllePersonen)
            {
                if (per.Name.Equals(name))
                    return per;
            }

            var newPerson = new Person { Name = name.Trim() };
            repository.Personen.Save(newPerson);
            AllePersonen.Add(newPerson);
            return newPerson;
        }
        return null;
    }

    public ReiseViewModel(IServiceProvider services, Reise reise)
    {
        this.services = services;
        navigation = services.GetRequiredService<NavigationStore>();
        repository = services.GetRequiredService<DataRepository>();
        this.reise = reise;
        
        Titel = reise.Titel ?? string.Empty;
        Startdatum = reise.Startdatum;
        Enddatum = reise.Enddatum;
        Beschreibung = reise.Beschreibung;
        Planender = reise.Planender;
        PlanenderName = reise.Planender?.Name ?? string.Empty;

        foreach (var person in repository.Personen.Elements)
        {
            AllePersonen.Add(person);
        }
        foreach (var vorlage in repository.Vorlagen.Elements)
        {
            Vorlagen.Add(vorlage);
        }

        foreach (var packliste in reise.Packlisten)
        {
            Packlisten.Add(packliste);

            foreach (var eintrag in packliste.Eintraege)
            {
                if (eintrag.Verantwortlicher != null)
                {
                    foreach (var person in AllePersonen)
                    {
                        if (person.Name == eintrag.Verantwortlicher.Name)
                        {
                            eintrag.Verantwortlicher = person;
                            break;
                        }
                    }
                }
            }
        }
        ValidateAllProperties();
    }
}
