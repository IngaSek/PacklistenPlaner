using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacklistenPlaner;

public class ReiseRepository
{
    private Dictionary<int, Reise> elements = new();
    public IEnumerable<Reise> Elements { get { return elements.Values; } }

    public ReiseRepository() { }
    public int Save(Reise reise)
    {
        if (reise.ReiseID == 0)
        {
            reise.ReiseID = elements.Count + 1;
        }
        elements[reise.ReiseID] = reise;
        return reise.ReiseID;
    }
    public void RemoveById(int reiseId)
    {
        elements.Remove(reiseId);
    }
    public void Clear()
    {
        elements.Clear();
    }
}
