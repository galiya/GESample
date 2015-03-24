using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GETweetsWeb.Models
{
    public class PredictionModelResult 
    {
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public int Value { get; set; }

    }

}