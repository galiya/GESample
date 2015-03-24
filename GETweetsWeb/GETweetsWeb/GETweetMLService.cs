using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GETweetsWeb.Models;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GETweetsWeb
{
    public class GETweetMLService
    {

        private List<PredictionModelResult> ParsePartyPredictionResultsResponse(string response)
        {
           // var json = JsonConvert.DeserializeObject<dynamic>(response);
           //string json = "{\"Results\":{\"output1\":{\"type\":\"table\",\"value\":{\"ColumnNames\":[\"Id\",\"Name\",\"Value\"],\"ColumnTypes\":[\"String\",\"String\",\"Int32\"],\"Values\":[[\"value\",\"value\",\"0\"],[\"value\",\"value\",\"0\"]]}}}}";

            List<PredictionModelResult> resultList = new List<PredictionModelResult>();
            var item = (JObject.Parse(response)["Results"]["GEresults"]["value"]["Values"]).Children().ToArray();

            resultList.AddRange(item.Select(array => new PredictionModelResult {LongName = (string)array[0], ShortName = ((string)array[1]).ToUpper(), Value = (int)array[2] }).ToList());
           
            return resultList;

        }


        public async Task<List<PredictionModelResult>> GetPredictionResultsAsync(bool debug = false)
        {

            if (debug)
            {
                var debugResponse = "{'Results':{'GEresults':{'type':'table','value':{'ColumnNames':['longName','shortName','results.agg'],'ColumnTypes':['String','String','Double'],'Values':[['conservative','con','36'],['green','grn','15'],['labour','lab','15'],['liberal.democrats','lib','6'],['SNP','snp','43'],['UKIP','ukp','31']]}}}}";
                return ParsePartyPredictionResultsResponse(debugResponse);
            }

            using (HttpClient httpClient = new HttpClient())
            {
                var request = new
                {
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };

                const string apiKey = "lCfOwr41SHTr5o0teEoLr6ab685Xrpp7504TDl7vh9Eri0/qdNBGKjXolJ1i0EzgbnAFxOrYrJ+xEVWaZReEhA=="; // Replace this with the API key for the web service
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                httpClient.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/4beff52806a84eab94d2e7d0f9cb50a5/services/106c2037b1a24696aa6240073c8a444c/execute?api-version=2.0&details=true");

                HttpResponseMessage response = await httpClient.PostAsJsonAsync("", request);

                if (response.IsSuccessStatusCode)
                {

                    var partyPredictionResult = await response.Content.ReadAsStringAsync();
                    //return JsonConvert.DeserializeObject<List<PredictionResult>>(result);
                    return ParsePartyPredictionResultsResponse(partyPredictionResult);
                }
                else
                {
                    var value = response.StatusCode;
                }
                return null;


            }
        }


        private class StringTable
        {
            public string[] ColumnNames { get; set; }
            public string[,] Values { get; set; }
        }




    }
}