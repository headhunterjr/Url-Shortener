using InforceTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InforceTask.Controllers
{
	public class UrlController : Controller
	{
		private readonly IUrlRepository _urlRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UrlController(IUrlRepository urlRepository, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_urlRepository = urlRepository ?? throw new ArgumentNullException(nameof(urlRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

		public async Task<ActionResult<IEnumerable<Url>>> ShortUrlsTable()
		{
			var urls = await _urlRepository.GetAllUrlsAsync();
			UrlTableViewModel urlTableViewModel = new UrlTableViewModel { Urls = urls };
			return View(urlTableViewModel);
		}
		public async Task<IActionResult> ShortUrlInfo(int id)
		{
			var url = await _urlRepository.GetUrlByIdAsync(id);
			if (url == null)
			{
				return NotFound();
			}
			return View(url);
		}
		public IActionResult About()
		{
			return View();
		}
		[Authorize]
		public IActionResult Add()
		{
			UrlAddViewModel urlAddViewModel = new UrlAddViewModel();
			return View(urlAddViewModel);
		}
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Add([FromBody] UrlAddViewModel urlAddViewModel)
		{
			Url urlToAdd = new Url
			{
				LongUrl = urlAddViewModel.LongUrl,
				ShortUrl = _urlRepository.ShortenUrlAlgorithm(urlAddViewModel.LongUrl),
				CreatedBy = _signInManager.IsSignedIn(User) ? _userManager.GetUserName(User) : "Anonymous",
				CreatedDate = DateTime.Now
			};
			var urls = await _urlRepository.GetAllUrlsAsync();
			if (urls.Any(u => u.LongUrl == urlToAdd.LongUrl || u.ShortUrl == urlToAdd.ShortUrl))
			{
				return BadRequest("Duplicate URL");
			}
			await _urlRepository.AddUrlAsync(urlToAdd);
			return Ok(urlToAdd);
		}
		[Authorize]
		public async Task<IActionResult> Delete(int id)
		{
			Url? urlToDelete = await _urlRepository.GetUrlByIdAsync(id);
			if (urlToDelete == null)
			{
				return NotFound("This url does not exist");
			}
			_urlRepository.DeleteUrl(urlToDelete);
			await _urlRepository.SaveChangesAsync();
			return RedirectToAction(nameof(ShortUrlsTable));
		}
		[Route("s/{url}")]
		public async Task<IActionResult> RedirectToOriginalUrl(string? url)
		{
			if (string.IsNullOrEmpty(url))
			{
				return RedirectToAction("Index", "Home");
			}
			string fullUrl = "https://localhost:7074/s/" + url;
			if (url.StartsWith('~') && url.EndsWith('~'))
			{
				var urls = await _urlRepository.GetAllUrlsAsync();
				var foundUrl = urls.FirstOrDefault(u => u.ShortUrl == fullUrl);
				if (foundUrl != null)
				{
					return Redirect(foundUrl.LongUrl);
				}
				return NotFound();
			}
			return Redirect(fullUrl);
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}
