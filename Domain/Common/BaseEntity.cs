namespace ClientDirectory.Domain.Common
{
    public interface IBase
    {
        public string Id { get; set; }
        public bool Deleted { get; set; }
    }
    public class BaseEntity : IBase
    {
        public string Id { get; set; }
        public bool Deleted { get; set; }
    }
}
