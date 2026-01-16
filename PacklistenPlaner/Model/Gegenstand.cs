using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacklistenPlaner;

public class Gegenstand
{
    public int GegenstandID { get; set; } = 0;
    public string Name { get; set; }
    public string Kategorie { get; set; }
    public double Standardmenge { get; set; }
    public bool StandardmengeProTag { get; set; } = false;
    public override string ToString()
    {
        return $"{GegenstandID}, {Name}";
    }
}
