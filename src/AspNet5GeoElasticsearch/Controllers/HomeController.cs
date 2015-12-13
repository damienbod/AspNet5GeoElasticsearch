using AspNet5GeoElasticsearch.Models;
using Newtonsoft.Json;
using Microsoft.AspNet.Mvc;
using AspNet5GeoElasticsearch.ElasticsearchApi;

namespace AspNet5GeoElasticsearch.Controllers
{
    public class HomeController : Controller
	{
        private readonly ISearchProvider _searchProvider;

        public HomeController(ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
        }

		public ActionResult Index()
		{
		    //initSearchEngine();

            var searchResult = _searchProvider.SearchForClosest(0, 7.44461, 46.94792);
			var mapModel = new MapModel
			{
				MapData = JsonConvert.SerializeObject(searchResult),
				// Bern	Lat 46.94792, Long 7.44461
				CenterLatitude = 46.94792,
				CenterLongitude = 7.44461,
				MaxDistanceInMeter=0
			};

			return View(mapModel);
		}

        /// <summary>
        /// tests
        /// </summary>
        /// <param name="maxDistanceInMeter">sdde</param>
        /// <param name="centerLongitude"></param>
        /// <param name="centerLatitude"></param>
        /// <returns></returns>
		public ActionResult Search(uint maxDistanceInMeter, double centerLongitude, double centerLatitude)
		{
			var searchResult = _searchProvider.SearchForClosest(maxDistanceInMeter, centerLongitude, centerLatitude);
			var mapModel = new MapModel
			{
				MapData = JsonConvert.SerializeObject(searchResult),
				CenterLongitude = centerLongitude,
				CenterLatitude = centerLatitude,
				MaxDistanceInMeter = maxDistanceInMeter
			};

			return View("Index", mapModel);
		}

        private void initSearchEngine()
        {
            var searchProvider = new SearchProvider();

            if (!searchProvider.MapDetailsIndexExists())
            {
                searchProvider.InitMapDetailMapping();
                searchProvider.AddMapDetailData();
            }
        }
    }
}