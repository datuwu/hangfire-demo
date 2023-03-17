using NoteManagementAPI.Entities;

namespace NoteManagementAPI.Models
{
    public class RoleUpdateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public RoleStatus Status { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
