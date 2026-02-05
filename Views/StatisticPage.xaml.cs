using Microcharts;
using Microcharts.Maui;
using SkiaSharp;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDiary.data;
using WorkoutDiary.Model;

namespace WorkoutDiary.Views
{
    public partial class StatisticPage : ContentPage
    {

        private readonly TodoItemDatabase _database;
        public ObservableCollection<BodyParts> BodyParts;
        private string _selectPart;
        public StatisticPage()
        {
            InitializeComponent();
            //chartView.Chart = new LineChart { Entries = entries };
            _database = new TodoItemDatabase();
            //LoadChart();
            BodyParts = new ObservableCollection<BodyParts>();
            LoadPart();
            OnAppearing();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_selectPart != null)
            {
                UpdateChart();
            }
        }

        private async void LoadPart()
        {
            var db = await _database.GetInvoiceAsync();
            var bodypartsDB = db.Select(x => x.Part).Where(x => !string.IsNullOrWhiteSpace(x))
                .OrderBy(x => x, StringComparer.CurrentCultureIgnoreCase).Distinct().ToList();
            namePicker.ItemsSource = bodypartsDB; // Ustawienie listy w Picker
            
        }
        private async Task LoadChartv2(string selectedPart)
        {
            chartView.Chart = null;
            _selectPart = selectedPart;
            var db = await _database.GetInvoiceAsync();
            var dbOrderBy1 = db.Where(x=>x.Part == selectedPart).OrderBy(x => x.DateTime).ToList();
            var dbOrderBy = db.Where(x => x.Part == selectedPart)
                  .GroupBy(x => x.DateTime.Date)  // Grupowanie po samej dacie (ignorujemy godziny)
                  .Select(group => new
                  {
                      Date = group.Key,  // Data
                      MaxWeight = group.Max(x => x.Weight),  // Najwyższa waga danego dnia
                      MinWeight = group.Min(x => x.Weight)
                  })
                  .OrderBy(x => x.Date)  // Sortowanie po dacie
                  .ToList();
            if (dbOrderBy.Count == 0)
                return;

            int widthPerPoint = 80; // np. 80 pikseli na punkt
            int chartWidth = Math.Max(dbOrderBy.Count * widthPerPoint, (int)this.Width); // nie mniejszy niż ekran
            chartView.WidthRequest = chartWidth;
            double firstweight = dbOrderBy.First().MaxWeight;
            double lastweight = dbOrderBy.Last().MaxWeight;
            DateTime firstDay = dbOrderBy.First().Date;
            DateTime lastDay = dbOrderBy.Last().Date;
            var minValue = dbOrderBy.Min(x => x.MaxWeight);
            var maxValue = dbOrderBy.Max(x => x.MaxWeight);

            // Dodaj mały margines 10% po bokach
            var range = maxValue - minValue;

            // Jeśli zakres jest za mały (np. 5 kg), ustaw minimalny rozstaw, żeby wykres nie był „płaski”
            if (range < 10) range = 10;

            var yMax = maxValue + range * 0.3f;       // +10% od góry
            var yMin = Math.Max(0, minValue - range * 0.1f); // -10% od dołu
            LabelPart.Text = selectedPart;
            ProgressLabel.Text = firstweight.ToString()+"=>"+maxValue.ToString()+"("+(maxValue-firstweight).ToString()+"kg)";
            StartExercises.Text = firstDay.ToString("dd.MM.yyyy");
            ProgressTime.Text = dbOrderBy.Count().ToString();
            var chartEntries = dbOrderBy.Select(entry => new Microcharts.ChartEntry((float?)(entry.MaxWeight))
            {
                Label = entry.Date.ToString("dd.MM"),
                ValueLabel = (entry.MaxWeight).ToString("0.0"),
                Color = SkiaSharp.SKColor.Parse("#000000"), // Kolor linii
                //TextColor = SkiaSharp.SKColor.Parse("#27AE60"),
                
                ValueLabelColor = SkiaSharp.SKColor.Parse("#000000")
                
            }).ToList();
            var lineChart = new Microcharts.LineChart
            {
                Entries = chartEntries,
                LineMode = LineMode.Straight, // Prosta linia
                LineSize = 3, // Grubość linii
                ValueLabelTextSize = 30,
                EnableYFadeOutGradient = false,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                IsAnimated = false,
                MaxValue = yMax,
                MinValue = yMin, 
                BackgroundColor = SKColors.Transparent, // Przezroczyste tło
                PointMode = PointMode.Circle, // Brak punktów na wykresie (opcjonalnie)
                LabelTextSize = 30,
                ValueLabelOption = ValueLabelOption.TopOfElement,
                LabelColor = SKColor.Parse("#000000"), 

            };

            chartView.Chart = lineChart;
        }
        private async void UpdateChart()
        {
            var db = await _database.GetInvoiceAsync();
            var dbOrderBy1 = db.Where(x => x.Part == _selectPart).OrderBy(x => x.DateTime).ToList();
            var dbOrderBy = db.Where(x => x.Part == _selectPart)
                  .GroupBy(x => x.DateTime.Date)  // Grupowanie po samej dacie (ignorujemy godziny)
                  .Select(group => new
                  {
                      Date = group.Key,  // Data
                      MaxWeight = group.Max(x => x.Weight)  // Najwyższa waga danego dnia
                  })
                  .OrderBy(x => x.Date)  // Sortowanie po dacie
                  .ToList();

            int widthPerPoint = 80; // np. 80 pikseli na punkt
            int chartWidth = Math.Max(dbOrderBy.Count * widthPerPoint, (int)this.Width); // nie mniejszy niż ekran
            chartView.WidthRequest = chartWidth;

            double firstweight = dbOrderBy.First().MaxWeight;
            double lastweight = dbOrderBy.Last().MaxWeight;
            DateTime firstDay = dbOrderBy.First().Date;
            DateTime lastDay = dbOrderBy.Last().Date;
            LabelPart.Text = _selectPart;
            var minValue = dbOrderBy.Min(x => x.MaxWeight);
            var maxValue = dbOrderBy.Max(x => x.MaxWeight);

            // Dodaj mały margines 10% po bokach
            var range = maxValue - minValue;

            // Jeśli zakres jest za mały (np. 5 kg), ustaw minimalny rozstaw, żeby wykres nie był „płaski”
            if (range < 10) range = 10;

            var yMax = maxValue + range * 0.3f;       // +10% od góry
            var yMin = Math.Max(0, minValue - range * 0.1f); // -10% od dołu
            ProgressLabel.Text = (lastweight - firstweight).ToString();
            ProgressTime.Text = dbOrderBy.Count().ToString();
            var chartEntries = dbOrderBy.Select(entry => new Microcharts.ChartEntry((float?)(entry.MaxWeight))
            {
                Label = entry.Date.ToString("dd.MM"),
                ValueLabel = (entry.MaxWeight).ToString("0.0"),
                Color = SkiaSharp.SKColor.Parse("#000000"), // Kolor linii
                                                            //TextColor = SkiaSharp.SKColor.Parse("#27AE60"),

                ValueLabelColor = SkiaSharp.SKColor.Parse("#000000")

            }).ToList();
            var lineChart = new Microcharts.LineChart
            {
                Entries = chartEntries,
                LineMode = LineMode.Straight, // Prosta linia
                LineSize = 5, // Grubość linii
                ValueLabelTextSize = 30,
                EnableYFadeOutGradient = false,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                IsAnimated = false,
                MaxValue = yMax,
                MinValue = yMin,
                BackgroundColor = SKColors.Transparent, // Przezroczyste tło
                PointMode = PointMode.Circle, // Brak punktów na wykresie (opcjonalnie)
                LabelTextSize = 30,
                ValueLabelOption = ValueLabelOption.TopOfElement,
                LabelColor = SKColor.Parse("#000000"), // Białe etykiety
                
            };

            chartView.Chart = lineChart;
        }
        private void PartSelected(object sender, EventArgs e)
        {
            if (namePicker.SelectedIndex != -1)
            {
                string selectedPart = namePicker.SelectedItem.ToString();
                _ = LoadChartv2(selectedPart); // Załaduj dane dla wybranej osoby
            }
        }



