
using Microsoft.EntityFrameworkCore;
using System.Text;
using static System.Net.WebRequestMethods;

namespace InforceTask.Models
{
	public class UrlRepository : IUrlRepository
	{
		private readonly UrlDbContext _context;

		public UrlRepository(UrlDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<IEnumerable<Url>> GetAllUrlsAsync()
		{
			return await _context.Urls.ToListAsync();
		}

		public async Task<Url?> GetUrlByIdAsync(int id)
		{
			return await _context.Urls.FirstOrDefaultAsync(u => u.Id == id);
		}

        public async Task<int> AddUrlAsync(Url url)
        {
			_context.Urls.Add(url);
			return await _context.SaveChangesAsync();
        }

        public string ShortenUrlAlgorithm(string longUrl)
        {
			
            string partToKeep = "https://localhost:7074/s/";
            StringBuilder shortenedUrl = new StringBuilder();

            for (int i = 0; i < longUrl.Length; i++)
            {
                if (i % 4 == 0)
                {
                    shortenedUrl.Append(longUrl[i]);
                }
            }
            shortenedUrl = shortenedUrl.Replace("/", "");
            return partToKeep + "~" + shortenedUrl.ToString() + "~";

        }

        public void DeleteUrl(Url url)
        {
			_context.Urls.Remove(url);
        }

        public async Task<bool> SaveChangesAsync()
        {
			return await _context.SaveChangesAsync() >= 0;
        }
    }
}
