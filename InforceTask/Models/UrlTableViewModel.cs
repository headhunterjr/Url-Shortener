using System.ComponentModel.DataAnnotations;

namespace InforceTask.Models
{
    public class UrlTableViewModel
    {
        [Required]
        public string LongUrl { get; set; } = default!;
        public string CreatedBy { get; set; } = "Anonymous";
        public IEnumerable<Url>? Urls { get; set; }
    }
}
