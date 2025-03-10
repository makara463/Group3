using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Group3_LIbraryManagement_AGAAPP.Models
{
    public class Issue
    {
        [Key]
        [MaxLength(10)]
        public required string Id { get; set; }

        [Required]
        [ForeignKey("Book")]
        public required string BookId { get; set; }

        [Required]
        [ForeignKey("Student")]
        public required string StudentId { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Required]
        public required string Status { get; set; }

        // Navigation properties
        public virtual required Book Book { get; set; }
        public virtual required Student Student { get; set; }
        
    }
}

