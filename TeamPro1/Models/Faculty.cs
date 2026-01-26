using System.ComponentModel.DataAnnotations;

namespace TeamPro1.Models
{
    public class Faculty
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

        [StringLength(100)]
        public string Department { get; set; } = "Computer Science";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
