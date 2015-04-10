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
            var netVoteTrend = repository.GetNetVoteTrendByLocation(id);
            var lastUpdated = repository.GetNetVoteTrendTimestamp();

            if (netVoteTrend.Count() == 0)
            {
                return new NetVoteTrendData() { LastUpdated = lastUpdated, Data = null };
            }


            //var dataGroup1 = tweets.Select(p => p.TweetDate).Distinct();

            //var list = dataGroup1.ToList();


            var dataGroup = from t in netVoteTrend
                            group t by new { t.Date, t.Party } into g
                            orderby g.Key.Date, g.Key.Party
                            select new { date = g.Key.Date, party = g.Key.Party, count = g.Sum(x => x.Count)};


            var dataDict = new Dictionary<string, List<object[]>>();

            foreach (var item in dataGroup)
            {
                
                object[] values = new object[2];
                values[0] = item.date.Value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
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

            var data = new List<NetVoteTrendDataModel>();

            foreach (var item in dataDict)
            {
                var party = PoliticalParty.GetList().Where(p => p.Code.ToLower() == item.Key.ToLower()).FirstOrDefault();
                data.Add(new NetVoteTrendDataModel() { Key = party.Name, Values = item.Value, Color = party.Color, Order = party.Order });
            }
            //data.OrderBy(x=> x.)

            return new Models.NetVoteTrendData() { LastUpdated = lastUpdated, Data = data.OrderBy(d => d.Order).ToList() };
        }




    }
}
