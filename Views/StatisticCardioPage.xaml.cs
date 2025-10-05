using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.data;
using WorkoutDiary.Model;
using LiveChartsCore.SkiaSharpView.Painting;
using System.ComponentModel;
using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Maui.Controls;

namespace WorkoutDiary.Views
{
    public partial class StatisticCardioPage : ContentPage, INotifyPropertyChanged
    {


        private readonly CardioDataBase _database;
        public ObservableCollection<Cardio> Cardio;
        private string _selectPart;
        public StatisticCardioPage()
        {
            InitializeComponent();
            _database = new CardioDataBase();
            //LoadChart();
            Cardio = new ObservableCollection<Cardio>();
            LoadCardio();
            
            OnAppearing();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_selectPart != null)
            {
                UpdateChart();
            }
            //this.SizeChanged += (s, e) =>
            //{
            //    if (this.Width > 0)
            //    {
            //        int widthPerPoint = 80;

            //        int width1 = Cardio.Count * widthPerPoint;
            //        int width2 = Cardio.Count * widthPerPoint;

            //        int finalWidth = Math.Max(Cardio.Count * widthPerPoint, (int)this.Width);

            //        chartView.WidthRequest = finalWidth;
            //        chartViewCalories.WidthRequest = finalWidth;
            //        ChartContainer.WidthRequest = finalWidth;
            //    }
            //};

        }
        private async void LoadCardio()
        {
            var db = await _database.GetCardioAsync();
            var cardioDB = db.Select(x => x.Name).Distinct().ToList();
            namePicker.ItemsSource = cardioDB; // Ustawienie listy w Picker

        }


