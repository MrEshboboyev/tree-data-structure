using System.Text;

namespace TreeDataStructure;

/// <summary>
/// A generic Binary Search Tree implementation with comprehensive functionality
/// </summary>
/// <typeparam name="T">The type of data stored in the tree, must implement IComparable</typeparam>
public class BinarySearchTree<T> : AbstractTree<T> where T : IComparable<T>
{
    public TreeNode<T> Root => _root;
    public int Count => _count;
    public bool IsEmpty => _root == null;
    
    /// <summary>
    /// Gets the height of the tree
    /// </summary>
    public int Height => GetHeight(_root);

    public BinarySearchTree()
    {
        _root = null;
        _count = 0;
    }

    public BinarySearchTree(IEnumerable<T> values) : this()
    {
        if (values == null) throw new ArgumentNullException(nameof(values));
        
        foreach (var value in values)
        {
            Insert(value);
        }
    }

    public override void Insert(T value)
    {
        _root = InsertRecursive(_root, null, value);
        _count++;
    }

    private TreeNode<T> InsertRecursive(TreeNode<T> current, TreeNode<T> parent, T value)
    {
        if (current == null)
        {
            var newNode = new TreeNode<T>(value) { Parent = parent };
            return newNode;
        }

        int comparison = value.CompareTo(current.Data);
        if (comparison < 0)
        {
            current.Left = InsertRecursive(current.Left, current, value);
        }
        else if (comparison > 0)
        {
            current.Right = InsertRecursive(current.Right, current, value);
        }
        else
        {
            // Value already exists, decrement count as we incremented it before calling
            _count--;
            throw new InvalidOperationException($"Value {value} already exists in the tree.");
        }

        return current;
    }

    public override bool Remove(T value)
    {
        if (_root == null) return false;

        var result = RemoveRecursive(_root, value);
        if (result != null)
        {
            _root = result;
            _count--;
            return true;
        }
        return false;
    }

    private TreeNode<T> RemoveRecursive(TreeNode<T> current, T value)
    {
        if (current == null) return null;

        int comparison = value.CompareTo(current.Data);
        if (comparison < 0)
        {
            current.Left = RemoveRecursive(current.Left, value);
            return current;
        }
        if (comparison > 0)
        {
            current.Right = RemoveRecursive(current.Right, value);
            return current;
        }

        // Node to delete found
        if (current.Left == null && current.Right == null)
        {
            // Leaf node
            return null;
        }
        if (current.Left == null)
        {
            // Has only right child
            var rightChild = current.Right;
            rightChild.Parent = current.Parent;
            return rightChild;
        }
        if (current.Right == null)
        {
            // Has only left child
            var leftChild = current.Left;
            leftChild.Parent = current.Parent;
            return leftChild;
        }

        // Has both children - find inorder successor (smallest in right subtree)
        T successorValue = FindMin(current.Right).Data;
        current.Data = successorValue;
        current.Right = RemoveRecursive(current.Right, successorValue);
        return current;
    }

    public override bool Contains(T value)
    {
        return Find(value) != null;
    }

    public override TreeNode<T> Find(T value)
    {
        return FindRecursive(_root, value);
    }

    private TreeNode<T> FindRecursive(TreeNode<T> current, T value)
    {
        if (current == null) return null;

        int comparison = value.CompareTo(current.Data);
        if (comparison == 0) return current;
        if (comparison < 0) return FindRecursive(current.Left, value);
        return FindRecursive(current.Right, value);
    }

    public override void Clear()
    {
        _root = null;
        _count = 0;
    }

    public override T GetMin()
    {
        if (_root == null) throw new InvalidOperationException("Tree is empty.");
        return FindMin(_root).Data;
    }

    private TreeNode<T> FindMin(TreeNode<T> node)
    {
        while (node.Left != null)
        {
            node = node.Left;
        }
        return node;
    }

    public override T GetMax()
    {
        if (_root == null) throw new InvalidOperationException("Tree is empty.");
        return FindMax(_root).Data;
    }

    private TreeNode<T> FindMax(TreeNode<T> node)
    {
        while (node.Right != null)
        {
            node = node.Right;
        }
        return node;
    }

    public override IEnumerable<T> InOrderTraversal()
    {
        var result = new List<T>();
        InOrderTraversalRecursive(_root, result);
        return result;
    }

