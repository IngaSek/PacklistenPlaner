using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using PacklistenPlaner;

namespace PacklistenPlanerTest;

public class ReiseTests
{
    [Fact]
    public void ReiseStatus_0Eintraege_ist_0()
    {
        var reise = new Reise();
        var packliste = new Packliste();
        
        reise.Packlisten.Add(packliste);

        var status = reise.Status;

        Assert.Equal(0, status);
    }

    [Fact]
    public void ReiseStatus_1EintragTrue_ist_100()
    {
        var reise = new Reise();
        var packliste = new Packliste();
        var eintrag = new Eintrag() { Status = true };

        packliste.Eintraege.Add(eintrag);
        reise.Packlisten.Add(packliste);
        var status = reise.Status;

        Assert.Equal(100, status);
    }

    [Fact]
    public void ReiseStatus_1EintragFalse_ist_0()
    {
        var reise = new Reise();
        var packliste = new Packliste();
        var eintrag = new Eintrag() { Status = false };

        packliste.Eintraege.Add(eintrag);
        reise.Packlisten.Add(packliste);
        var status = reise.Status;

        Assert.Equal(0, status);
    }

    [Fact]
    public void ReiseTage_01012027_01012027_ist_1()
    {
        var reise = new Reise();
        reise.Startdatum = new DateTime(2027, 1, 1);
        reise.Enddatum = new DateTime(2027, 1, 1);
        int tage = reise.Tage;

        Assert.Equal(1, tage);
    }
}
