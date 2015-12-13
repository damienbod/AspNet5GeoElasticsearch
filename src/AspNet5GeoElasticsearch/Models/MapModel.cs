namespace AspNet5GeoElasticsearch.Models
{
	public class MapModel
	{
		public string MapData { get; set; }
		public double CenterLongitude { get; set; }
		public double CenterLatitude { get; set; }
		public uint MaxDistanceInMeter { get; set; }
	}
}