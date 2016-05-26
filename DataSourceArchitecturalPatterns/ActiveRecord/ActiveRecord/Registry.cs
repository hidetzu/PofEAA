using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveRecord
{
    class Registry
    {
        private static Dictionary<int, Person> persons = new Dictionary<int, Person>();

        public static Person GetPerson(int id)
        {
            Person person = null;
            if (persons.ContainsKey(id))
            {
                person = persons[id];
            }
            return person;
        }

        public static void AddPerson(Person person)
        {
            persons.Add(person.Id, person);
        }

        public static void RemovePerson(Person person)
        {
            persons.Remove(person.Id);
        }
    }
}
