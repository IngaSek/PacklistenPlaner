using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Provider;

namespace PacklistenPlaner;

public class PersonRepository
{
    private Dictionary<int, Person> elements = new ();
    public IEnumerable<Person> Elements { get { return elements.Values; } }

    public PersonRepository() { }

    public int Save(Person person)
    {
        if (person.PersonID == 0)
        {
            person.PersonID = elements.Count + 1;
        }
        elements[person.PersonID] = person;
        return person.PersonID;
    }

    public void RemoveById(int personId)
    {
        elements.Remove(personId);
    }

    public void Clear()
    {
        elements.Clear();
    }
}
