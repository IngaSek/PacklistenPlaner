using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacklistenPlaner;

public class GegenstandRepository
{
    private Dictionary<int, Gegenstand> elements = new();
    public IEnumerable<Gegenstand> Elements { get { return elements.Values; } }

    public GegenstandRepository() { }

    public int Save(Gegenstand gegenstand)
    {
        if (gegenstand.GegenstandID == 0)
        {
            gegenstand.GegenstandID = elements.Count + 1;
        }
        elements[gegenstand.GegenstandID] = gegenstand;
        return gegenstand.GegenstandID;
    }

    public void RemoveById(int gegenstandId)
    {
        elements.Remove(gegenstandId);
    }

    public void Clear()
    {
        elements.Clear();
    }
}
