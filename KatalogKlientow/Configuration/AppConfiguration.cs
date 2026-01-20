using KatalogKlientow.Configuration;
using System.Configuration;

namespace KatalogKlientow.Models
{
    public class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration()
        {
           ConnectionString = ConfigurationManager.ConnectionStrings["localDb"].ConnectionString;
           ErrorFileName = ConfigurationManager.AppSettings["ErrorFileName"];
        }

        public string ConnectionString { get; }
        public string ErrorFileName { get; }
    }
}
