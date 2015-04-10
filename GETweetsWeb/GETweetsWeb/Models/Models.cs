using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GETweetsWeb.Models
{
    public class NetVoteModel {
        
        public int Order { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }

        public string Color { get; set; }
    }

    public class NetVotePercentModel
    {
        

        public int Order { get; set; }
        public string Name { get; set; }
        public Decimal Percent { get; set; }

        public string Color { get; set; }
    }

    public class SentimentVoteModel {

        public int Order { get; set; }
        public string Name { get; set; }

        

        public int PositiveCount { get; set; }
        public int NegativeCount { get; set; }
        public string Color { get; set; }
    }

    public class NetVoteData
    {
        public DateTime LastUpdated { get; set; }
        public List<NetVoteModel> Data { get; set; }
    }

    public class NetVotePercentData
    {
        public DateTime LastUpdated { get; set; }
        public List<NetVotePercentModel> Data { get; set; }
    }

    public class SentimentVoteDataModel
    {
        //public DateTime LastUpdated { get; set; }
        //public List<SentimentVoteModel> Data { get; set; }

        public string Key { get; set; }
        public string Color { get; set; }
        public List<object[]> Values { get; set; }
    }

    public class SentimentVoteData
    {
        public DateTime LastUpdated { get; set; }
        public List<SentimentVoteDataModel> Data { get; set; }
    }

    public class NetVoteTrendDataModel
    {
        public int Order { get; set; }
        public string Key { get; set; }
        public string Color { get; set; }

        
        public List<object[]> Values {get; set;}

    }

 

    public class NetVoteTrendData {
        public DateTime LastUpdated { get; set; }

        public List<NetVoteTrendDataModel> Data { get; set; }
    }



    public class MapVoteData
    {
        public Dictionary<int, string> Data { get; set; }  
    }

}