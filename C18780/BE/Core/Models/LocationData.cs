namespace StoreApi.Models
{
    public class LocationData
    {
        public string[] BoundingBox { get; set; }
        public string Class { get; set; }
        public string Display_name { get; set; }
        public string Icon { get; set; }
        public double Importance { get; set; }
        public string Lat { get; set; }
        public string Licence { get; set; }
        public string Lon { get; set; }
        public string Osm_id { get; set; }
        public string Osm_type { get; set; }
        public string Place_id { get; set; }
        public string Type { get; set; }
    }
}
