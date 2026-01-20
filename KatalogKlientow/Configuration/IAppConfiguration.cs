using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KatalogKlientow.Configuration
{
    public interface IAppConfiguration
    {
        string ConnectionString { get; }
        string ErrorFileName { get; }
    }
}
