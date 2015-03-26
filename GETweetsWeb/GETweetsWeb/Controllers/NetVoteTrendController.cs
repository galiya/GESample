using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Description;
using GETweetsWeb.Models;
using System.Web.Mvc;
using System.Runtime.Caching;
using System.IO;

namespace GETweetsWeb.Controllers
{


    public class NetVoteTrendController : ApiController
    {
        private Repository repository = new Repository();

        // GET api/NetVoteTrend
        [ResponseType(typeof(Models.NetVoteTrendData))]
        public NetVoteTrendData Get(int? id = null)
        {

            var mapLocationFilter = new MapLocation(id);

            var tweets = repository.GetTweetsByLocation(mapLocationFilter.Id);

            if (tweets.Count == 0)
            {
                return null;
            }

            

            var dataDict = new Dictionary<string, List<object[]>>();

            var dataGroup = from t in tweets group t by new { t.Timestamp, t.party.Name} into g select new { date = g.Key.Timestamp, party = g.Key.Name, count = g.Sum(v => v.SentimentScore) };
            dataGroup.OrderBy(p => p.date);

            List<object[]> timeseries = new List<object[]>(); 


            foreach (var item in dataGroup)
            {
                
                object[] values = new object[2];
                values[0] = item.date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                values[1] = item.count;
                

                if (dataDict.ContainsKey(item.party))
                {
                    dataDict[item.party].Add(values);
                }
                else
                {
                    dataDict.Add(item.party, new List<object[]> { values });
                }
            }

            var data = new List<TweetsByDates>();

            foreach (var item in dataDict)
            {
                var party = PoliticalParty.GetList().Where(p => p.Name.ToLower() == item.Key.ToLower()).FirstOrDefault();
                data.Add(new TweetsByDates() { Key = party.Name, Values = item.Value, Color = party.Color });
            }

            return new Models.NetVoteTrendData() { LastUpdated = DateTime.Now, Data = data };
        }




    }
}
