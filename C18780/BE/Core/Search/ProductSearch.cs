using System.Text.RegularExpressions;
using StoreApi.Models;

namespace StoreApi.Search
{
    public sealed class InvertedTreeNode
    {
        internal Dictionary<string, InvertedTreeNode> Words { get; }
        internal HashSet<Product> Products { get; }

        public InvertedTreeNode()
        {
            Words = new Dictionary<string, InvertedTreeNode>();
            Products = new HashSet<Product>();
        }
    }

    public sealed class ProductSearch
    {
        private readonly InvertedTreeNode root;

        public ProductSearch(IEnumerable<Product> products)
        {
            if (products.Count() == 0)
            {
                throw new ArgumentException("The products cannot be empty.");
            }
            root = new InvertedTreeNode();
            BuildInvertedTree(products);
        }

        private void BuildInvertedTree(IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                var words = GetWordsFromText(product.Name + " " + product.Description);
                foreach (var word in words)
                {
                    AddWordToInvertedTree(word, product);
                }
            }
        }

        private IEnumerable<string> GetWordsFromText(string text)
        {
            return Regex.Split(text.ToLower(), @"\W+").Where(word => !string.IsNullOrWhiteSpace(word));
        }

        private void AddWordToInvertedTree(string word, Product product)
        {
            var node = root;
            if (!node.Words.ContainsKey(word))
            {
                node.Words[word] = new InvertedTreeNode();
            }
            node = node.Words[word];
            node.Products.Add(product);
        }

        public IEnumerable<Product> Search(string search)
        {
            var keywords = GetWordsFromText(search);
            var resultSet = new HashSet<Product>();
            var node = root;
            foreach (var keyword in keywords)
            {
                if (node.Words.ContainsKey(keyword))
                {
                    node = node.Words[keyword];
                    resultSet.UnionWith(node.Products);
                }
                else
                {
                    return Enumerable.Empty<Product>();
                }
            }
            return resultSet;
        }
    }
}
