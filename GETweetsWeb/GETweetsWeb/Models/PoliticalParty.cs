using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GETweetsWeb.Models
{
    public class PoliticalParty
    {
        public int Id { get; set; }
        public string LongName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Color { get; set; }

        public string[] Shades { get; set; }
        public int Order { get; set; }

        public static List<PoliticalParty> GetList()
        {
            var result = new List<PoliticalParty>();

            result.Add(new PoliticalParty() { Id = 0, Order = 1,
                LongName = "Conservative and Unionist Party", Name = "Conservative", Code = "con",
                Color = "#0087dc", Shades = new string []{ "#0087DC", "#009BFA", "#37B3FF", "#69C6FF",} });

            result.Add(new PoliticalParty() { Id = 2, Order = 2,
                LongName = "Liberal Democrats", Name = "Lib Dem", 
                Code = "lib", Color = "#FDBB30", Shades = new string[] { "#FDBB30", "#FDC659", "#FDCA63", "#FED582" } });

            result.Add(new PoliticalParty() { Id = 1, Order = 3,
                LongName = "Labour Party", Name = "Labour", Code = "lab",
                Color = "#d50000", Shades = new string[] { "#D50000", "#F60000", "#FF2929", "#FF4F4F" } });

            result.Add(new PoliticalParty() { Id = 4, Order = 4,
                LongName = "Scottish National Party", Name = "SNP", Code = "snp",
                Color = "#FFF95D", Shades = new string[] { "#FFF95D", "#FFF875", "#FFFBA7", "#FFFCC1" } });

            result.Add(new PoliticalParty() { Id = 5, Order = 5,
                LongName = "Green Party of England and Wales", Name = "Green", Code = "grn",
                Color = "#008066", Shades = new string[] { "#008066", "#009273", "#00A481", "#00B48D" } });

            result.Add(new PoliticalParty() { Id = 3, Order = 6,
                LongName = "UK Independence Party", Name = "UKIP", Code = "ukp",
                Color = "#B3009D", Shades = new string[] { "#B7009D", "#CC00AF", "#E200C2", "#FF33E2" } });

            return result;
        }
    }
}