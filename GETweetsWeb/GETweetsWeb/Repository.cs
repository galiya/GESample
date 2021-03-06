﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using GETweetsWeb;
using GETweetsWeb.Models;

namespace GETweetsWeb
{
    public sealed class Repository : IDisposable
    {
        private GETweetsWebDB db = new GETweetsWebDB();

        public static string GetConnectionString()
        {

            string connectionString = new System.Data.EntityClient.EntityConnectionStringBuilder
            {
                Metadata = "res://*",
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = new System.Data.SqlClient.SqlConnectionStringBuilder
                {
                    InitialCatalog = System.Configuration.ConfigurationManager.AppSettings["DBName"],
                    DataSource = System.Configuration.ConfigurationManager.AppSettings["DBDataSource"],
                    IntegratedSecurity = false,
                    UserID = System.Configuration.ConfigurationManager.AppSettings["DBUserId"],                 // User ID such as "sa"
                    Password = System.Configuration.ConfigurationManager.AppSettings["DBPwd"],               // hide the password
                }.ConnectionString
            }.ConnectionString;

            return connectionString;
        }

        private static int GetCacheRefreshIntervalMin()
        {
            var interval = 12 * 60;
            var cacheRefreshInterval = System.Configuration.ConfigurationManager.AppSettings["RefreshDataIntervalMin"];
            if (!String.IsNullOrWhiteSpace(cacheRefreshInterval))
            {
                if (int.TryParse(cacheRefreshInterval, out interval))
                {
                    return interval;
                }
            }
            return interval;
        }

        public GETweetsWebDB context
        {
            get { return this.db; }
        }

        
        private const string cacheNetVoteUK = "Repository:NetVote";
        private const string cacheNetVoteUKTimestamp = "Repository: NetVoteTimestamp";

