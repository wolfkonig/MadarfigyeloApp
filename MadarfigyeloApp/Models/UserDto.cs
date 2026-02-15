namespace MadarfigyeloApp.Models
{
    public class UserDto
    {
        public UserDto(string email)
        {
            Email = email;
        }

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
