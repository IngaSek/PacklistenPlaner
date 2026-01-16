using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacklistenPlaner;

public class Reise
{
    public int ReiseID { get; set; } = 0;
    public string Titel { get; set; }
    public DateTime Startdatum { get; set; }
    public DateTime Enddatum { get; set; }
    public int Tage { get { return (Enddatum - Startdatum).Days + 1; } }
    public string Beschreibung { get; set; }
    public int Status 
    { 
        get 
        { 
            int eintrageGesamt = 0;
            int erledigt = 0;
            foreach (var packliste in Packlisten)
            {
                eintrageGesamt += packliste.Eintraege.Count;
                foreach (var eintrag in packliste.Eintraege)
                {
                    if (eintrag.Status)
                        erledigt ++;
                }
            }
            if (eintrageGesamt == 0)
                return 0;
            return Convert.ToInt32(Convert.ToDouble(erledigt) / eintrageGesamt * 100); 
        } 
    }
    public Person Planender { get; set; }
    public List<Packliste> Packlisten { get; set; } = new List<Packliste>();

    public Reise()
    {
        Titel = string.Empty;
        Startdatum = DateTime.Now;
        Enddatum = DateTime.Now.AddDays(1);
        Beschreibung = string.Empty;
        Planender = null;
    }

    public string ToString()
    {
        return $"{ReiseID}, {Titel}";
    }
}
