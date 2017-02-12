namespace AspNet5GeoElasticsearch.Models
{
    using ElasticsearchCRUD.ContextAddDeleteUpdate.CoreTypeAttributes;
    using ElasticsearchCRUD.Model.GeoModel;

    public class MapDetail
	{
		public long Id { get; set; }

		public string Name { get; set; }

		public string Details { get; set; }

		public string Information { get; set; }

		public string DetailsType { get; set; }

		[ElasticsearchGeoPoint]
		public GeoPoint DetailsCoordinates { get; set; }
	}
}