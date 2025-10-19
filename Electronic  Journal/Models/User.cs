namespace Electronic__Journal.Models
{
    public class User
    {
        public int? Id { get; init; }

        public string? Login { get; init; }

        public string? LastName { get; init; }

        public string? FirstName { get; init; }

        public string? MiddleName { get; init; }

        public int? GroupId { get; set; }

        public string? DateOfBirth { get; init; }
        
        public UserType Type { get; set; }

    }

    public enum UserType
    { Student, Teacher }
}
