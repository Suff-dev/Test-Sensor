namespace POC.Domain.Entities
{
    public class Measurement
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; }
    }
}
