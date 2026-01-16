using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PacklistenPlaner;

partial class App : Application
{
    private static IHost host;
    [STAThread]
    public static void Main(string[] args)
    {
        var builder = new HostBuilder();

        builder.ConfigureServices((context, services) =>
        {
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<DataRepository>();
            services.AddTransient<MainWindow>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<ReisenViewModel>();
            services.AddTransient<ReiseViewModel>();
            services.AddTransient<PersonenViewModel>();
            services.AddTransient<VorlagenViewModel>();
        });
        host = builder.Build();
        host.Start();

        var repository = host.Services.GetRequiredService<DataRepository>();
        Json_Serializer.LoadFromFile(repository, "daten.json");

        if(!repository.Reisen.Elements.Any())
        ErstelleTestDaten(repository);

        var app = new App();
        app.Run();
    }

    public App()
    {
        var navigation = host.Services.GetService<NavigationStore>();
        navigation.CurrentViewModel = host.Services.GetService<ReisenViewModel>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        host.Services.GetRequiredService<MainWindow>().Show();
        base.OnStartup(e);
    }
    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
    }

    private static void ErstelleTestDaten(DataRepository repository)
    {
        var person1 = new Person { Name = "Alice" };
        var person2 = new Person { Name = "Bob" };
        repository.Personen.Save(person1);
        repository.Personen.Save(person2);

        var reise1 = new Reise
        {
            Titel = "Sommerurlaub 2026",
            Startdatum = new DateTime(2024, 7, 1),
            Enddatum = new DateTime(2024, 7, 14),
            Beschreibung = "Campingurlaub",
            Planender = person1
        };

        var reise2 = new Reise
        {
            Titel = "Faltboottour 2026",
            Startdatum = new DateTime(2024, 12, 20),
            Enddatum = new DateTime(2025, 1, 2),
            Beschreibung = "Vielleicht mit dem Pionier Zweier, der Mosel. Und nicht nochmal Weser.",
            Planender = person2
        };
        
        var packliste1 = new Packliste { Name = "Persönliches" };
        packliste1.Eintraege.Add(new Eintrag { Name = "Buch", Menge = 1, Verantwortlicher = person1, Status = true  });
        packliste1.Eintraege.Add(new Eintrag { Name = "Fotoapperat", Menge = 1, Verantwortlicher = person1, Status = true });
        packliste1.Eintraege.Add(new Eintrag { Name = "Skatblatt", Menge = 1, Verantwortlicher = person1 });
        reise1.Packlisten.Add(packliste1);
        repository.Reisen.Save(reise1);

        var packliste2 = new Packliste { Name = "DKV" };
        packliste2.Eintraege.Add(new Eintrag { Name = "DKV Ökoschulung Nachweis", Menge = 1, Verantwortlicher = person1 });
        packliste2.Eintraege.Add(new Eintrag { Name = "DKV Mitgliedsausweis", Menge = 1, Verantwortlicher = person1, Status = true});
        packliste2.Eintraege.Add(new Eintrag { Name = "DKV Wimpel", Menge = 1, Verantwortlicher = person2 });
        reise2.Packlisten.Add(packliste2);
        repository.Reisen.Save(reise2);

        var vorlage1 = new Vorlage { Name = "Wandern", Typ = "Fortbewegungsmittel" };
        vorlage1.Gegenstaende.Add(new Gegenstand { Name = "Wanderschuhe", Standardmenge = 1 });
        vorlage1.Gegenstaende.Add(new Gegenstand { Name = "Rucksack", Standardmenge = 1 });
        vorlage1.Gegenstaende.Add(new Gegenstand { Name = "Wasserflasche", Standardmenge = 3 });
        vorlage1.Gegenstaende.Add(new Gegenstand { Name = "Landkarte", Standardmenge = 1 });
        repository.Vorlagen.Save(vorlage1);

        var vorlage2 = new Vorlage { Name = "Fahrradfahren", Typ = "Fortbewegungsmittel" };
        vorlage2.Gegenstaende.Add(new Gegenstand { Name = "Fahrrad", Standardmenge = 1 });
        vorlage2.Gegenstaende.Add(new Gegenstand { Name = "Satteltaschen", Standardmenge = 1 });
        vorlage2.Gegenstaende.Add(new Gegenstand { Name = "Helm", Standardmenge = 1 });
        vorlage2.Gegenstaende.Add(new Gegenstand { Name = "Flickzeug", Standardmenge = 1 });
        vorlage2.Gegenstaende.Add(new Gegenstand { Name = "Fahrradbrille", Standardmenge = 1 });
        vorlage2.Gegenstaende.Add(new Gegenstand { Name = "Trikot", Standardmenge = 2 });
        vorlage2.Gegenstaende.Add(new Gegenstand { Name = "Fahrradschuhe", Standardmenge = 1 });
        vorlage2.Gegenstaende.Add(new Gegenstand { Name = "Fahrradcomputer", Standardmenge = 1 });

        var vorlage3 = new Vorlage { Name = "Paddeln", Typ = "Fortbewegungsmittel" };
        vorlage3.Gegenstaende.Add(new Gegenstand { Name = "Faltboot", Standardmenge = 1 });
        vorlage3.Gegenstaende.Add(new Gegenstand { Name = "Seesack", Standardmenge = 6 });
        vorlage3.Gegenstaende.Add(new Gegenstand { Name = "Paddelhut", Standardmenge = 2 });
        vorlage3.Gegenstaende.Add(new Gegenstand { Name = "Schwamm", Standardmenge = 1 });
        vorlage3.Gegenstaende.Add(new Gegenstand { Name = "Spritzdecke", Standardmenge = 1 });
        vorlage3.Gegenstaende.Add(new Gegenstand { Name = "Persenning", Standardmenge = 1 });
        vorlage3.Gegenstaende.Add(new Gegenstand { Name = "Schwimmweste", Standardmenge = 2 });

        var vorlage4 = new Vorlage { Name = "Campingkocher", Typ = "Verpflegung" };
        vorlage4.Gegenstaende.Add(new Gegenstand { Name = "Kocher", Standardmenge = 1 });
        vorlage4.Gegenstaende.Add(new Gegenstand { Name = "Spiritus/Gas", Standardmenge = 1 });
        vorlage4.Gegenstaende.Add(new Gegenstand { Name = "Feuerzeug", Standardmenge = 1 });
        vorlage4.Gegenstaende.Add(new Gegenstand { Name = "Teller", Standardmenge = 3 });
        vorlage4.Gegenstaende.Add(new Gegenstand { Name = "Multifunktionsbesteck", Standardmenge = 3 });
        vorlage4.Gegenstaende.Add(new Gegenstand { Name = "Spüli", Standardmenge = 1 });
        vorlage4.Gegenstaende.Add(new Gegenstand { Name = "Küchenschwamm", Standardmenge = 1 });
        vorlage4.Gegenstaende.Add(new Gegenstand { Name = "Geschirrtuch", Standardmenge = 2 });
        vorlage4.Gegenstaende.Add(new Gegenstand { Name = "Taschenmesser", Standardmenge = 1 });
        vorlage4.Gegenstaende.Add(new Gegenstand { Name = "Gewürze", Standardmenge = 1 });
        vorlage4.Gegenstaende.Add(new Gegenstand { Name = "Tee", Standardmenge = 1 });

        var vorlage5 = new Vorlage { Name = "Snacks", Typ = "Verpflegung" };
        vorlage5.Gegenstaende.Add(new Eintrag { Name = "Wasserflasche", Standardmenge = 3 });
        vorlage5.Gegenstaende.Add(new Eintrag { Name = "Riegel", Standardmenge = 2, StandardmengeProTag = true });
        vorlage5.Gegenstaende.Add(new Eintrag { Name = "Studentenfutter", Standardmenge = 0.5, StandardmengeProTag = true  });
        vorlage5.Gegenstaende.Add(new Eintrag { Name = "Obst", Standardmenge = 2, StandardmengeProTag = true });

        var vorlage6 = new Vorlage { Name = "Zelten", Typ = "Übernachtungsart" };
        vorlage6.Gegenstaende.Add(new Eintrag { Name = "Zelt", Standardmenge = 1 });
        vorlage6.Gegenstaende.Add(new Eintrag { Name = "Isomatte", Standardmenge = 2 });
        vorlage6.Gegenstaende.Add(new Eintrag { Name = "Schlafsack", Standardmenge = 2 });
        vorlage6.Gegenstaende.Add(new Eintrag { Name = "Taschenlampe", Standardmenge = 1 });

        repository.Vorlagen.Save(vorlage1);
        repository.Vorlagen.Save(vorlage2);
        repository.Vorlagen.Save(vorlage3);
        repository.Vorlagen.Save(vorlage4);
        repository.Vorlagen.Save(vorlage5);
        repository.Vorlagen.Save(vorlage6);
    }
}
