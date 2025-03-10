using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Group3_LIbraryManagement_AGAAPP.Models{
    public class Book{
         [Key]  
        [Column("id")]  
        public string Id { get; set; } // Primary key  
         
        public string Title { get; set; }
            [Required]
        [MaxLength(20)]
        public string ISBN_Code { get; set; } // ISBN Code  

        [ForeignKey("Publication")]  
        public string PublicationId { get; set; } // Foreign Key to Publication  

        [Required]  
        [MaxLength(100)]  
        public string ShelfLocation { get; set; } // Shelf location of the book  

        [MaxLength(50)]  
        public required string BookCondition { get; set; } // Condition of the book  

        [MaxLength(50)]  
        public required string Status { get; set; } // Status of the book  
        // Navigation property  
        public virtual required Publication Publication { get; set; } // Reference to Publication  
        public ICollection<Issue> Issues { get; set; }
    }
}