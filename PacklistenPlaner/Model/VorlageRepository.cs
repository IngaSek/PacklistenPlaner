using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacklistenPlaner;

public class VorlageRepository
{
    private Dictionary<int, Vorlage> elements = new();
    public IEnumerable<Vorlage> Elements { get { return elements.Values; } }

    public VorlageRepository() { }

    public int Save(Vorlage vorlage)
    {
        if (vorlage.VorlageID == 0)
        {
            vorlage.VorlageID = elements.Count + 1;
        }
        elements[vorlage.VorlageID] = vorlage;
        return vorlage.VorlageID;
    }

    public void RemoveById(int vorlageId)
    {
        elements.Remove(vorlageId);
    }

    public void Clear()
    {
        elements.Clear();
    }
}