        private async Task LoadChartv2(string selectedPart)
        {
            _selectPart = selectedPart;

            chartView.Chart = null;

            var db = await _database.GetCardioAsync(); //zmiana wykresu na liniowy tempo na minute
            var dbOrderBy = db.Where(x => x.Name == selectedPart)
                .GroupBy(x => x.DateTime.Date)
                .Select(group => new
                {
                    Date = group.Key,
                    TotalDistance = group.Sum(x => x.Distance) / 1000.0,  // Suma dystansu
                    TotalTime = group.Sum(x => x.Time),          // Suma czasu
                    TotalCalories = group.Sum(x => x.Calories)   // Suma kalorii
                })
                .OrderBy(x => x.Date)
                .ToList();

            if (dbOrderBy.Count == 0)
                return;

            var distanceEntries = new List<ChartEntry>();
            var caloriesEntries = new List<ChartEntry>();
            int widthPerPoint = 80; // np. 80 pikseli na punkt
            int chartWidth = Math.Max(dbOrderBy.Count * widthPerPoint, (int)this.Width); // nie mniejszy niż ekran
            chartView.WidthRequest = chartWidth;
            int chartWidthCalories = Math.Max(dbOrderBy.Count * widthPerPoint, (int)this.Width); // nie mniejszy niż ekran
            chartViewCalories.WidthRequest = chartWidthCalories;
            ChartContainer.WidthRequest = chartWidthCalories;
            LabelName.Text = _selectPart;
            CountExercises.Text = dbOrderBy.Count().ToString();
            FullDistance.Text = (db.Sum(x => x.Distance) / 1000).ToString("0.00");
            ProgressTime.Text = (db.Sum(x => x.Time) / (db.Sum(x => x.Distance) / 1000)).ToString("0.00");
            MaxKcal.Text = db.Sum(x => x.Calories).ToString("0.00");
            AvgKcal.Text = (db.Sum(x => x.Calories) / (db.Sum(x => x.Distance) / 1000)).ToString("0.00");
            foreach (var data in dbOrderBy)
            {
                float avgPace = data.TotalDistance > 0 ? (float)(data.TotalTime / data.TotalDistance) : 0; // Tempo na minutę
                float caloriesBurned = (float)data.TotalCalories; // Spalone kalorie

                distanceEntries.Add(new ChartEntry(avgPace)
                {
                    Label = data.Date.ToString("dd.MM"),
                    ValueLabel = avgPace.ToString("0.00") + " min/km",
                    ValueLabelColor = SkiaSharp.SKColor.Parse("#66b3ff"),
                    Color = SKColor.Parse("#66b3ff") // Niebieska linia dla tempa
                });

                caloriesEntries.Add(new ChartEntry(caloriesBurned)
                {
                    Label = data.Date.ToString("dd.MM"),
                    ValueLabel = caloriesBurned.ToString() + " kcal",
                    Color = SKColor.Parse("#b32400"), // Czerwona linia dla kalorii
                    ValueLabelColor = SkiaSharp.SKColor.Parse("#b32400")
                });
            }

            // Wykres dla dwóch serii danych
            var distanceChart = new LineChart
            {
                Entries = distanceEntries,
                LineMode = LineMode.Straight, // Prosta linia
                LineSize = 5, // Grubość linii
                ValueLabelTextSize = 30,
                EnableYFadeOutGradient = false,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                IsAnimated = false,
                MaxValue = dbOrderBy.Max(x => x.TotalTime), // Automatyczna skala dla Y
                MinValue = 0,
                BackgroundColor = SKColors.Transparent, // Przezroczyste tło
                PointMode = PointMode.Circle, // Brak punktów na wykresie (opcjonalnie)
                LabelTextSize = 30,
                ValueLabelOption = ValueLabelOption.TopOfElement,
                LabelColor = SKColor.Parse("#000000"), // Białe etykiety
            };

            var caloriesChart = new LineChart
            {
                Entries = caloriesEntries,
                LineMode = LineMode.Straight, // Prosta linia
                LineSize = 5, // Grubość linii
                ValueLabelTextSize = 30,
                EnableYFadeOutGradient = false,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                IsAnimated = false,
                MaxValue = dbOrderBy.Max(x => x.TotalCalories), // Automatyczna skala dla Y
                MinValue = 0,
                BackgroundColor = SKColors.Transparent, // Przezroczyste tło
                PointMode = PointMode.Circle, // Brak punktów na wykresie (opcjonalnie)
                LabelTextSize = 30,
                ValueLabelOption = ValueLabelOption.TopOfElement,
                LabelColor = SKColor.Parse("#000000"), // Białe etykiety
            };

            chartView.Chart = distanceChart;
            chartViewCalories.Chart = caloriesChart;

            //UpdateChart(chartEntries);
        }
        private async void UpdateChart()
        {
            var db = await _database.GetCardioAsync(); //zmiana wykresu na liniowy tempo na minute
            var dbOrderBy = db.Where(x => x.Name == _selectPart)
                .GroupBy(x => x.DateTime.Date)
                .Select(group => new
                {
                    Date = group.Key,
                    TotalDistance = group.Sum(x => x.Distance) / 1000.0,  // Suma dystansu
                    TotalTime = group.Sum(x => x.Time),          // Suma czasu
                    TotalCalories = group.Sum(x => x.Calories)   // Suma kalorii
                })
                .OrderBy(x => x.Date)
                .ToList();


            var distanceEntries = new List<ChartEntry>();
            var caloriesEntries = new List<ChartEntry>();
            int widthPerPoint = 80; // np. 80 pikseli na punkt
            int chartWidth = Math.Max(dbOrderBy.Count * widthPerPoint, (int)this.Width); // nie mniejszy niż ekran
            chartView.WidthRequest = chartWidth;
            int chartWidthCalories = Math.Max(dbOrderBy.Count * widthPerPoint, (int)this.Width); // nie mniejszy niż ekran
            chartViewCalories.WidthRequest = chartWidthCalories;
            ChartContainer.WidthRequest = chartWidthCalories;
            LabelName.Text = _selectPart;
            CountExercises.Text = dbOrderBy.Count().ToString();
            FullDistance.Text = (db.Sum(x => x.Distance) / 1000).ToString("0.00");
            ProgressTime.Text = (db.Sum(x => x.Time) / (db.Sum(x => x.Distance) / 1000)).ToString("0.00");
            MaxKcal.Text = db.Sum(x => x.Calories).ToString("0.0");
            AvgKcal.Text = (db.Sum(x => x.Calories) / (db.Sum(x => x.Distance)/1000)).ToString("0.00");
            foreach (var data in dbOrderBy)
            {
                float avgPace = data.TotalDistance > 0 ? (float)(data.TotalTime / data.TotalDistance) : 0; // Tempo na minutę
                float caloriesBurned = (float)data.TotalCalories; // Spalone kalorie
                
                distanceEntries.Add(new ChartEntry(avgPace)
                {
                    Label = data.Date.ToString("dd.MM"),
                    ValueLabel = avgPace.ToString("0.00") + " min/km",
                    ValueLabelColor = SkiaSharp.SKColor.Parse("#66b3ff"),
                    Color = SKColor.Parse("#66b3ff") // Niebieska linia dla tempa
                });

                caloriesEntries.Add(new ChartEntry(caloriesBurned)
                {
                    Label = data.Date.ToString("dd.MM"),
                    ValueLabel = caloriesBurned.ToString() + " kcal",
                    Color = SKColor.Parse("#b32400"), // Czerwona linia dla kalorii
                    ValueLabelColor = SkiaSharp.SKColor.Parse("#b32400")
                });
            }

            // Wykres dla dwóch serii danych
            var distanceChart = new LineChart
            {
                Entries = distanceEntries,
                LineMode = LineMode.Straight, // Prosta linia
                LineSize = 5, // Grubość linii
                ValueLabelTextSize = 30,
                EnableYFadeOutGradient = false,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                IsAnimated = false,
                MaxValue = dbOrderBy.Max(x=>x.TotalTime), // Automatyczna skala dla Y
                MinValue = 0,
                BackgroundColor = SKColors.Transparent, // Przezroczyste tło
                PointMode = PointMode.Circle, // Brak punktów na wykresie (opcjonalnie)
                LabelTextSize = 30,
                ValueLabelOption = ValueLabelOption.TopOfElement,
                LabelColor = SKColor.Parse("#000000"), // Białe etykiety
            };

            var caloriesChart = new LineChart
            {
                Entries = caloriesEntries,
                LineMode = LineMode.Straight, // Prosta linia
                LineSize = 5, // Grubość linii
                ValueLabelTextSize = 30,
                EnableYFadeOutGradient = false,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                IsAnimated = false,
                MaxValue = dbOrderBy.Max(x => x.TotalCalories), // Automatyczna skala dla Y
                MinValue = 0,
                BackgroundColor = SKColors.Transparent, // Przezroczyste tło
                PointMode = PointMode.Circle, // Brak punktów na wykresie (opcjonalnie)
                LabelTextSize = 30,
                ValueLabelOption = ValueLabelOption.TopOfElement,
                LabelColor = SKColor.Parse("#000000"), // Białe etykiety
            };

            chartView.Chart = distanceChart;
            chartViewCalories.Chart = caloriesChart;
        }

        private void PartSelected(object sender, EventArgs e)
        {
            if (namePicker.SelectedIndex != -1)
            {
                string selectedPart = namePicker.SelectedItem.ToString();
                _ = LoadChartv2(selectedPart); // Załaduj dane dla wybranej osoby
            }
        }


        private async void Button_Clicked_Strong(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///StatisticPage");
        }
    }
}
