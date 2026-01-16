using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacklistenPlaner;

public class Person
{
    public int PersonID { get; set; } = 0;
    public string Name { get; set; }
    public Person()
    {
        Name = string.Empty;
    }
}
