namespace POC.Domain.Entities
{
    public class Equipment
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> SensorIds { get; set; }
    }
}
