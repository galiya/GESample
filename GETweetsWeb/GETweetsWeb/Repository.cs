using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GETweetsWeb.Models;

namespace GETweetsWeb
{
    public sealed class Repository : IDisposable
    {
        //private GETweetWebEntities db = new GETweetWebEntities();

        //public GETweetWebEntities Context
        //{
        //    get { return this.db; }
        //}

        private const string cacheConversationsUK = "Repository:ConversationsUK";
        public List<Tweet> GetTweets()
        {
            var tweets = (List<Tweet>)HttpContext.Current.Cache.Get(cacheConversationsUK);
            if (tweets == null)
            {


                //using (var db = new GETweetWebEntities())
                //{
                //    db.ContextOptions.LazyLoadingEnabled = false;
                //    tweets = db.TweetsScored.Include("InheritedPermissions")/*.OrderBy(f => f.NameEnglish)*/.ToList();
                //}
                tweets = GetDummyTweets();

                HttpContext.Current.Cache.Insert(cacheConversationsUK, tweets);
            }
            return tweets;
        }

        public List<Tweet> GetTweetsByLocation(int? locationId)
        {
            if (locationId.HasValue)
            {
                return this.GetTweets().Where(f => f.Location.Id == locationId).ToList();
            }
            else
                return this.GetTweets();
        }

        public List<Tweet> GetDummyTweets()
        {
            var result = new List<Tweet>();

            var parties = PoliticalParty.GetList();

            var party = parties.Where(p => p.Id == 0).FirstOrDefault();
            result.Add(new Tweet() { Timestamp = System.DateTime.Today.AddDays(-7), twitterUser = "xyz", party = party, Location = new MapLocation(null), SentimentScore = -1 });
            result.Add(new Tweet() { Timestamp = System.DateTime.Today.AddDays(-7), twitterUser = "abc", party = party, Location = new MapLocation(null), SentimentScore = 2 });
            result.Add(new Tweet() { Timestamp = System.DateTime.Today.AddDays(-7), twitterUser = "def", party = party, Location = new MapLocation(null), SentimentScore = 0 });
            result.Add(new Tweet() { Timestamp = System.DateTime.Today.AddDays(-6), twitterUser = "xyz", party = party, Location = new MapLocation(null), SentimentScore = 3 });
            result.Add(new Tweet() { Timestamp = System.DateTime.Today.AddDays(-5), twitterUser = "xyz", party = party, Location = new MapLocation(null), SentimentScore = -2 });
            result.Add(new Tweet() { Timestamp = System.DateTime.Today.AddDays(-4), twitterUser = "xyzL", party = party, Location = new MapLocation(null), SentimentScore = 1 });

            party = parties.Where(p => p.Id == 1).FirstOrDefault();
            result.Add(new Tweet() { Timestamp = System.DateTime.Today.AddDays(-7), twitterUser = "xyzL", party = party, Location = new MapLocation(null), SentimentScore = 1 });
            result.Add(new Tweet() { Timestamp = System.DateTime.Today.AddDays(-6), twitterUser = "xyzL", party = party, Location = new MapLocation(null), SentimentScore = -3 });
            result.Add(new Tweet() { Timestamp = System.DateTime.Today.AddDays(-5), twitterUser = "xyzL", party = party, Location = new MapLocation(null), SentimentScore = 5 });
            result.Add(new Tweet() { Timestamp = System.DateTime.Today.AddDays(-4), twitterUser = "xyzL", party = party, Location = new MapLocation(null), SentimentScore = 0 });
            

            party = parties.Where(p => p.Id == 2).FirstOrDefault();
            result.Add(new Tweet() { Timestamp = System.DateTime.Today.AddDays(-7), twitterUser = "xyzU", party = party, Location = new MapLocation(null), SentimentScore = 5 });
            result.Add(new Tweet() { Timestamp = System.DateTime.Today.AddDays(-7), twitterUser = "def", party = party, Location = new MapLocation(null), SentimentScore = 0 });
            result.Add(new Tweet() { Timestamp = System.DateTime.Today.AddDays(-6), twitterUser = "xyzU", party = party, Location = new MapLocation(null), SentimentScore = 0 });
            result.Add(new Tweet() { Timestamp = System.DateTime.Today.AddDays(-5), twitterUser = "xyzU", party = party, Location = new MapLocation(null), SentimentScore = 1 });
            result.Add(new Tweet() { Timestamp = System.DateTime.Today.AddDays(-4), twitterUser = "xyzU", party = party, Location = new MapLocation(null), SentimentScore = 3 });
            return result;
        }

        public void Dispose()
        {
            //db.Dispose();
        }

    }

}