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

        public static List<PoliticalParty> GetList()
        {
            var result = new List<PoliticalParty>();

            result.Add(new PoliticalParty() { Id = 0, LongName = "Conservative and Unionist Party", Name = "Conservative", Code = "Con", Color = "#0087dc" });
            result.Add(new PoliticalParty() { Id = 1, LongName = "Labour Party", Name = "Labour", Code = "Lab", Color = "#d50000" });
            result.Add(new PoliticalParty() { Id = 2, LongName = "Liberal Democrats", Name = "Lib Dem", Code = "Ld", Color = "#FDBB30" });
            result.Add(new PoliticalParty() { Id = 3, LongName = "UK Independence Party", Name = "UKIP", Code = "Ukip", Color = "#B3009D" });
            result.Add(new PoliticalParty() { Id = 4, LongName = "Scottish National Party", Name = "SNP", Code = "Snp", Color = "#FFF95D" });
            result.Add(new PoliticalParty() { Id = 5, LongName = "Green Party of England and Wales", Name = "Green", Code = "Grn", Color = "#008066" });
            return result;
        }
    }
}