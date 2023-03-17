namespace NoteManagementAPI.Models
{
    public class NoteUpdateVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
