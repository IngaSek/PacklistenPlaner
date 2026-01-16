using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacklistenPlaner;

public class DataRepository
{
    public ReiseRepository Reisen { get; set; }
    public PersonRepository Personen { get; set; }
    public VorlageRepository Vorlagen { get; set; } 
    public GegenstandRepository Gegenstaende { get; set; }

    public DataRepository()
    {
        Reisen = new ReiseRepository();
        Personen = new PersonRepository();
        Vorlagen = new VorlageRepository();
        Gegenstaende = new GegenstandRepository();
    }

    public void Clear()
    {
        Reisen.Clear();
        Personen.Clear();
        Vorlagen.Clear();
        Gegenstaende.Clear();
    }
}
