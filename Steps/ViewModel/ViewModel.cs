using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Steps.Model;
using Steps.AuxiliaryClasses;
using LiveCharts;
using LiveCharts.Wpf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Steps.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        public List<Person> People { get; set; }
        public string[] Labels { get; set; }
        private SeriesCollection series;
        public SeriesCollection Series 
        { 
            get { return series; } 
            set 
            {
                series = value;
                OnPropertyChanged("Series");
            }
        }
        public Func<double, string> Formatter { get; set; }
        private Person selectedPerson;
        public Person SelectedPerson 
        { 
            get { return selectedPerson; } 
            set 
            {
                selectedPerson = value;
                OnPropertyChanged("SelectedPerson");
                EditDataForChart();
            }
        }
        private ClickCommand buttonClick;
        public ClickCommand ButtonClick
        {
            get 
            {
                return buttonClick ?? (buttonClick = new ClickCommand(obj =>
                {
                    if (SelectedPerson != null)
                    {
                        JsonSerializer.SerializingData(SelectedPerson);
                    }
                    else
                    {
                        MessageBox.Show("Select the person whose data you want to export."); 
                    }
                }));
            }
        }

        public ViewModel()
        {
            People = GetPeople();
            Labels = GetLabels();
            EditDataForChart();
        }

        private List<Person> GetPeople()
        {
            string[] data = Directory.GetFiles("Data");
            HashSet<Person> tempPeople = new HashSet<Person>();
            Dictionary<int, List<Person>> statisticsForAllDays = new Dictionary<int, List<Person>>();
            for (int i = 0; i < data.Length; i++)
            {
                statisticsForAllDays.Add(i, Deserializer.DeserializingData(data[i]));
                tempPeople.AddRange(statisticsForAllDays[i]);
            }
            List<Person> people = tempPeople.ToList();
            for (int i = 0; i < people.Count(); i++)
            {
                for (int j = 0; j < statisticsForAllDays.Count(); j++)
                {
                    if (statisticsForAllDays[j].Exists(p => p.FullName == people[i].FullName))
                    {
                        Person personToAdd = statisticsForAllDays[j].Find(p => people[i].FullName == p.FullName);
                        people[i].AllSteps.Add(personToAdd.Steps);
                        people[i].Statuses.Add(personToAdd.Status);
                        people[i].Ranks.Add(personToAdd.Rank);
                    }
                    else
                    {
                        people[i].AllSteps.Add(0);
                        people[i].Statuses.Add(string.Empty);
                        people[i].Ranks.Add(0);
                    }
                }
            }
            return people;
        }

        private string[] GetLabels()
        {
            string[] tempLabels = new string[People[0].AllSteps.Count()];
            for (int i = 0; i < tempLabels.Count(); i++)
            {
                tempLabels[i] += (i + 1).ToString() + " day";
            }
            return tempLabels;
        }

        private void EditDataForChart()
        {
            if (SelectedPerson != null)
            {
                Series = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Values = new ChartValues<double>((IEnumerable<double>)SelectedPerson.AllSteps),
                        DataLabels = true,
                        LabelPoint = point => point.Y.ToString(),
                    }
                };
                Formatter = value => value.ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
