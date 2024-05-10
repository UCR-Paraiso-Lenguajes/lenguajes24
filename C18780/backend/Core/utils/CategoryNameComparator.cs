using System.Collections;
using StoreApi.Models;

public class CategoryNameComparator : IComparer<Categories>
{
    public int Compare(Categories x, Categories y)
    {
        return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
    }
}