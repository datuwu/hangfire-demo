using NoteManagementAPI.Entities;

namespace NoteManagementAPI.Models
{
    public class UserUpdateVM
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public int RoleId { get; set; }
        public DateTime UpdateTime { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;
    }
}
