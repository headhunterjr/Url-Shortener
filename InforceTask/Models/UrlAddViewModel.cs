using System.ComponentModel.DataAnnotations;

namespace InforceTask.Models
{
    public class UrlAddViewModel
    {
        [Required]
        public string LongUrl { get; set; } = default!;
    }
}
