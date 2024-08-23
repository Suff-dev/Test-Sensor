namespace POC.Application.Dtos
{
    public class SensorDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string EquipmentId { get; set; }
        public List<MeasurementDto> Measurements { get; set; }
    }
}
