using ClientDirectory.Domain.Common;

namespace ClientDirectory.Domain.Entities
{
    public class Person : BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public int Gender { get; set; }
        public string Identification { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
