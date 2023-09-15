using System.ComponentModel.DataAnnotations;

namespace MudBlazorCountJungLab.Models
{
    public class InstarPostItem
    {
        public InstarPostItem() 
        {
            Id = new();
        }
        public Guid Id { get; set; }
        [Required]
        [StringLength(2000, MinimumLength = 3, ErrorMessage = "Max Character=2000, Min=3")]
        public string? Description { get; set; }
        public DateTime? RecordDateTime { get; set; }
        [Required]
        public string? Base64ImageData { get; set; }
        [Required]
        public string? Author { get; set; }
    }
}
