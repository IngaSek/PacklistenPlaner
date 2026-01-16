using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacklistenPlaner;

public class Eintrag : Gegenstand
{
    public int EintragID { get; set; } = 0;
    public bool Status { get; set; } = false;
    public int Menge { get; set; }
    public Person Verantwortlicher { get; set; } = null;
    public override string ToString()
    {
        return $"{Name}: {Menge} x , {Status}";
    }
}
