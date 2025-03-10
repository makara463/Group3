using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Group3_LIbraryManagement_AGAAPP.Models
{
    public class Penalty
    {
        [Key]
        [Required]
        public required string Id { get; set; }

        [Required]
        [ForeignKey("Student")]
        public required string StudentId { get; set; }

        [Required]
        [ForeignKey("Issue")]
        public required string IssueId { get; set; }

        [Required]
        public required string PenaltyType { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public DateTime PenaltyDate { get; set; }

        [Required]
        public required string PaymentStatus { get; set; }

        public DateTime? PaymentDate { get; set; }

        // Navigation properties
        public virtual required Student Student { get; set; }
        public virtual required Issue Issue { get; set; }
    }
}