        public DateTime GetNetVoteTimestamp(){

            var timestampCache = HttpContext.Current.Cache.Get(cacheNetVoteUKTimestamp);
            DateTime timestamp;
              
            if (timestampCache == null)
            {
                timestamp = DateTime.Now;
                HttpContext.Current.Cache.Insert(cacheNetVoteUKTimestamp, timestamp, null, timestamp.AddMinutes(GetCacheRefreshIntervalMin()), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            else
            {
                timestamp = (DateTime)timestampCache;
            }
            return timestamp;
        }

        public List<VoteSentiment> GetNetVote()
        {
            var netVote = (List<VoteSentiment>)HttpContext.Current.Cache.Get(cacheNetVoteUK);
            if (netVote == null)
            {
                try
                {
                    using (var db = new GETweetsWebDB(GetConnectionString()))
                    {
                        netVote = db.VoteSentiments.Include("GeoShape").ToList<VoteSentiment>();
                        var timestamp = DateTime.Now;
                        HttpContext.Current.Cache.Insert(cacheNetVoteUK, netVote, null, timestamp.AddMinutes(GetCacheRefreshIntervalMin()), System.Web.Caching.Cache.NoSlidingExpiration);
                        HttpContext.Current.Cache.Insert(cacheNetVoteUKTimestamp, timestamp, null, timestamp.AddMinutes(GetCacheRefreshIntervalMin()), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return netVote;
        }

        public IEnumerable<VoteSentiment> GetNetVoteByLocation(int? locationId)
        {
            return GetSentimentVoteByLocation(locationId);
        }

        public IEnumerable<VoteSentiment> GetSentimentVoteByLocation(int? locationId)
        {
            try
            {
                var netVote = this.GetNetVote();
                if (locationId.HasValue && locationId > 0)
                {
                    return netVote.Where(f => f.GeoShapeId == locationId);
                    //return netVote.Where(f => f.GeoShape.GeoShapeId == locationId);
                }
                else
                    return netVote;
            }
            catch (Exception)
            {

                return null;
            }
        }

        private const string cacheNetVoteTrendUK = "Repository:NetVoteTrend";
        private const string cacheNetVoteTrendUKTimestamp = "Repository:NetVoteTrendTimestamp";
        public DateTime GetNetVoteTrendTimestamp()
        {

            var timestampCache = HttpContext.Current.Cache.Get(cacheNetVoteTrendUKTimestamp);
            DateTime timestamp;
              
            if (timestampCache == null)
            {
                timestamp = DateTime.Now;
                HttpContext.Current.Cache.Insert(cacheNetVoteTrendUKTimestamp, timestamp, null, timestamp.AddMinutes(GetCacheRefreshIntervalMin()), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            else
            {
                timestamp = (DateTime)timestampCache;
            }
            return timestamp;
        }

        public IEnumerable<VoteTrend> GetNetVoteTrend()
        {
            var netVoteTrend = (List<VoteTrend>)HttpContext.Current.Cache.Get(cacheNetVoteTrendUK);
            if (netVoteTrend == null)
            {
                try
                {
                    using (var db = new GETweetsWebDB(GetConnectionString()))
                    {
                        netVoteTrend = db.VoteTrends.Include("GeoShape").ToList<VoteTrend>();
                        var timestamp = DateTime.Now;

                        HttpContext.Current.Cache.Insert(cacheNetVoteTrendUK, netVoteTrend, null, timestamp.AddMinutes(GetCacheRefreshIntervalMin()), System.Web.Caching.Cache.NoSlidingExpiration);
                        HttpContext.Current.Cache.Insert(cacheNetVoteTrendUKTimestamp, timestamp, null, timestamp.AddMinutes(GetCacheRefreshIntervalMin()), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return netVoteTrend;
        }

        public IEnumerable<VoteTrend> GetNetVoteTrendByLocation(int? locationId)
        {
            try
            {
                if (locationId.HasValue && locationId > 0)
                {
                    //return this.GetNetVoteTrend().Where(f => f.GeoShape.GeoShapeId == locationId);
                    return this.GetNetVoteTrend().Where(f => f.GeoShapeId == locationId);
                }
                else
                    return this.GetNetVoteTrend();
            }
            catch (Exception)
            {

                return null;
            }
        }


        private const string cacheGeoShapes = "Repository:GeoShapes";

        public List<GeoShape> GetGeoShapes()
        {
            var result = (List<GeoShape>)HttpContext.Current.Cache.Get(cacheGeoShapes);
            if (result == null)
            {
                try
                {
                    using (var db = new GETweetsWebDB(GetConnectionString()))
                    {
                        result = db.GeoShapes.ToList();
                        var timestamp = DateTime.Now;

                        HttpContext.Current.Cache.Insert(cacheGeoShapes, result, null, timestamp.AddDays(30), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                }
                catch
                {
                    return null;
                }
            }
            return result;
        }


        public List<GeoShape> GetMapAreasList()
        {
            List<GeoShape> counties = new List<GeoShape>();
            counties.Add(new GeoShape() { Name = "Great Britain", GeoShapeId = 0 });

            try
            {
                using (var db = new GETweetsWebDB(GetConnectionString()))
                {
                    counties.AddRange(GetGeoShapes());
                }
                return counties;

            }
            catch
            {
                return counties;
            }

        }

        private const string cacheMapVote = "Repository:MapVote";
        

        public Dictionary<int, string> GetMapVotePerParty()
        {

            var mapVote = (List<VoteMap>)HttpContext.Current.Cache.Get(cacheMapVote);
            if (mapVote == null)
            {
                try
                {
                    using (var db = new GETweetsWebDB(GetConnectionString()))
                    {
                        mapVote = db.VoteMaps.Where(x => x.WinningDiff > 0).ToList();
                        var timestamp = DateTime.Now;

                        HttpContext.Current.Cache.Insert(cacheMapVote, mapVote, null, timestamp.AddMinutes(GetCacheRefreshIntervalMin()), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                }
                catch
                {
                    mapVote = null;
                }
            }


            var result = new Dictionary<int, string>();
            try
            {
                using (var db = new GETweetsWebDB(GetConnectionString()))
                {
                    var party = PoliticalParty.GetList();

                    foreach (var item in mapVote)
                    {
                        var color = "";

                        var itemParty = party.Where(p => p.Code.ToLower() == item.Party.ToLower()).FirstOrDefault();
                        if (itemParty != null)
                        {
                            if (item.WinningDiff.Value < 0.26m)
                            {
                                color = itemParty.Shades[3];
                            }
                            else if (item.WinningDiff.Value < 0.51m)
                            {
                                color = itemParty.Shades[2];
                            }
                            else if (item.WinningDiff.Value < 0.76m)
                            {
                                color = itemParty.Shades[1];
                            }
                            else if (item.WinningDiff.Value <= 1.0m)
                            {
                                color = itemParty.Shades[0];
                            }
                        }
                        result.Add(item.GeoShapeId.Value, color);
                    }
                }
                return result;

            }
            catch
            {
                return result;
            }

            
        }

        public void Dispose()
        {
            //db.Dispose();
        }

    }

}