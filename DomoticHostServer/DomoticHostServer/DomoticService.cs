using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DomoticHostServer
{
    // NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di classe "Service1" nel codice e nel file di configurazione contemporaneamente.
    
    public class DomoticService : IDomoticService
    {
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "insert/{dataType}")]
        public string insertValue(string dataType, ValueType value)
        {
            switch (dataType) { 
                case "temperature":
                    try
                    {
                        DomoticDAOTableAdapters.temperatureTableAdapter adapter =
                            new DomoticDAOTableAdapters.temperatureTableAdapter();
                        adapter.InsertValue(value.Value);
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                    break;
                default:
                    return "KO";
                    
            }
            return "OK";
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "debug")]
        public ValueType debug()
        {
            return new ValueType() { Value=10.0 };
            
        }


        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "echo")]
        public ValueType echo(ValueType item)
        {
            return item;

        }


    }
}
