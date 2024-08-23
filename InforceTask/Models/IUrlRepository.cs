namespace InforceTask.Models
{
	public interface IUrlRepository
	{
		public Task<IEnumerable<Url>> GetAllUrlsAsync();
		public Task<Url?> GetUrlByIdAsync(int id);
		public Task<int> AddUrlAsync(Url url);
		public string ShortenUrlAlgorithm(string longUrl);
		public void DeleteUrl(Url url);
		public Task<bool> SaveChangesAsync();
	}
}
