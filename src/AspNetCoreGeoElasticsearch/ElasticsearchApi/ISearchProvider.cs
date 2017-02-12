using System.Collections.Generic;
using AspNet5GeoElasticsearch.Models;

namespace AspNet5GeoElasticsearch.ElasticsearchApi
{
    public interface ISearchProvider
    {
        void AddMapDetailData();
        void InitMapDetailMapping();
        bool MapDetailsIndexExists();
        List<MapDetail> SearchForClosest(uint maxDistanceInMeter, double centerLongitude, double centerLatitude);
    }
}