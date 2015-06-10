using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

namespace DomoticHostServer
{
    
    // NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di classe "Service1" nel codice e nel file di configurazione contemporaneamente.
    
    public class DomoticService : IDomoticService
    {

        static string endpoint_board = "http://192.168.1.202:80/";
        
        private static Boolean AutomaticLightsState = true;
        private static Boolean AutomaticHeatherState = true;
        
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
                case "luminosity":
                    try
                    {
                        DomoticDAOTableAdapters.luminosityTableAdapter adapter =
                            new DomoticDAOTableAdapters.luminosityTableAdapter();
                        adapter.InsertValue(value.Value);
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                    break;
                case "light":
                    try
                    {
                        DomoticDAOTableAdapters.lightTableAdapter adapter =
                            new DomoticDAOTableAdapters.lightTableAdapter();
                        adapter.InsertValue(value.Value == 0 ? false : true);
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                    break;
                case "presence":
                    try
                    {
                        DomoticDAOTableAdapters.presenceTableAdapter adapter =
                            new DomoticDAOTableAdapters.presenceTableAdapter();
                        adapter.InsertValue(value.Value == 0 ? false : true);
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                    break;
                case "heather":
                    try
                    {
                        DomoticDAOTableAdapters.heatherTableAdapter adapter =
                            new DomoticDAOTableAdapters.heatherTableAdapter();
                        adapter.InsertValue(value.Value == 0 ? false : true);
                    }
                    catch (Exception e) {
                        return e.Message;
                    }
                    break;
                case "automatic_light":
                    try
                    {
                        AutomaticLightsState = value.Value == 0 ? false : true;
                    }
                    catch (Exception e) {
                        return e.Message;
                    }
                    break;
                case "automatic_heather":
                    try
                    {
                        AutomaticHeatherState = value.Value == 0 ? false : true;
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
            UriTemplate = "temperature?period={period}")]
        public WebSiteResponse<double> getTemperatureValues(string period) {
            WebSiteResponse<double> response = new WebSiteResponse<double>();
            switch (period) { 
                case "LAST":
                    DomoticDAOTableAdapters.temperatureTableAdapter adapter =
                            new DomoticDAOTableAdapters.temperatureTableAdapter();
            
                    DomoticDAO.temperatureDataTable last=adapter.GetLastDatum();
                    List<Record<double>> res = new List<Record<double>>();
                    if (last.Count > 0)
                    {
                        string date =last.ElementAt(0).time.ToString("o");
                        res.Add(new Record<double>(last.ElementAt(0).value, date.Substring(0, date.Length-4) + "Z"));
                    }
                    response.record = res;
                    break;
                case "TWELVE_HOUR":
                    DomoticDAOTableAdapters.TemperatureByHoursTableAdapter qAdapter =
                            new DomoticDAOTableAdapters.TemperatureByHoursTableAdapter();
                    DomoticDAO.TemperatureByHoursDataTable means12=qAdapter.GetDataOverLast(12);
                    List<Record<double>> res12 = new List<Record<double>>();
                    for (int i = 0; i < means12.Count; i++) { 
                       res12.Add(new Record<double>(means12.ElementAt(i).mean,
                           means12.ElementAt(i).anno + "-" + means12.ElementAt(i).mese.ToString("D2") + "-"
                           + means12.ElementAt(i).giorno.ToString("D2") + "T" + means12.ElementAt(i).hour.ToString("D2")
                           +":00:00.000Z"));
                    }
                    response.record = res12;
                    break;
                case "TWENTYFOUR_HOUR":
                    DomoticDAOTableAdapters.TemperatureByHoursTableAdapter tAdapter =
                            new DomoticDAOTableAdapters.TemperatureByHoursTableAdapter();
                    DomoticDAO.TemperatureByHoursDataTable means24=tAdapter.GetDataOverLast(24);
                    List<Record<double>> res24 = new List<Record<double>>();
                    for (int i = 0; i < means24.Count; i++) { 
                       res24.Add(new Record<double>(means24.ElementAt(i).mean,
                           means24.ElementAt(i).anno + "-" + means24.ElementAt(i).mese.ToString("D2") + "-"
                           + means24.ElementAt(i).giorno.ToString("D2") + "T" + means24.ElementAt(i).hour.ToString("D2")
                           +":00:00.000Z"));
                    }
                    response.record = res24;
                    break;
                case "LAST_WEEK":
                    DomoticDAOTableAdapters.TemperatureByDaysTableAdapter wAdapter =
                            new DomoticDAOTableAdapters.TemperatureByDaysTableAdapter();
                    DomoticDAO.TemperatureByDaysDataTable means_week=wAdapter.GetData(7);
                    List<Record<double>> res_week = new List<Record<double>>();
                    for (int i = 0; i < means_week.Count; i++)
                    {
                        res_week.Add(new Record<double>(means_week.ElementAt(i).mean,
                           means_week.ElementAt(i).anno + "-" + means_week.ElementAt(i).mese.ToString("D2") + "-"
                           + means_week.ElementAt(i).giorno.ToString("D2")
                           +"T00:00:00.000Z"));
                    }
                    response.record = res_week;
                    break;
                default:
                    throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);
            
            }
            return response;
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "light/{option}")]
        public WebSiteResponse<bool> getLightStatus(string option) {
            WebSiteResponse<bool> response = new WebSiteResponse<bool>();
            List<Record<bool>> result = new List<Record<bool>>();
            switch (option) {
                case "status":
                    DomoticDAOTableAdapters.lightTableAdapter adapter =
                        new DomoticDAOTableAdapters.lightTableAdapter();
                    DomoticDAO.lightDataTable last =  adapter.GetLastDatum();
                    if (last.Count > 0) {
                        string date = last.ElementAt(0).time.ToString("o");
                        result.Add(new Record<bool>(last.ElementAt(0).turned_on, date.Substring(0, date.Length - 4) + "Z"));

                    }else{
                        result.Add(new Record<Boolean>(false, null));
                    }
                    break;
                case "automatic":
                    result.Add(new Record<Boolean>(AutomaticLightsState, null));
                    break;
                default:
                    throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);
            
                
            }
            response.record = result;
            return response;
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "heather/{option}")]
        public WebSiteResponse<bool> getHeatherStatus(string option)
        {
            WebSiteResponse<bool> response = new WebSiteResponse<bool>();
            List<Record<bool>> result = new List<Record<bool>>();
            switch (option)
            {
                case "status":
                    DomoticDAOTableAdapters.heatherTableAdapter adapter =
                        new DomoticDAOTableAdapters.heatherTableAdapter();
                    DomoticDAO.heatherDataTable last = adapter.GetLastDatum();
                    if (last.Count > 0)
                    {
                        string date = last.ElementAt(0).time.ToString("o");
                        result.Add(new Record<bool>(last.ElementAt(0).turned_on, date.Substring(0, date.Length - 4) + "Z"));

                    }
                    else
                    {
                        result.Add(new Record<Boolean>(false, null));
                    }
                    break;
                case "automatic":
                    result.Add(new Record<Boolean>(AutomaticHeatherState, null));
                    break;
                default:
                    throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);


            }
            response.record = result;
            return response;
        }


        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "luminosity")]
        public WebSiteResponse<double> getLuminosity()
        {
            DomoticDAOTableAdapters.luminosityTableAdapter adapter = new DomoticDAOTableAdapters.luminosityTableAdapter();
            DomoticDAO.luminosityDataTable e = adapter.GetLastDatum();
            WebSiteResponse<double> resp = new WebSiteResponse<double>();
            if (e.Count > 0) {
                string date = e.ElementAt(0).time.ToString("o");
                resp.record.Add(new Record<double>(e.ElementAt(0).value,
                    date.Substring(0, date.Length - 4) + "Z"));
            }
            return resp;
        }

        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "light/status")]
        public WebSiteResponse<bool> changeLightState(Stream input)
        {
            NameValueCollection nvc = parseFormString(input);
            switch (nvc["action"]) { 
                case "ON":
                    if (sendRequestToBoard("PUT", "lights/on", null) != "\"OK\"")
                        throw new WebFaultException(System.Net.HttpStatusCode.ServiceUnavailable);
                    break;
                case "OFF":
                    if (sendRequestToBoard("PUT", "lights/off", null) != "\"OK\"")
                        throw new WebFaultException(System.Net.HttpStatusCode.ServiceUnavailable);
                    break;
                default:
                    throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);
            
            }
            WebSiteResponse<bool> resp = new WebSiteResponse<bool>();
            List<Record<bool>> result = new List<Record<bool>>();
            result.Add(new Record<bool>(true, null));
            resp.record = result;
            return resp;
        }

        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "heather/status")]
        public WebSiteResponse<bool> changeHeatherState(Stream input)
        {
            NameValueCollection nvc = parseFormString(input);
            switch (nvc["action"])
            {
                case "ON":
                    if (sendRequestToBoard("PUT", "heather/on", null) != "\"OK\"")
                        throw new WebFaultException(System.Net.HttpStatusCode.ServiceUnavailable);
                    break;
                case "OFF":
                    if (sendRequestToBoard("PUT", "heather/off", null) != "\"OK\"")
                        throw new WebFaultException(System.Net.HttpStatusCode.ServiceUnavailable);
                    break;
                default:
                    throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);

            }
            WebSiteResponse<bool> resp = new WebSiteResponse<bool>();
            List<Record<bool>> result = new List<Record<bool>>();
            result.Add(new Record<bool>(true, null));
            resp.record = result;
            return resp;

        }


        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "light/automatic")]
        public WebSiteResponse<bool> changeLightAutomatic(Stream input)
        {
            NameValueCollection nvc = parseFormString(input);
            switch (nvc["action"])
            {
                case "ON":
                    if (sendRequestToBoard("PUT", "automatic_light/on", null) != "\"OK\"")
                        throw new WebFaultException(System.Net.HttpStatusCode.ServiceUnavailable);
                    break;
                case "OFF":
                    if (sendRequestToBoard("PUT", "automatic_light/off", null) != "\"OK\"")
                        throw new WebFaultException(System.Net.HttpStatusCode.ServiceUnavailable);
                    break;
                default:
                    throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);

            }
            WebSiteResponse<bool> resp = new WebSiteResponse<bool>();
            List<Record<bool>> result = new List<Record<bool>>();
            result.Add(new Record<bool>(true, null));
            resp.record = result;
            return resp;

        }

        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest,
            UriTemplate = "heather/automatic")]
        public WebSiteResponse<bool> changeHeatherAutomatic(Stream input)
        {
            NameValueCollection nvc = parseFormString(input);
            switch (nvc["action"])
            {
                case "ON":
                    if (sendRequestToBoard("PUT", "automatic_heather/on", null) != "\"OK\"")
                        throw new WebFaultException(System.Net.HttpStatusCode.ServiceUnavailable);
                    break;
                case "OFF":
                    if (sendRequestToBoard("PUT", "automatic_heather/off", null) != "\"OK\"")
                        throw new WebFaultException(System.Net.HttpStatusCode.ServiceUnavailable);
                    break;
                default:
                    throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);

            }
            WebSiteResponse<bool> resp = new WebSiteResponse<bool>();
            List<Record<bool>> result = new List<Record<bool>>();
            result.Add(new Record<bool>(true, null));
            resp.record = result;
            return resp;

        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "presence?period={period}")]
        public WebSiteResponse<bool> getPresence(string period)
        {
            WebSiteResponse<bool> resp = new WebSiteResponse<bool>();
            DomoticDAOTableAdapters.presenceTableAdapter adapter = new DomoticDAOTableAdapters.presenceTableAdapter();

            switch(period){
                case "LAST":
                    DomoticDAO.presenceDataTable e = adapter.GetLastDatum();
                
                    if (e.Count > 0)
                    {
                        string date = e.ElementAt(0).time.ToString("o");
                        resp.record.Add(new Record<bool>(e.ElementAt(0).presence,
                            date.Substring(0, date.Length - 4) + "Z"));
                    }
                break;
                case "TWELVE_HOUR":
                    DomoticDAO.presenceDataTable rows = adapter.GetDataInLastHours(12);

                    for (int i = 0; i < rows.Count;i++ )
                    {

                        string date = rows.ElementAt(i).time.ToString("o");
                        resp.record.Add(new Record<bool>(rows.ElementAt(i).presence,
                            date.Substring(0, date.Length - 4) + "Z"));
                    }
                    break;
                case "TWENTYFOUR_HOUR":
                    DomoticDAO.presenceDataTable rows1 = adapter.GetDataInLastHours(24);

                    for (int i = 0; i < rows1.Count; i++)
                    {

                        string date = rows1.ElementAt(i).time.ToString("o");
                        resp.record.Add(new Record<bool>(rows1.ElementAt(i).presence,
                            date.Substring(0, date.Length - 4) + "Z"));
                    }
                    break;
                case "LAST_WEEK":
                    DomoticDAO.presenceDataTable rows2 = adapter.GetDataInLastHours(24*7);

                    for (int i = 0; i < rows2.Count; i++)
                    {

                        string date = rows2.ElementAt(i).time.ToString("o");
                        resp.record.Add(new Record<bool>(rows2.ElementAt(i).presence,
                            date.Substring(0, date.Length - 4) + "Z"));
                    }
                    break;

                default:
                    throw new WebFaultException(System.Net.HttpStatusCode.BadRequest);


            }
            return resp;
        }






        private string sendRequestToBoard(string method, string uri, Dictionary<string, string> parameters)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint_board+uri);

            string postData = "";

            if (parameters != null && parameters.Count > 0) { 
                
                foreach(string k in parameters.Keys){
                    postData += HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(parameters[k])+"&";
                }
                postData = postData.Substring(0, postData.Length - 1);
            }
            byte[] data = Encoding.ASCII.GetBytes(postData);

            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        private NameValueCollection parseFormString(Stream input)
        {
            var streamReader = new StreamReader(input);
            string streamString = streamReader.ReadToEnd();
            streamReader.Close();

            return HttpUtility.ParseQueryString(streamString);
        }

    }
}
