// <copyright>
//   Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Dan Krusi</author>
// <summary>
//   PlantControl
//   Full-Stack Automated Home Garden
//   https://github.com/dankrusi/PlantControl
// </summary>

using Unosquare.Labs.LiteLib;
using Unosquare.Labs.LiteLib.Log;

namespace PlantControl.Model
{
    public sealed class DataModelContext : LiteDbContext
	{
		public DataModelContext()
			: base(
				System.AppDomain.CurrentDomain.BaseDirectory + "/" + "DB" + "/" + "PlantControl.db", 
				new ConsoleLog())
		{
			// map this context to the database file mydbfile.db and don't use any logging capabilities.
		}

		public DataModelContext(string filename, ILog logger)
			: base(filename, logger)
        {
            // map this context to the database file mydbfile.db and don't use any logging capabilities.
        }

		public LiteDbSet<User> Users { get; set; }
		public LiteDbSet<Plant> Plants { get; set; }
		public LiteDbSet<AuthToken> AuthTokens { get; set; }
		public LiteDbSet<DataPoint> DataPoints { get; set; }

		public void Init() {
			using(DataModelContext db = new DataModelContext()) {
				// No users?
				if(db.Users.Count() == 0) {
					db.Users.Insert(User.CreateUser("admin","admin")); //TODO
				}
			}
		}
    }
}