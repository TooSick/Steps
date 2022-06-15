using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steps.Model;

namespace Steps.ViewModel
{
    public class ViewModel
    {
        private Dictionary<uint, List<Person>> DailyRecords { get; set; }
        public List<Person> People { get; set; }

        public ViewModel()
        {
            DailyRecords = new Dictionary<uint, List<Person>>();
            GetAllActivities();
            People = new List<Person>();
            AddUniquePeople();
            AddStepsToUniquePerson();
        }

        private void AddUniquePeople()
        {
            HashSet<Person> tempSet = new HashSet<Person>(DailyRecords[0]);
            for (uint i = 1; i < DailyRecords.Count(); i++)
            {
                for (int j = 0; j < DailyRecords[i].Count(); j++)
                {
                    tempSet.Add(DailyRecords[i][j]);
                }
            }

            People = tempSet.ToList();
        }

        private void AddStepsToUniquePerson()
        {
            for (int i = 0; i < People.Count(); i++)
            {
                List<Person> tempList = new List<Person>();
                for (uint j = 0; j < DailyRecords.Count(); j++)
                {
                    var tempPerson = (from p in DailyRecords[j] where p.FullName == People[i].FullName && p.Steps != People[i].Steps select p).ToList();
                    tempList.AddRange(tempPerson);
                }

                for (int j = 0; j < tempList.Count(); j++)
                {
                    People[i].AllSteps.Add(tempList[j].Steps);
                    // возможно надо добавлять в список и rank и status
                }
            }
        }

        private void GetAllActivities()
        {
            string[] data = Directory.GetFiles(@"C:\Users\meshc\OneDrive\Рабочий стол\Projects\Steps\Steps\Data\");
            for (uint i = 0; i < data.Length; i++)
            {
                DailyRecords.Add(i, Deserializer.DeserializingData(data[i]));
            }
        }
    }
}
