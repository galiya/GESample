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
            var mapLocationFilter = new MapLocation(id);

            var tweets = repository.GetTweetsByLocation(mapLocationFilter.Id);  //TODO: change

            if (tweets.Count() == 0)
            {
                return new NetVotePercentData() { LastUpdated = DateTime.Now, Data = null };
            }

            var data = new List<TweetsByPartyPercent>();
            var dataGroup = from t in tweets group t by t.PoliticalParty into g orderby g.Key select new { party = g.Key, count = g.Count() };

            var sum = (decimal)dataGroup.Select(r => r.count).Sum();

            var totalsWithPercentage = dataGroup.Select(r => new {
                party = r.party,
                percentage = sum != 0 ? Math.Round((r.count / sum), 4) : 0
            });



            foreach (var item in totalsWithPercentage)
            {
                var party = PoliticalParty.GetList().Where(p => p.Code.ToLower() == item.party.ToLower()).FirstOrDefault();
                data.Add(new TweetsByPartyPercent() { Name = party.Name, Percent = item.percentage });
            }

            return new NetVotePercentData() { LastUpdated = DateTime.Now, Data = data };
        }

        



    }
}
