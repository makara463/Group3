using System.ComponentModel.DataAnnotations;

namespace Group3_LIbraryManagement_AGAAPP.Models
{
    public class Student
    {
        [Key]
        [MaxLength(10)]
        public required string Id { get; set; }

        [Required] 
        [StringLength(50)]
        public required string Name { get; set; }

        [Required] 
        [StringLength(50)]
        public required string Gender { get; set; }

        [Required] 
        [StringLength(50)]
        public required string Phone { get; set; }

        [Required] 
        [EmailAddress]
        public required string Email { get; set; }

        [Required] 
        [StringLength(50)]
        public required string Major { get; set; }

        [Required] 

        public int Year { get; set; }

        [Required] 
        public required string Semester { get; set; }

        [Required] 
        public required string Shift { get; set; }



        public required ICollection<Issue> Issues { get; set; }


        public required ICollection<Penalty> Penalties { get; set; }


        public required ICollection<Attendance> Attendances { get; set; }
    }
}
