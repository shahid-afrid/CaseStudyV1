using System.ComponentModel.DataAnnotations;

namespace TeamPro1.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TeamNumber { get; set; }

        [Required]
        public int Student1Id { get; set; }

        [Required]
        public int Student2Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual Student? Student1 { get; set; }
        public virtual Student? Student2 { get; set; }
    }
}
