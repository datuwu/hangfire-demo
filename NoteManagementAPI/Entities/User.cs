using NoteManagementAPI.Entities.Ultities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteManagementAPI.Entities
{
    public class User : BaseModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 6)]
        [Column(TypeName = "nvarchar")]
        public string Username { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 8)]
        [Column(TypeName = "nvarchar")]
        public string Password { get; set; }
        [Required]
        [MaxLength(40)]
        [Column(TypeName = "nvarchar")]
        public string Fullname { get; set; }
        [Required]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar")]
        public string Email { get; set; }
        [Column(TypeName = "nvarchar")]
        public string? ImageUrl { get; set; }
        [DefaultValue(UserStatus.Active)]
        public UserStatus Status { get; set; }
        public DateTime? LastLoginTime { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<Note> Notes { get; set; }
        public ICollection<LoginRecord> LoginRecords { get; set; }
    }
    public enum UserStatus
    {
        Active,
        Disabled,
        Deleted,
    }
}
