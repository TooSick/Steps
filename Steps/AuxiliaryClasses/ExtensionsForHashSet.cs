using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steps.Model;

namespace Steps.AuxiliaryClasses
{
    static class ExtensionsForHashSet
    {
        public static void AddRange(this HashSet<Person> people, List<Person> adding)
        {
            for (int i = 0; i < adding.Count(); i++)
            {
                people.Add(adding[i]);
            }
        }
    }
}
