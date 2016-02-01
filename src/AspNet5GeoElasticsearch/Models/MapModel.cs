namespace AspNet5GeoElasticsearch.Models
{
	public class MapModel
	{
        /// <summary>
        /// The Map Data comment yep
        /// </summary>
		public string MapData { get; set; }

        /// <summary>
        /// My Center Longitude
        /// </summary>
		public double CenterLongitude { get; set; }

        /// <summary>
        /// My Center Latitude
        /// </summary>
		public double CenterLatitude { get; set; }

        /// <summary>
        /// Max distance in meter...
        /// </summary>
		public uint MaxDistanceInMeter { get; set; }
	}
}