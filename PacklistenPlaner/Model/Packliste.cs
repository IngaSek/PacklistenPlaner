using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacklistenPlaner;

public class Packliste : ObservableObject
{
    public int PacklisteID { get; set; } = 0;
    public string Name { get; set; }
    public int Fortschritt 
    { 
        get 
        {
            int gesamt = Eintraege.Count;
            int erledigt = 0;

            if (gesamt == 0)
                return 0;

            foreach (var eintrag in Eintraege)
            {
                if (eintrag.Status)
                    erledigt++;
            }
            return Convert.ToInt32(Convert.ToDouble(erledigt) / gesamt * 100);
        } 
    }
    public ObservableCollection<Eintrag> Eintraege { get; set; } = new ObservableCollection<Eintrag>();
    public override string ToString()
    {
        return $"{PacklisteID}, {Name}";
    }
}
