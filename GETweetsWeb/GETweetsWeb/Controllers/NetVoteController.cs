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
            var netVote = repository.GetNetVoteByLocation(id);
            var lastUpdated = repository.GetNetVoteTimestamp();

            if (netVote.Count() == 0)
            {
                return new NetVoteData() { LastUpdated = lastUpdated, Data = null };
            }

            var data = new List<NetVoteModel>();
            var dataGroup = from t in netVote group t by t.Party into g orderby g.Key select new { party = g.Key, count = g.Sum(x => x.PositiveCount)};
            foreach (var item in dataGroup)
            {
                var party = PoliticalParty.GetList().Where(p => p.Code.ToLower() == item.party.ToLower()).FirstOrDefault();
                data.Add(new NetVoteModel() {Name = party.Name, Count = item.count.Value, Color = party.Color, Order = party.Order });
            }
            return new NetVoteData() { LastUpdated = lastUpdated, Data = data.OrderBy(d => d.Order).ToList() };
        }

        



    }
}
