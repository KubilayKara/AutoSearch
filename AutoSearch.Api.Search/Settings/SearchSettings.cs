namespace AutoSearch.Api.Search.Settings
{
    public interface ISearchSettings
    {
        public string DesiredUrl { get; set; }
        public string Url { get; set; }
        public string Regex { get; set; }
    }
   
    public class SearchSettings : ISearchSettings
    {

        //   "SearchSettings": {
        //  "URL": "https://www.google.co.uk/search?num=[num]&q=[keywords]",
        //  "Regex": "egMi0 kCrYT(.+?)sa=U"
        //},
        public string DesiredUrl { get; set; }
        public string Url { get; set; }
        public string Regex { get; set; }
    }
}
