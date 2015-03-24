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


    public class PredictionsController : ApiController
    {

        private ObjectCache cache = MemoryCache.Default;
        

        private List<PredictionModelResult> cacheData = null;

        private GETweetMLService service = new GETweetMLService();

        // GET api/Predictions
        [ResponseType(typeof(List<PredictionModelResult>))]
        public async Task<IHttpActionResult> Get()
        {
            Dictionary<DateTime, List<PredictionModelResult>> predictionData = cache["predictionData"] as Dictionary<DateTime, List<PredictionModelResult>>;


            if (predictionData == null)
            {
                // request data using AzureML API
                var predictionResults = await service.GetPredictionResultsAsync();

                predictionData = new Dictionary<DateTime, List<PredictionModelResult>>();
                predictionData.Add(DateTime.Now, predictionResults);

                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddHours(1);
                cache.Set("predictionData", predictionData, policy);
            }

            if (predictionData == null)
            {
                return this.NotFound();
            }

            return this.Ok(predictionData);
        }

        



    }
}
