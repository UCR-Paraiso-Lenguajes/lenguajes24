using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class BSTNode<T> where T : IComparable<T>
    {
        public List<T> Values { get; private set; }
        public BSTNode<T> Left { get; private set; }
        public BSTNode<T> Right { get; private set; }

        public BSTNode(T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value), "El valor no puede ser nulo.");
            Values = new List<T> { value };
        }

        public void AddValue(T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value), "El valor no puede ser nulo.");
            Values.Add(value);
        }

        public void SetLeft(BSTNode<T> left)
        {
            Left = left;
        }

        public void SetRight(BSTNode<T> right)
        {
            Right = right;
        }
    }

    public class BinarySearchTree<T> where T : IComparable<T>
    {
        public BSTNode<T> Root { get; private set; }

        public void Insert(T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value), "El valor no puede ser nulo.");
            Root = InsertRec(Root, new BSTNode<T>(value));
        }

        private BSTNode<T> InsertRec(BSTNode<T> root, BSTNode<T> newNode)
        {
            if (root == null)
            {
                return newNode;
            }

            int comparison = newNode.Values[0].CompareTo(root.Values[0]);
            if (comparison < 0)
            {
                var left = InsertRec(root.Left, newNode);
                root.SetLeft(left);
            }
            else if (comparison > 0)
            {
                var right = InsertRec(root.Right, newNode);
                root.SetRight(right);
            }
            else
            {
                root.AddValue(newNode.Values[0]);
            }

            return root;
        }

        public List<T> Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) throw new ArgumentNullException(nameof(searchString), "El valor no puede ser nulo.");
            return SearchRec(Root, searchString);
        }

        private List<T> SearchRec(BSTNode<T> root, string searchString)
        {
            if (root == null) return new List<T>();

            List<T> results = new List<T>();

            if (root.Values.Any(v => v.ToString().IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                results.AddRange(root.Values.Where(v => v.ToString().IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0));
            }

            results.AddRange(SearchRec(root.Left, searchString));
            results.AddRange(SearchRec(root.Right, searchString));

            return results;
        }
    }
}