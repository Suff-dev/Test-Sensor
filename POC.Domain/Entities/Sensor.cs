namespace POC.Domain.Entities
{
    public class Sensor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string EquipmentId { get; set; }
        public List<Measurement> Measurements { get; set; }
    }
}
