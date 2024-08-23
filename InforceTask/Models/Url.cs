using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InforceTask.Models
{
	public class Url
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

		[Required]
		public string LongUrl { get; set; } = string.Empty;

		public string? ShortUrl { get; set; }

		[MaxLength(50)]
		public string CreatedBy { get; set; } = "Anonymous";

		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
	}
}
