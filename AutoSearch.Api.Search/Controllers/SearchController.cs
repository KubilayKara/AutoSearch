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
        public async Task<ResponseDto> Search(string text, int num, string desiredUrl, int? searchEngineId)
        {
            ResponseDto _response = new ResponseDto();

            try
            {

                _response.Result = await GetSearchResultAsync(this._settings, text, num, desiredUrl, searchEngineId);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessaages = new List<string>() { ex.ToString() };
                throw;
            }
            return _response;
        }


        [HttpGet("SearchEngines")]
        public async Task<ResponseDto> GetSearchEngines()
        {
            ResponseDto _response = new ResponseDto();

            try
            {

                _response.Result = _settings.SearchEngines;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessaages = new List<string>() { ex.ToString() };
                throw;
            }
            return _response;
        }


        private async Task<List<int>> GetSearchResultAsync(ISearchSettings settings, string text, int num,string desiredUrl, int? searchEngineId)
        {
            //"URL": "http://www.google.co.uk/search?num=[num]&q=[keywords]",

            var engine = searchEngineId != null ? settings.SearchEngines.First(e => e.Id == searchEngineId.Value)
                : settings.SearchEngines.First(e => e.IsDefault);
            var url = engine.Url.Replace("[keywords]", HttpUtility.UrlEncode(text)).Replace("[num]", num.ToString());

            using var client = new HttpClient();

            if (engine.Url.ToLower().Contains("bing"))
            {
                client.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36");
            }

            var resp = HttpUtility.HtmlDecode(await client.GetStringAsync(url));
            var links = Regex.Matches(resp, engine.Regex).Select(r => r.Value).ToList();
            if (string.IsNullOrEmpty(desiredUrl))
                desiredUrl = settings.DesiredUrl;
            var result = links == null ? new List<int>() :
                links.Where(l => l.Contains(desiredUrl, StringComparison.OrdinalIgnoreCase))
                .Select(l => links.IndexOf(l) + 1)
                .Distinct()
                .ToList();
            return result;
        }


    }
}
