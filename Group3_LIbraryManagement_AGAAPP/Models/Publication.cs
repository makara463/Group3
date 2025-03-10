using System.ComponentModel.DataAnnotations;
namespace Group3_LIbraryManagement_AGAAPP.Models{
    public class Publication{
        [Key]  
        public string Id { get; set; } // Primary Key  
        [Required]  
        [MaxLength(255)]  
        public string Title { get; set; }  

        [Required]  
        [MaxLength(255)]  
        public string Author { get; set; }  

        [MaxLength(20)]  
        public string ISBN_Code { get; set; }  

        [MaxLength(255)]  
        public string Publisher { get; set; }  

        [MaxLength(100)]  
        public string Genre { get; set; }  

        [MaxLength(100)]  
        public string Publication_Type { get; set; }  

        [MaxLength(100)]  
        public string Publication_Language { get; set; }  

        [MaxLength(100)]  
        public string Book_Edition { get; set; }  

        public int? Page { get; set; }  

        public string Description { get; set; } 

        public int? Available_Copy { get; set; }  

        public int? Copies_Total { get; set; }  
         public virtual ICollection<Book> Books { get; set; } 
    }
}