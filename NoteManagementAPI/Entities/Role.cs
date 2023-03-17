using NoteManagementAPI.Entities.Ultities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteManagementAPI.Entities
{
    public class Role : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [DefaultValue(RoleStatus.Active)]
        public RoleStatus Status { get; set; }
        
        public ICollection<User> Users { get; set; }
    }
    public enum RoleStatus
    {
        Active,
        Disabled,
        Deleted,
    }

    public enum RoleName
    {
        Admin,
        User,
    }
}
