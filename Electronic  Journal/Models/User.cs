namespace Electronic__Journal.Models
{
    public class User
    {
        public int? Id { get; init; }

        public string? FirstName { get; init; }

        public string? MiddleName { get; init; }

        public string? LastName { get; init; }

        public string? Group { get; set; }
        
        public UserType Type { get; set; }

    }

    public enum UserType
    { Student, Teacher }
}
