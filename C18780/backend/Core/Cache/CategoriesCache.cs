using StoreApi.Models;

namespace StoreApi.Cache
{
    public sealed class CategoriesCache
    {
        public static IEnumerable<Categories> _categories { get; set; }
        private CategoriesCache() { }

        private static CategoriesCache _instance;

        public static CategoriesCache GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CategoriesCache();
            }
            return _instance;
        }
        public Categories GetCategoryByName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                foreach (var category in _categories)
                {
                    if (category.Name.Equals(name))
                    {
                        return category;
                    }
                }
            }
            return default;
        }
    }
}