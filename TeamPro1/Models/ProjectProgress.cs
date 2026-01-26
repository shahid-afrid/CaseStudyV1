using System.ComponentModel.DataAnnotations;

namespace TeamPro1.Models
{
    public class ProjectProgress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TeamId { get; set; }

        // Assigned Faculty (Mentor)
        public int? AssignedFacultyId { get; set; }

        // Problem Statement assigned to the team
        [StringLength(500)]
        public string? ProblemStatement { get; set; }

        // Project Completion Percentage (0-100)
        [Range(0, 100)]
        public int CompletionPercentage { get; set; } = 0;

        // Proof/Documentation uploads (comma-separated file paths or URLs)
        public string? ProofUploads { get; set; }

        // Faculty Review/Suggestions (editable only by faculty)
        public string? FacultyReview { get; set; }

        // Status: Pending, In Progress, Completed, etc.
        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdated { get; set; }

        // Navigation properties
        public virtual Team? Team { get; set; }
        public virtual Faculty? AssignedFaculty { get; set; }
    }
}
