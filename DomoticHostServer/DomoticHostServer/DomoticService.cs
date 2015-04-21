using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DomoticHostServer
{
    // NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di classe "Service1" nel codice e nel file di configurazione contemporaneamente.
    public class DomoticService : IDomoticService
    {

        public void insertValue(string dataType, double value)
        {
            switch (dataType) { 
                case "temperature":
                    DomoticDAOTableAdapters.temperatureTableAdapter adapter =
                        new DomoticDAOTableAdapters.temperatureTableAdapter();
                    adapter.InsertValue(value);
                    break;
                default:
                    throw new NotImplementedException();
                    
            }
        }
    }
}
