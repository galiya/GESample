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


    public class NetVotePercentController : ApiController
    {

        private Repository repository = new Repository();

        // GET api/NetVotePercent
        [ResponseType(typeof(NetVotePercentData))]
        public NetVotePercentData Get(int? id = null)
        {
            var netVote = repository.GetNetVoteByLocation(id);
            var lastUpdated = repository.GetNetVoteTimestamp();

            if (netVote.Count() == 0)
            {
                return new NetVotePercentData() { LastUpdated = lastUpdated, Data = null };
            }

            var data = new List<NetVotePercentModel>();
            var dataGroup = from t in netVote group t by t.Party into g orderby g.Key select new { party = g.Key, count = g.Sum(x => x.PositiveCount) };

            var sum = (decimal)dataGroup.Select(r => r.count).Sum();

            var totalsWithPercentage = dataGroup.Select(r => new {
                party = r.party,
                percentage = sum != 0 ? Math.Round((r.count.Value / sum), 4) : 0
            });

            foreach (var item in totalsWithPercentage)
            {
                var party = PoliticalParty.GetList().Where(p => p.Code.ToLower() == item.party.ToLower()).FirstOrDefault();
                data.Add(new NetVotePercentModel() { Name = party.Name, Percent = item.percentage, Color = party.Color, Order = party.Order });
            }

            return new NetVotePercentData() { LastUpdated = lastUpdated, Data = data.OrderBy(d => d.Order).ToList() };
        }

        



    }
}
