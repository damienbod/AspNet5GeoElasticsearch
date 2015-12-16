using Microsoft.AspNet.Mvc;
using AspNet5GeoElasticsearch.ElasticsearchApi;
using AspNet5GeoElasticsearch.Models;
using Swashbuckle.SwaggerGen.Annotations;
using Newtonsoft.Json;

namespace AspNet5GeoElasticsearch.Controllers
{
    /// <summary>
    /// This class is used as an api for the search requests.
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SearchController : Controller
    {
        private readonly ISearchProvider _searchProvider;

        public SearchController(ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
        }

        /// <summary>
        /// This method returns the found documents from Elasticsearch
        /// </summary>
        /// <param name="maxDistanceInMeter">Distance in meters from your location</param>
        /// <param name="centerLongitude">center Longitude </param>
        /// <param name="centerLatitude">center Latitude </param>
        /// <returns>All the documents which were found</returns>
        [HttpGet]
        [Produces(typeof(MapModel))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(MapModel))]
        [Route("GeoSearch")]
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

            return Ok(mapModel);
        }

        /// <summary>
        /// Inits the Elasticsearch documents
        /// </summary>
        [HttpPost]
        [Route("InitData")]
        public ActionResult InitData()
        {
            if (!_searchProvider.MapDetailsIndexExists())
            {
                _searchProvider.InitMapDetailMapping();
                _searchProvider.AddMapDetailData();
            }

            return Ok();
        }
    }
}
