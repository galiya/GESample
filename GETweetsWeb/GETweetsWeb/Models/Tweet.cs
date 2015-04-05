using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GETweetsWeb.Models
{
    public class MapLocation
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public MapLocation(int? id)
        {
            if (!id.HasValue)
            {
                Id = 0;
                Name = "UK";
            }
            else
            {
                // read from lookup table
                Id = id.Value;
                Name = "DummyLocation" + id.Value.ToString();
            }
        }
    }

    


    //public class Tweet
    //{
    //    public DateTime Timestamp { get; set; }
    //    public MapLocation Location {get; set;}
    //    public string twitterUser { get; set; }
    //    //public string twitterMsg { get; set; }
    //    public PoliticalParty party { get; set; }
    //    public int SentimentScore { get; set; }

    //}

    public class TweetsByPartyCount {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class TweetsByPartyPercent
    {
        public string Name { get; set; }
        public Decimal Percent { get; set; }
    }

    public class TweetsByDates
    {
        public string Key { get; set; }
        public string Color { get; set; }
        public List<object[]> Values {get; set;}

    }

    public class NetVoteData {
        public DateTime LastUpdated { get; set; }
        public List<TweetsByPartyCount> Data { get; set; }
    }

    public class NetVoteTrendData {
        public DateTime LastUpdated { get; set; }

        public List<TweetsByDates> Data { get; set; }
    }

    public class NetVotePercentData
    {
        public DateTime LastUpdated { get; set; }
        public List<TweetsByPartyPercent> Data { get; set; }
    }
}