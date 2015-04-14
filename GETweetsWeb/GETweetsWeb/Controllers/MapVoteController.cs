using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using GETweetsWeb.Models;


namespace GETweetsWeb.Controllers
{
    public class MapVoteController : ApiController
    {
        private Repository repository = new Repository();

        // GET api/NetVote
        [ResponseType(typeof(MapVoteData))]
        public MapVoteData Get()
        {
            var result = new MapVoteData();
            result.Data = new Dictionary<int, string>();

            var parties = PoliticalParty.GetList();
            foreach (var item in repository.GetMapVotePerParty())
            {
                var resultShade = "";
                if (!String.IsNullOrWhiteSpace(item.Value))
                {
                    resultShade = item.Value;
                        
                }
                result.Data.Add(item.Key, resultShade);   // GeoShapeId, #Hex color for a shade
            }
            return result;
        }
    }
}
