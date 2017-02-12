using Microsoft.AspNetCore.Mvc;
using AspNetCoreGeoElasticsearch.ElasticsearchApi;
using AspNetCoreGeoElasticsearch.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspNetCoreGeoElasticsearch.Controllers
{
    /// <summary>
    /// This class is used as an api for the search requests.
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SearchController : Controller
    {
        private readonly ISearchProvider _searchProvider;

        /// <summary>
        /// Search controller for geo search service
        /// </summary>
        /// <param name="searchProvider"></param>
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
        [SwaggerResponse(200, Type = typeof(MapModel))]
        [Route("GeoSearch")]
        public ActionResult Search([FromQuery]uint maxDistanceInMeter, [FromQuery]double centerLongitude, [FromQuery]double centerLatitude)
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
        /// http://localhost:21453/api/Search/InitData
        /// </summary>
        [HttpPost]
        [HttpGet]
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
