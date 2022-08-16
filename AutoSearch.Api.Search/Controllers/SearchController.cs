using AutoSearch.Api.Search.Models.Dto;
using AutoSearch.Api.Search.Settings;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Web;

namespace AutoSearch.Api.Search.Controllers
{

    [Route("api")]
    public class SearchController : ControllerBase
    {
        ISearchSettings _settings;

        public SearchController(ISearchSettings settings)
        {
            this._settings = settings;
        }

        [HttpGet("Search")]
        public async Task<ResponseDto> Search(string text, int num)
        {
            ResponseDto _response = new ResponseDto();

            try
            {

                _response.Result = await GetSearchResultAsync(this._settings, text, num);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessaages = new List<string>() { ex.ToString() };
                throw;
            }
            return _response;
        }



        private async Task<List<int>> GetSearchResultAsync(ISearchSettings settings, string text, int num)
        {
            //"URL": "http://www.google.co.uk/search?num=[num]&q=[keywords]",

            var url = settings.Url.Replace("[keywords]", HttpUtility.UrlEncode(text)).Replace("[num]", num.ToString());

            using var client = new HttpClient();

            var resp = HttpUtility.HtmlDecode(await client.GetStringAsync(url));
            var links = Regex.Matches(resp, settings.Regex).Select(r => r.Value).ToList();
            var result = links == null ? new List<int>() :
                links.Where(l => l.Contains(settings.DesiredUrl, StringComparison.OrdinalIgnoreCase))
                .Select(l => links.IndexOf(l))
                .Distinct()
                .ToList();
            return result;
        }


    }
}
