using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


namespace GETweetsWeb
{

    public partial class GETweetsWebDB : DbContext
    {
        public GETweetsWebDB(string sConnectionString)
            : base(sConnectionString)
        {
        }
    }
}