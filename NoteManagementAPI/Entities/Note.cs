using NoteManagementAPI.Entities.Ultities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace NoteManagementAPI.Entities
{
    public class Note : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        [Column(TypeName = "nvarchar")]
        public string Title { get; set; }
        [Required]
        [MaxLength(300)]
        [Column(TypeName = "nvarchar")]
        public string Content { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

    }

    public enum NoteStatus
    {
        Published,
        Drafted,
        Error,
    }

    
}
