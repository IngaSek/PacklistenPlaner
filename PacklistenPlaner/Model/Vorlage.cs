using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacklistenPlaner;

public class Vorlage
{
    public int VorlageID { get; set; }
    public string Name { get; set; }
    public string Typ { get; set; }
    public string Beschreibung { get; set; }

    public ObservableCollection<Gegenstand> Gegenstaende { get; set; } = new ObservableCollection<Gegenstand>();
    public override string ToString()
    {
        return $"{VorlageID}, {Name}";
    }
}
