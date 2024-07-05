namespace ApiLab7;

public class TreeNode
{
    public Product Product { get; private set; }
    public TreeNode Left { get; set; }
    public TreeNode Right { get; set; }

    public TreeNode(Product product)
    {
        if (product == null)
            throw new ArgumentException("The product can't be null");
        Product = product;
    }
}

public class ProductSearchTree
{
    private TreeNode root;

    public void AddProduct(Product product)
    {
        if (product == null)
            throw new ArgumentException("The product can't be null");
        root = AddProduct(root, product);
    }

    private TreeNode AddProduct(TreeNode node, Product product)
    {
        if (node == null)
        {
            return new TreeNode(product);
        }

        int compareResult = string.Compare(product.Name, node.Product.Name);

        if (compareResult < 0)
        {
            node.Left = AddProduct(node.Left, product);
        }
        else if (compareResult > 0)
        {
            node.Right = AddProduct(node.Right, product);
        }

        return node;
    }

    public IEnumerable<Product> Search(string query)
    {
        if(string.IsNullOrWhiteSpace(query)) throw new ArgumentException("Query can't be null");
        List<Product> matchedProducts = new List<Product>();
        Search(root, query, matchedProducts);
        return matchedProducts;
    }

    private void Search(TreeNode node, string query, List<Product> matchedProducts)
    {
        if (node == null)
        {
            return;
        }

        bool productMatchesNameOrDescriptionWithQuery = node.Product.Name.ToLower().Contains(query.ToLower()) 
            || node.Product.Description.ToLower().Contains(query.ToLower());

        if (productMatchesNameOrDescriptionWithQuery)
        {
            matchedProducts.Add(node.Product);
        }

        Search(node.Left, query, matchedProducts);
        Search(node.Right, query, matchedProducts);
    }
}
