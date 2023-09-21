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
        public string? Author { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "Title length less than 10")]
        public string? Title { get; set; }
        public ushort FavoriteCount { get; set; }
        [Required]
        [StringLength(2000, MinimumLength = 3, ErrorMessage = "Max Character=2000, Min=3")]
        public string? Description { get; set; }
        public DateTime? RecordDateTime { get; set; }
        [Required]
        [StringLength(1024 * 1024 * 10, ErrorMessage = "Image Size must lower than 10mb")]
        public string? Base64ImageData { get; set; }
    }
}
