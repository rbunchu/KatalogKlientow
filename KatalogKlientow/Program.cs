using KatalogKlientow.Configuration;
using KatalogKlientow.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KatalogKlientow
{
    internal static class Program
    {
        public static IServiceProvider Services { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("pl");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("pl");

            Application.ThreadException += Application_ThreadException;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                Services.GetRequiredService<Infrastructure.ErrorLogger>()
                    .Log(e.Exception);
                e.SetObserved();
            };

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddAppServices();
            Services = serviceCollection.BuildServiceProvider();

            var databaseInitializer = Services.GetRequiredService<Infrastructure.DatabaseInitializer>();

            try
            {
                if (databaseInitializer.Initialize())
                {
                    databaseInitializer.SeedData();
                }
            } catch(Exception ex)
            {
                Services.GetRequiredService<Infrastructure.ErrorLogger>()
                    .Log(ex);
                MessageBox.Show("Wystąpił błąd podczas inicjalizacji bazy danych. Aplikacja zostanie zamknięta.",
                    "Błąd krytyczny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(Services.GetRequiredService<ClientCatalogForm>());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Services.GetRequiredService<Infrastructure.ErrorLogger>()
                .Log(e.ExceptionObject as Exception);
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Services.GetRequiredService<Infrastructure.ErrorLogger>()
                .Log(e.Exception);
        }
    }
}
