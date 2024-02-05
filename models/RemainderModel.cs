
using System.ComponentModel.DataAnnotations;

namespace AllInOne.Models
{
    public class RemainderModel
    {
    
        public string? Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public required string Title { get; set; }

        public string? Description { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public required DateTime SDate { get; set; }
        
        [Required(ErrorMessage = "UserId is required")]
        public required string UserId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public required DateTime EDate { get; set; }

        public string? Status { get; set; }

        
    }
}