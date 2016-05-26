using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveRecord
{
    class Program
    {
        static void Main(string[] args)
        {
            var taro = new Person()
            {
                Id = 0,
                LastName = "山田",
                FirstName = "太郎",
                NumberOfDependents = 1,
            };

            taro.Insert();


            Person person = Person.Find(0);

            System.Console.WriteLine(person.FirstName);
            System.Console.WriteLine(person.LastName);
            System.Console.WriteLine(person.NumberOfDependents);

            person.NumberOfDependents = 10;
            person.Update();
            System.Console.WriteLine(person.FirstName);
            System.Console.WriteLine(person.LastName);
            System.Console.WriteLine(person.NumberOfDependents);

            // Display the number of command line arguments:
            System.Console.WriteLine(args.Length);
        }
    }
}
