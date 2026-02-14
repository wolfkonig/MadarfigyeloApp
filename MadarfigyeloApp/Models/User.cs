namespace MadarfigyeloApp.Models
{
    public class User(string email)
    {
        public string Email { get; set; } = email;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
    }
}
