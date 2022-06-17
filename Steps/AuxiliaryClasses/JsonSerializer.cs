using Steps.Model;
using System.IO;

namespace Steps.AuxiliaryClasses
{
    public static class JsonSerializer
    {
        public static async void SerializingData(Person person)
        {
            Directory.CreateDirectory($"{person.FullName}");
            for (int i = 0; i < person.AllSteps.Count; i++)
            {
                using (FileStream fs = new FileStream($"{person.FullName}\\{i + 1} day.json", FileMode.OpenOrCreate))
                {
                    person.Steps = (uint)person.AllSteps[i];
                    person.Rank = person.Ranks[i];
                    person.Status = person.Statuses[i];
                    await System.Text.Json.JsonSerializer.SerializeAsync(fs, person);
                }
            }
        }
    }
}
