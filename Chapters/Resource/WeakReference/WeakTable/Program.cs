namespace WeakReferenceSample.WeakTable
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    class Program
    {
        static void Main()
        {
            WeakKey();
            StrongKey();
        }

        private static void WeakKey()
        {
            var people = new[]
            {
                new Person {Id = 1, Name = "Jurian Naul" },
                new Person {Id = 2, Name = "Thomas Bent" },
                new Person {Id = 3, Name = "Ellen Carson" },
                new Person {Id = 4, Name = "Katrina Lauran" },
                new Person {Id = 5, Name = "Monica Ausbach" },
            };

            var locations = new ConditionalWeakTable<Person, string>();

            locations.Add(people[0], "Shinon");
            locations.Add(people[1], "Lance");
            locations.Add(people[2], "Pidona");
            locations.Add(people[3], "Loanne");
            locations.Add(people[4], "Loanne");

            foreach (var p in people)
            {
                if (locations.TryGetValue(p, out var location))
                    Console.WriteLine(p.Name + " at " + location);
            }
        }

        private static void StrongKey()
        {
            var people = new[]
            {
                new Person {Id = 1, Name = "Jurian Naul" },
                new Person {Id = 2, Name = "Thomas Bent" },
                new Person {Id = 3, Name = "Ellen Carson" },
                new Person {Id = 4, Name = "Katrina Lauran" },
                new Person {Id = 5, Name = "Monica Ausbach" },
            };

            var locations = new Dictionary<Person, string>();

            locations.Add(people[0], "Shinon");
            locations.Add(people[1], "Lance");
            locations.Add(people[2], "Pidona");
            locations.Add(people[3], "Loanne");
            locations.Add(people[4], "Loanne");

            foreach (var p in people)
            {
                if (locations.TryGetValue(p, out var location))
                    Console.WriteLine(p.Name + " at " + location);
            }
        }
    }
}
