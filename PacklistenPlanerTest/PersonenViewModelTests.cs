using Microsoft.Extensions.DependencyInjection;
using PacklistenPlaner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PacklistenPlanerTest;

public class PersonenViewModelTests
{
    private IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddSingleton<NavigationStore>();
        services.AddSingleton<DataRepository>();
        return services.BuildServiceProvider();
    }

    [Fact]
    public void ElementSelected_IsPersonSelected_ReturnsTrue()
    {
        var services = CreateServiceProvider();
        var viewModel = new PersonenViewModel(services);
        var person = new Person { PersonID = 1, Name = "Test Person" };
        viewModel.Personen.Add(person);

        viewModel.SelectedPerson = person;
        
        Assert.True(viewModel.IsPersonSelected);
    }

    [Fact]
    public void PersonHinzufuegen_fuegtPersonHinzu()
    {
        var services = CreateServiceProvider();
        var viewModel = new PersonenViewModel(services);
        int anzahlVorher = viewModel.Personen.Count;

        viewModel.PersonHinzufuegenCommand.Execute(null);
        Assert.Equal(anzahlVorher + 1, viewModel.Personen.Count);
    }
}
