using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DomoticHostServer
{
    // NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di interfaccia "IService1" nel codice e nel file di configurazione contemporaneamente.
    [ServiceContract]
    public interface IDomoticService
    {
        [OperationContract]
        string insertValue(string dataType, ValueType value);

        [OperationContract]
        ValueType debug();

        [OperationContract]
        ValueType echo(ValueType item);
    }

    // Per aggiungere tipi compositi alle operazioni del servizio utilizzare un contratto di dati come descritto nell'esempio seguente.
    // È possibile aggiungere file XSD nel progetto. Dopo la compilazione del progetto è possibile utilizzare direttamente i tipi di dati definiti qui con lo spazio dei nomi "DomoticHostServer.ContractType".
    [DataContract]
    public class ValueType
    {
        [DataMember]
        public double Value
        {
            get;
            set;
        }

    }
    
}
