using System;
using System.Collections.Generic;
using System.IO;
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
        WebSiteResponse<double> getTemperatureValues(string period);


        [OperationContract]
        WebSiteResponse<bool> getLightStatus(string option);

        [OperationContract]
        WebSiteResponse<bool> getHeatherStatus(string option);

        [OperationContract]
        WebSiteResponse<double> getLuminosity();

        [OperationContract]
        void changeLightState(Stream input);

        [OperationContract]
        void changeHeatherState(Stream input);

        [OperationContract]
        void changeHeatherAutomatic(Stream input);

        [OperationContract]
        void changeLightAutomatic(Stream input);

        [OperationContract]
        WebSiteResponse<bool> getPresence(string period);




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

    [DataContract]
    public class Record<T> {

        [DataMember(Name="date", IsRequired=false)]
        public string date{get;set;}

        [DataMember(Name="value", IsRequired=true)]
        public T value { get; set; }

        public Record(T v, string d)
        {
            this.value = v;
            this.date = d;
        }
        
    }

    [DataContract]
    public class WebSiteResponse<T> {

        [DataMember(Name = "record")]
        public List<Record<T>> record { get; set; }
        
    }




    
}
