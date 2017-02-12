using System.Collections.Generic;
using AspNetCoreGeoElasticsearch.Models;

namespace AspNetCoreGeoElasticsearch.ElasticsearchApi
{
    public interface ISearchProvider
    {
        void AddMapDetailData();
        void InitMapDetailMapping();
        bool MapDetailsIndexExists();
        List<MapDetail> SearchForClosest(uint maxDistanceInMeter, double centerLongitude, double centerLatitude);
    }
}