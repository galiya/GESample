using System;
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


        public GETweetsWebDB context
        {
            get { return this.db; }
        }

        private const string cacheConversationsUK = "Repository:ConversationsUK";
        public IEnumerable<DummyTweet> GetTweets()
        {
            var tweets = (List<DummyTweet>)HttpContext.Current.Cache.Get(cacheConversationsUK);
            if (tweets == null)
            {
                try
                {
                    using (var db = new GETweetsWebDB(GetConnectionString()))
                    {
                        tweets = db.DummyTweets.Include("GeoShape").ToList<DummyTweet>();
                    }
                }
                catch (Exception)
                {
                    return null;
                }

                HttpContext.Current.Cache.Insert(cacheConversationsUK, tweets);
            }
            return tweets;
        }

        public IEnumerable<DummyTweet> GetTweetsByLocation(int? locationId)
        {
            try
            {
                if (locationId.HasValue && locationId > 0)
                {
                    return this.GetTweets().Where(f => f.GeoShape.GeoShapeId == locationId);
                }
                else
                    return this.GetTweets();
            }
            catch (Exception)
            {

                return null;
            }
        }

        public List<GeoShape> GetMapAreasList()
        {
            List<GeoShape> counties = new List<GeoShape>();
            counties.Add(new GeoShape() { Name = "Great Britain", PolygonId = 0, GeoShapeId = 0 });

            try
            {
                using (var db = new GETweetsWebDB(GetConnectionString()))
                {
                    counties.AddRange(db.GeoShapes.ToList());
                }
                return counties;

            }
            catch
            {
                return counties;
            }

        }

        public void Dispose()
        {
            //db.Dispose();
        }

    }

}