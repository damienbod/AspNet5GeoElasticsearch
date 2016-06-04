using System.Collections.Generic;
using System.Linq;
using AspNet5GeoElasticsearch.Models;
using ElasticsearchCRUD;
using ElasticsearchCRUD.Model.GeoModel;
using ElasticsearchCRUD.Model.SearchModel;
using ElasticsearchCRUD.Model.SearchModel.Filters;
using ElasticsearchCRUD.Model.SearchModel.Queries;
using ElasticsearchCRUD.Model.SearchModel.Sorting;
using ElasticsearchCRUD.Model.Units;
using ElasticsearchCRUD.Tracing;

namespace AspNet5GeoElasticsearch.ElasticsearchApi
{
    public class SearchProvider : ISearchProvider
    {
        private readonly IElasticsearchMappingResolver _elasticsearchMappingResolver = new ElasticsearchMappingResolver();
        private const string ConnectionString = "http://localhost:9200";

        public void InitMapDetailMapping()
        {
            using (var context = new ElasticsearchContext(ConnectionString, new ElasticsearchSerializerConfiguration(_elasticsearchMappingResolver)))
            {
                context.TraceProvider = new ConsoleTraceProvider();
                context.IndexCreate<MapDetail>();
            }
        }

        public void AddMapDetailData()
        {
            var dotNetGroup = new MapDetail { DetailsCoordinates = new GeoPoint(7.47348, 46.95404), Id = 1, Name = ".NET User Group Bern", Details = "http://www.dnug-bern.ch/", DetailsType = "Work" };
            var dieci = new MapDetail { DetailsCoordinates = new GeoPoint(7.41148, 46.94450), Id = 2, Name = "Dieci Pizzakurier Bern", Details = "http://www.dieci.ch", DetailsType = "Pizza" };
            var babylonKoeniz = new MapDetail { DetailsCoordinates = new GeoPoint(7.41635, 46.92737), Id = 3, Name = "PIZZERIA BABYLON Köniz", Details = "http://www.pizza-babylon.ch/home-k.html", DetailsType = "Pizza" };
            var babylonOstermundigen = new MapDetail { DetailsCoordinates = new GeoPoint(7.48256, 46.95578), Id = 4, Name = "PIZZERIA BABYLON Ostermundigen", Details = "http://www.pizza-babylon.ch/home-o.html", DetailsType = "Pizza" };
            using (var context = new ElasticsearchContext(ConnectionString, new ElasticsearchSerializerConfiguration(_elasticsearchMappingResolver)))
            {
                context.TraceProvider = new ConsoleTraceProvider();
                context.AddUpdateDocument(dotNetGroup, dotNetGroup.Id);
                context.AddUpdateDocument(dieci, dieci.Id);
                context.AddUpdateDocument(babylonKoeniz, babylonKoeniz.Id);
                context.AddUpdateDocument(babylonOstermundigen, babylonOstermundigen.Id);
                context.SaveChanges();
            }
        }

        //{
        //  "query" :
        //  {
        //	"filtered" : {
        //		"query" : {
        //			"match_all" : {}
        //		},
        //		"filter" : {
        //			"geo_distance" : {
        //				"distance" : "300m",
        //				 "detailscoordinates" : [7.41148,46.9445]
        //			}
        //		}
        //	}
        //  },
        // "sort" : [
        //		{
        //			"_geo_distance" : {
        //				"detailscoordinates" : [7.41148,46.9445],
        //				"order" : "asc",
        //				"unit" : "km"
        //			}
        //		}
        //	]
        //	}
        //}
        public List<MapDetail> SearchForClosest(uint maxDistanceInMeter, double centerLongitude, double centerLatitude)
        {
            var search = new Search
            {
                Query = new Query(
                    new Filtered( 
                        new Filter(
                            new GeoDistanceFilter( 
                                "detailscoordinates", 
                                new GeoPoint(centerLongitude, centerLatitude), 
                                new DistanceUnitMeter(maxDistanceInMeter)
                            )
                        )
                    )
                    {
                        Query = new Query(new MatchAllQuery())
                    }
                ),
                Sort = new SortHolder(
                    new List<ISort>
                    {
                        new SortGeoDistance("detailscoordinates", DistanceUnitEnum.m, new GeoPoint(centerLongitude, centerLatitude))
                        {
                            Order = OrderEnum.asc
                        }
                    }
                )
            };

            List<MapDetail> result;
            using (var context = new ElasticsearchContext(ConnectionString, new ElasticsearchSerializerConfiguration(_elasticsearchMappingResolver)))
            {
                result = context.Search<MapDetail>(search).PayloadResult.Hits.HitsResult.Select(t => t.Source).ToList();
            }

            return result;
        }

        public bool MapDetailsIndexExists()
        {
            bool exists;
            using (
                var context = new ElasticsearchContext(ConnectionString,
                    new ElasticsearchSerializerConfiguration(_elasticsearchMappingResolver)))
            {
                exists = context.IndexExists<MapDetail>();
            }

            return exists;
        }
    }
}