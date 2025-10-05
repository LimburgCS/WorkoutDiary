
using Microcharts.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using WorkoutDiary.data;
using WorkoutDiary.ViewModels;
using WorkoutDiary.Views;

namespace WorkoutDiary
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder


                .UseMauiApp<App>()
                .UseMicrocharts()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<TodoItemDatabase>();
            builder.Services.AddSingleton<PersonDataBase>();
            builder.Services.AddSingleton<CardioDataBase>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddSingleton<MainPage>(); //unikanie ponownego tworzenia
            builder.Services.AddTransient<AddExerciseViewModel>(); // każada strona ma nowy kontent
            builder.Services.AddTransient<AddCardioViewModel>(); 
            builder.Services.AddTransient<CardioViewModel>();
            builder.Services.AddTransient<AddExercisePage>();
            builder.Services.AddTransient<StatisticCardioPage>();
            builder.Services.AddTransient<AddCardioPage>();
            builder.Services.AddTransient<AddPersonPage>();
            builder.Services.AddTransient<StatisticPage>();
            builder.Services.AddTransient<CardioPage>();
            builder.Services.AddTransient<SettingPage>();
            builder.Services.AddTransient<AppShellViewModel>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
