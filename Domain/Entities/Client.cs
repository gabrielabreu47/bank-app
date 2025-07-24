namespace ClientDirectory.Domain.Entities
{
    public class Client : Person
    {
        public string Password { get; set; }
        public bool Status { get; set; }
    }
}
