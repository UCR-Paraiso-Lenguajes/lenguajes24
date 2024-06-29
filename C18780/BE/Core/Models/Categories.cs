namespace StoreApi.Models
{
    public struct Categories
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public Categories(Category category)
        {
            Uuid = category.Uuid;
            Name = category.Name;
            
        }
    }
}