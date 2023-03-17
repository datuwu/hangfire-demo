using NoteManagementAPI.Entities;

namespace NoteManagementAPI.Models
{
    public class UserRegisterVM
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;
    }
}
