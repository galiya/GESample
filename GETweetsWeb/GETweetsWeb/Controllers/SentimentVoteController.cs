using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using GETweetsWeb.Models;

namespace GETweetsWeb.Controllers
{
    public class SentimentVoteController : ApiController
    {
        private Repository repository = new Repository();

        // GET api/SentimentVote
        [ResponseType(typeof(Models.SentimentVoteData))]
        public SentimentVoteData Get(int? id = null)
        {
            var sentimentVote = repository.GetSentimentVoteByLocation(id);
            var lastUpdated = repository.GetNetVoteTimestamp();

            if (sentimentVote.Count() == 0)
            {
                return new SentimentVoteData() { LastUpdated = lastUpdated, Data = null };
            }

            var data = new List<SentimentVoteDataModel>();
            var dataGroup = from t in sentimentVote group t by t.Party into g orderby g.Key
                            select new { party = g.Key, posCount = g.Sum(x => x.PositiveCount), negCount = g.Sum(x => x.NegativeCount) };
            

            /*----*/
            var dataDict = new Dictionary<string, List<object[]>>();

            var nameSeries1 = "Negative";
            var nameSeries2= "Positive";

            var colorSeries1 = "#ff6633";
            var colorSeries2 = "#66ff33";

            foreach (var item in dataGroup)
            {

                var party = PoliticalParty.GetList().Where(p => p.Code.ToLower() == item.party.ToLower()).FirstOrDefault();

                object[] series1 = new object[3];
                object[] series2 = new object[3];

                series1[0] = party.Name;
                series1[1] = item.negCount;
                series1[2] = party.Order;

                series2[0] = party.Name;
                series2[1] = item.posCount;
                series2[2] = party.Order;

                if (dataDict.ContainsKey(nameSeries1))
                {
                    dataDict[nameSeries1].Add(series1);
                }
                else
                {
                    dataDict.Add(nameSeries1, new List<object[]> { series1 });
                }

                if (dataDict.ContainsKey(nameSeries2))
                {
                    dataDict[nameSeries2].Add(series2);
                }
                else
                {
                    dataDict.Add(nameSeries2, new List<object[]> { series2 });
                }
            }

            
            foreach (var item in dataDict)
            {
                //    var party = PoliticalParty.GetList().Where(p => p.Code.ToLower() == item.Key.ToLower()).FirstOrDefault();
                var color = colorSeries1;
                if (item.Key == nameSeries2)
                {
                    color = colorSeries2;
                }
                data.Add(new SentimentVoteDataModel() { Key = item.Key, Values = item.Value.OrderBy(x => x[2]).ToList(), Color = color});
            }

            return new SentimentVoteData() { LastUpdated = lastUpdated, Data = data };
        }
    }
}
