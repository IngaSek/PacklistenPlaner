using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacklistenPlaner;

public partial class PersonenViewModel : ObservableObject
{
    private IServiceProvider services;
    private NavigationStore navigation;
    private DataRepository repository;

    public ObservableCollection<Person> Personen { get; set; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPersonSelected))]
    [NotifyCanExecuteChangedFor(nameof(PersonEntfernenCommand))]
    private Person selectedPerson = null;
    public bool IsPersonSelected { get { return SelectedPerson != null; } }

    [RelayCommand(CanExecute = nameof(IsPersonSelected))]
    public void PersonEntfernen()
    {
        repository.Personen.RemoveById(SelectedPerson.PersonID);
        foreach (var reise in repository.Reisen.Elements)
        {
            if (reise.Planender != null && reise.Planender.PersonID == SelectedPerson.PersonID)
                reise.Planender = null;

            foreach (var packliste in reise.Packlisten)
            {
                foreach (var eintrag in packliste.Eintraege)
                {
                    if (eintrag.Verantwortlicher != null && eintrag.Verantwortlicher.PersonID == SelectedPerson.PersonID)
                        eintrag.Verantwortlicher = null;
                }
            }
        }
        Personen.Remove(SelectedPerson);
        UpdatePersonen();
    }

    [RelayCommand]
    public void PersonHinzufuegen()
    {
        var neuePerson = new Person { Name = "Neue Person" };
        repository.Personen.Save(neuePerson);
        Personen.Add(neuePerson);
        SelectedPerson = neuePerson;
    }

    public PersonenViewModel(IServiceProvider services)
    {
        this.services = services;
        navigation = services.GetRequiredService<NavigationStore>();
        repository = services.GetRequiredService<DataRepository>();
        UpdatePersonen();
    }

    private void UpdatePersonen()
    {
        Personen.Clear();
        foreach (var person in repository.Personen.Elements)
        {
            Personen.Add(person);
        }
    }
}