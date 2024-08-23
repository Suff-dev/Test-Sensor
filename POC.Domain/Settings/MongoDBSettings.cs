namespace POC.Domain.Settings
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string MedicoesCollectionName { get; set; }
        public string SensoresCollectionName { get; set; }
    }
}
