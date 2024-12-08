namespace AdaptationManagement.Models
{
    public class Mentor
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Department Department { get; set; }
    }
}