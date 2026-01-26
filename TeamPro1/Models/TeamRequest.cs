using System.ComponentModel.DataAnnotations;

namespace TeamPro1.Models
{
    public class TeamRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Student? Sender { get; set; }
        public virtual Student? Receiver { get; set; }
    }
}
