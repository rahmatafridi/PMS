namespace ds.pms.dal.Models
{
    public partial class PmsDB
    {
        public PmsDB(string providerName, string connectionString)
            : base(providerName, connectionString)
        {
            InitDataContext();
            InitMappingSchema();
        }
    }
}
