using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Group3_LIbraryManagement_AGAAPP.Models
{
    public class Attendance
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [ForeignKey("Student")]
        public string StudentId { get; set; }

        [Required]
        public DateTime AttendanceDate { get; set; }

        [Required]
        public DateTime TimeIn { get; set; }

        public DateTime? TimeOut { get; set; } // Nullable because the student might not have timed out yet

        [Required]
        public string Status { get; set; } // e.g., Present, Absent, Late

        // Navigation property
        public virtual Student Student { get; set; }
    }
}

