using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNet5GeoElasticsearch.Controllers
{
    using AspNet5GeoElasticsearch.ElasticsearchApi;
    using AspNet5GeoElasticsearch.Models;

    using Newtonsoft.Json;

    using Swashbuckle.SwaggerGen;
    using Swashbuckle.SwaggerGen.Annotations;

    /// <summary>
    /// This class is used as an api for the search requests.
    /// </summary>
    [Route("api/[controller]")]
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
            initSearchEngine();
            return Ok();
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