    private void InOrderTraversalRecursive(TreeNode<T> node, List<T> result)
    {
        if (node != null)
        {
            InOrderTraversalRecursive(node.Left, result);
            result.Add(node.Data);
            InOrderTraversalRecursive(node.Right, result);
        }
    }

    public override IEnumerable<T> PreOrderTraversal()
    {
        var result = new List<T>();
        PreOrderTraversalRecursive(_root, result);
        return result;
    }

    private void PreOrderTraversalRecursive(TreeNode<T> node, List<T> result)
    {
        if (node != null)
        {
            result.Add(node.Data);
            PreOrderTraversalRecursive(node.Left, result);
            PreOrderTraversalRecursive(node.Right, result);
        }
    }

    public override IEnumerable<T> PostOrderTraversal()
    {
        var result = new List<T>();
        PostOrderTraversalRecursive(_root, result);
        return result;
    }

    private void PostOrderTraversalRecursive(TreeNode<T> node, List<T> result)
    {
        if (node != null)
        {
            PostOrderTraversalRecursive(node.Left, result);
            PostOrderTraversalRecursive(node.Right, result);
            result.Add(node.Data);
        }
    }

    public override IEnumerable<T> LevelOrderTraversal()
    {
        var result = new List<T>();
        if (_root == null) return result;

        var queue = new Queue<TreeNode<T>>();
        queue.Enqueue(_root);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current.Data);

            if (current.Left != null) queue.Enqueue(current.Left);
            if (current.Right != null) queue.Enqueue(current.Right);
        }

        return result;
    }

    public override string Serialize()
    {
        if (_root == null) return "";
        return SerializeRecursive(_root);
    }

    private string SerializeRecursive(TreeNode<T> node)
    {
        if (node == null) return "null";
        
        var sb = new StringBuilder();
        sb.Append($"({node.Data},{SerializeRecursive(node.Left)},{SerializeRecursive(node.Right)})");
        return sb.ToString();
    }

    public override void Deserialize(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            Clear();
            return;
        }
        
        int index = 0;
        _root = DeserializeRecursive(data, ref index);
        _count = CountNodes(_root);
    }

    private TreeNode<T> DeserializeRecursive(string data, ref int index)
    {
        if (index >= data.Length || data.Substring(index).StartsWith("null"))
        {
            if (data.Substring(index).StartsWith("null"))
                index += 4;
            return null;
        }

        if (data[index] == '(')
        {
            index++; // Skip '('
            
            // Find the data value
            int start = index;
            while (index < data.Length && data[index] != ',' && data[index] != ')')
                index++;
            
            string valueStr = data.Substring(start, index - start);
            T value = (T)Convert.ChangeType(valueStr, typeof(T));
            
            var node = new TreeNode<T>(value);
            
            if (index < data.Length && data[index] == ',')
                index++; // Skip ','
            
            node.Left = DeserializeRecursive(data, ref index);
            
            if (index < data.Length && data[index] == ',')
                index++; // Skip ','
            
            node.Right = DeserializeRecursive(data, ref index);
            
            if (index < data.Length && data[index] == ')')
                index++; // Skip ')'
            
            return node;
        }
        
        return null;
    }
    
    private int CountNodes(TreeNode<T> node)
    {
        if (node == null) return 0;
        return 1 + CountNodes(node.Left) + CountNodes(node.Right);
    }

    private new int GetHeight(TreeNode<T> node)
    {
        if (node == null) return 0;
        return 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
    }

    public override ITree<T> Clone()
    {
        var clonedTree = new BinarySearchTree<T>();
        if (_root != null)
        {
            clonedTree._root = CloneRecursive(_root);
            clonedTree._count = _count;
        }
        return clonedTree;
    }
    
    private TreeNode<T> CloneRecursive(TreeNode<T> node)
    {
        if (node == null) return null;

        var clonedNode = new TreeNode<T>(node.Data);
        
        if (node.Left != null)
        {
            clonedNode.Left = CloneRecursive(node.Left);
            if (clonedNode.Left != null) clonedNode.Left.Parent = clonedNode;
        }
        
        if (node.Right != null)
        {
            clonedNode.Right = CloneRecursive(node.Right);
            if (clonedNode.Right != null) clonedNode.Right.Parent = clonedNode;
        }

        return clonedNode;
    }

    public override IEnumerator<T> GetEnumerator()
    {
        return InOrderTraversal().GetEnumerator();
    }
}
