using Unosquare.Labs.LiteLib;
using Unosquare.Labs.LiteLib.Log;

namespace PlantControl.Model
{
    internal sealed class DataModelContext : LiteDbContext
    {
		public DataModelContext(string filename, ILog logger)
			: base(filename, logger)
        {
            // map this context to the database file mydbfile.db and don't use any logging capabilities.
        }

        //public LiteDbSet<Person> People { get; set; }
    }
}