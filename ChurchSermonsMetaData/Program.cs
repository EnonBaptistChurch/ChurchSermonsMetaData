using AudioService.Interfaces;
using AudioService.Services;
using Microsoft.Extensions.DependencyInjection;
using SermonData.Interfaces;
using SermonData.Services;

namespace ChurchSermonsMetaData
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            // 1. Initialize WinForms configuration first!
            ApplicationConfiguration.Initialize();

            // 2. Configure DI container
            var services = new ServiceCollection();

            // Register services and forms
            // services.AddSingleton<IWeatherService, WeatherService>();
            services.AddTransient<FrmChurchSermonsMetaData>();
            services.AddScoped<IAudioService, AudioService.Services.AudioService>();
            services.AddScoped<IBiblePassageExtractor, BiblePassageExtractor>();
            services.AddScoped<ISermonMetaDataExtractor, SermonMetaDataExtractor>();

            // 3. Build provider
            using var serviceProvider = services.BuildServiceProvider();

            // 4. Resolve the form from DI
            var mainForm = serviceProvider.GetRequiredService<FrmChurchSermonsMetaData>();

            // 5. Run the app
            Application.Run(mainForm);
        }
    }
}