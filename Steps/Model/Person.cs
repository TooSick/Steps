using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace Steps.Model
{
    public class Person
    {
        [JsonPropertyName("User")]
        public string FullName { get; set; }
        public uint Rank { get; set; }
        [JsonIgnore]
        public List<uint> Ranks { get; set; }
        public string Status { get; set; }
        [JsonIgnore]
        public List<string> Statuses { get; set; }
        public uint Steps { get; set; }
        public double AverageSteps { get; private set; }
        public uint BestResult { get; private set; }
        public uint WorstResult { get; private set; }
        [JsonIgnore]
        public ObservableCollection<double> AllSteps { get; set; }
        [JsonIgnore]
        public Brush RowColor { get; set; }

        public Person()
        {
            AllSteps = new ObservableCollection<double>();
            AllSteps.CollectionChanged += NewStepsAdded;
            Ranks = new List<uint>();
            Statuses = new List<string>();
            RowColor = Brushes.White;
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
            ChangeColor();
        }

        private void CalculatingAverageSteps()
        {
            if (AverageSteps == 0)
            {
                AverageSteps = AllSteps[^1];
            }
            else
            {
                AverageSteps = ((AverageSteps * (AllSteps.Count - 1)) + AllSteps[^1]) / AllSteps.Count;
            }
        }

        private void CalculatingBestResult()
        {
            BestResult = (uint)AllSteps.Max();
        }

        private void CalculatingWorstResult()
        {
            WorstResult = (uint)AllSteps.Min();
        }

        private void ChangeColor()
        {
            if (BestResult > (AverageSteps + (AverageSteps * 0.2)) || WorstResult < (AverageSteps - (AverageSteps * 0.2)))
            {
                RowColor = Brushes.Red;
            }
        }
    }
}