        private async void Button_Clicked_Cardio(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(StatisticCardioPage));
        }



        //private async void LoadChart()
        //{
        //    var data = await _database.GetInvoiceAsync();
        //    var weeklyData = data
        //        .GroupBy(d => CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(d.DateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
        //        .Select(g => new
        //        {
        //            WeekNumber = g.Key,
        //            LastWeight = g.OrderByDescending(x => x.DateTime).First().Weight // Ostatnia waga w danym tygodniu
        //        })
        //        .OrderBy(d => d.WeekNumber) // Sortowanie według tygodnia
        //        .ToList();

        //    // Tworzenie punktów dla wykresu
        //    var entries = weeklyData.Select(d => new ChartEntry((float)d.LastWeight)
        //    {
        //        Label = $"Tydz {d.WeekNumber}",
        //        ValueLabel = d.LastWeight.ToString("F1"), // Zaokrąglona wartość wagi
        //        Color = SKColor.Parse("#3498db") // Niebieski kolor linii
        //    }).ToList();

        //    // Tworzenie wykresu liniowego
        //    chartView.Chart = new LineChart
        //    {
        //        //Entries = entries,
        //        //LineMode = LineMode.Straight, // Prosta linia
        //        //LineSize = 1,
        //        //PointSize = 10,
        //        //BackgroundColor = SKColors.Transparent
        //        Entries = entries,
        //        LineMode = LineMode.Straight, // Prosta linia
        //        LineSize = 3, // Cieńsza linia
        //        PointMode = PointMode.Circle, // Pokazanie punktów
        //        PointSize = 8, // Mniejsze punkty
        //        BackgroundColor = SKColors.Transparent,
        //        MinValue = entries.Min(e => float.Parse(e.ValueLabel)) - 2, // Zostawienie odstępu na dole
        //        MaxValue = entries.Max(e => float.Parse(e.ValueLabel)) + 2  // Zostawienie odstępu na górze
        //    };
        //}
    }
}