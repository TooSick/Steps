using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json.Serialization;

namespace Steps.Model
{
    public class Person
    {
        struct Data
        {
            public uint Rank { get; set; }
            public string Status { get; set; }
            public uint Steps { get; set; }
        }
        //нужен словарь для хранения шагов и дней
        [JsonPropertyName("User")]
        public string FullName { get; set; }
        //[JsonPropertyName("Rank")]
        //private uint rank;
        //public uint Rank 
        //{ 
        //    get => rank; 
        //    set 
        //    {
        //        rank = value;
        //        Ranks.Add(rank);
        //    } 
        //}
        //public List<uint> Ranks { get; set; }
        //private string status;
        //public string Status 
        //{
        //    get => status; 
        //    set 
        //    {
        //        Statuses.Add(status);
        //    } 
        //}
        //public List<string> Statuses { get; set; }
        //private uint steps;
        //public uint Steps 
        //{
        //    get { return steps; }
        //    set 
        //    { 
        //        steps = value;
        //        AllSteps.Add(steps);
        //    } 
        //}
        //public float AverageSteps { get; private set; }
        //public uint BestResult { get; private set; }
        //public uint WorstResult { get; private set; }
        //public ObservableCollection<uint> AllSteps { get; set; }

        public Person()
        {
            AllSteps = new ObservableCollection<uint>();
            AllSteps.CollectionChanged += NewStepsAdded;
            Ranks = new List<uint>();
            Statuses = new List<string>();
        }

        public override bool Equals(object obj)
        {
            if (obj is Person person)
            {
                return FullName == person.FullName;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.FullName);
        }

        private void NewStepsAdded(object? sender, NotifyCollectionChangedEventArgs e)
        {
            CalculatingAverageSteps();
            CalculatingBestResult();
            CalculatingWorstResult();
        }

        private void CalculatingAverageSteps()
        {
            uint allSteps = 0;
            foreach (uint s in AllSteps)
            {
                allSteps += s;
            }

            AverageSteps = allSteps / AllSteps.Count();
        }

        private void CalculatingBestResult()
        {
            BestResult = AllSteps.Max();
        }

        private void CalculatingWorstResult()
        {
            WorstResult = AllSteps.Min();
        }
    }
}
