using AspNetCoreGeoElasticsearch.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreGeoElasticsearch.ElasticsearchApi;

namespace AspNetCoreGeoElasticsearch.Controllers
{
    /// <summary>
    /// Home controller which uses the search data and displays it in a google ma
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ISearchProvider _searchProvider;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="searchProvider"></param>
        public HomeController(ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
        }

        /// <summary>
        /// Default load method
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //if (!_searchProvider.MapDetailsIndexExists())
            //{
            //    _searchProvider.InitMapDetailMapping();
            //    _searchProvider.AddMapDetailData();
            //}

            var searchResult = _searchProvider.SearchForClosest(0, 7.44461, 46.94792);
            var mapModel = new MapModel
            {
                MapData = JsonConvert.SerializeObject(searchResult),
                // Bern	Lat 46.94792, Long 7.44461
                CenterLatitude = 46.94792,
                CenterLongitude = 7.44461,
                MaxDistanceInMeter = 0
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
    }
}