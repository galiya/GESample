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


    public class NetVoteController : ApiController
    {
        private Repository repository = new Repository();

        // GET api/NetVote
        [ResponseType(typeof(NetVoteData))]
        public NetVoteData Get(int? id = null)
        {

            var mapLocationFilter = new MapLocation(id);

            var tweets = repository.GetTweetsByLocation(mapLocationFilter.Id);

            if (tweets.Count == 0)
            {
                return null;
            }

            var data = new List<TweetsByPartyCount>();
            var dataGroup = from t in tweets group t by t.party.Name into g select new { party = g.Key, count = g.Count()};
            foreach (var item in dataGroup)
            {
                data.Add(new TweetsByPartyCount() {Name = item.party, Count = item.count });
            }

            return new NetVoteData() { LastUpdated = DateTime.Now, Data = data };
        }

        



    }
}
