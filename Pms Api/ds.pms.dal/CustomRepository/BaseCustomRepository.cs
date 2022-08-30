using ds.pms.dal.Models;
using LinqToDB.Data;
using System.Diagnostics;

namespace ds.pms.dal.CustomRepository
{
    public class BaseCustomRepository
    {
        protected PmsDB dataContext;
        //protected int clientId;
        protected string providerName, connectionString;

        public BaseCustomRepository(string databaseProviderName, string databaseConnectionString)
        {
            providerName = databaseProviderName;
            connectionString = databaseConnectionString;
            dataContext = new PmsDB(providerName, connectionString);
            LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;

            if (Debugger.IsAttached)
            {
                DataConnection.TurnTraceSwitchOn();
                //DataConnection.WriteTraceLine = (s, s1) => Debug.WriteLine(s, s1);
            }
        }
    }
}
