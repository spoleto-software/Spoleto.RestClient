namespace Spoleto.RestClient.Tests.Models
{
    public class ObjectModel
    {
        public Guid ReportId { get; set; }

        public string ReportOutputType { get; set; }

        public ObjectType ObjectType { get; set; }

        public Guid?[] ObjectIds { get; set; }

        public string? ReportFormJson { get; set; }
    }
}
