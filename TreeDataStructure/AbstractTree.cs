using System.Collections;
using System.Text;

namespace TreeDataStructure;

/// <summary>
/// Abstract base class for tree implementations providing common functionality
/// </summary>
/// <typeparam name="T">The type of data stored in the tree</typeparam>
public abstract class AbstractTree<T> : ITree<T> where T : IComparable<T>
{
    protected TreeNode<T>? _root;
    protected int _count;

    public TreeNode<T> Root => _root!;
    public int Count => _count;
    public bool IsEmpty => _root == null;
    public int Height => GetHeight(_root!);

    public abstract void Insert(T value);
    
    public virtual void InsertRange(IEnumerable<T> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        
        foreach (var value in values)
        {
            Insert(value);
        }
    }

    public abstract bool Remove(T value);
    public abstract bool Contains(T value);
    public abstract TreeNode<T> Find(T value);
    public abstract void Clear();

    public abstract IEnumerable<T> InOrderTraversal();
    public abstract IEnumerable<T> PreOrderTraversal();
    public abstract IEnumerable<T> PostOrderTraversal();
    public abstract IEnumerable<T> LevelOrderTraversal();

    public virtual IEnumerable<T> ReverseInOrderTraversal()
    {
        var result = new List<T>();
        ReverseInOrderTraversalRecursive(_root, result);
        return result;
    }

    private static void ReverseInOrderTraversalRecursive(TreeNode<T> node, List<T> result)
    {
        if (node != null)
        {
            ReverseInOrderTraversalRecursive(node.Right, result);
            result.Add(node.Data);
            ReverseInOrderTraversalRecursive(node.Left, result);
        }
    }

    public abstract T GetMin();
    public abstract T GetMax();

    public virtual int GetDepth(T value)
    {
        var node = Find(value);
        if (node == null) return -1;

        int depth = 0;
        var current = node;
        while (current.Parent != null)
        {
            current = current.Parent;
            depth++;
        }
        return depth;
    }

    public virtual IEnumerable<T> GetValuesAtLevel(int level)
    {
        if (_root == null || level < 0) return new List<T>();

        var result = new List<T>();
        GetValuesAtLevelRecursive(_root, level, 0, result);
        return result;
    }

    private static void GetValuesAtLevelRecursive(TreeNode<T> node, int targetLevel, int currentLevel, List<T> result)
    {
        if (node == null) return;

        if (currentLevel == targetLevel)
        {
            result.Add(node.Data);
            return;
        }

        if (currentLevel < targetLevel)
        {
            GetValuesAtLevelRecursive(node.Left, targetLevel, currentLevel + 1, result);
            GetValuesAtLevelRecursive(node.Right, targetLevel, currentLevel + 1, result);
        }
    }

    public abstract string Serialize();
    public abstract void Deserialize(string data);

    public abstract ITree<T> Clone();

    public virtual bool IsBalanced()
    {
        return IsBalancedRecursive(_root).IsBalanced;
    }

    private (bool IsBalanced, int Height) IsBalancedRecursive(TreeNode<T> node)
    {
        if (node == null) return (true, 0);

        var leftResult = IsBalancedRecursive(node.Left);
        var rightResult = IsBalancedRecursive(node.Right);

        bool isBalanced = leftResult.IsBalanced && 
                          rightResult.IsBalanced && 
                          Math.Abs(leftResult.Height - rightResult.Height) <= 1;

        int height = 1 + Math.Max(leftResult.Height, rightResult.Height);

        return (isBalanced, height);
    }

    public virtual IEnumerable<T> GetPathToValue(T value)
    {
        var path = new List<T>();
        if (FindPathToValue(_root, value, path))
        {
            return path;
        }
        return new List<T>();
    }

    private bool FindPathToValue(TreeNode<T> node, T value, List<T> path)
    {
        if (node == null) return false;

        path.Add(node.Data);

        if (node.Data.CompareTo(value) == 0)
        {
            return true;
        }

        if (value.CompareTo(node.Data) < 0)
        {
            if (FindPathToValue(node.Left, value, path)) return true;
        }
        else
        {
            if (FindPathToValue(node.Right, value, path)) return true;
        }

        path.RemoveAt(path.Count - 1);
        return false;
    }

    public virtual TreeNode<T> BreadthFirstSearch(T value)
    {
        if (_root == null) return null;

        var queue = new Queue<TreeNode<T>>();
        queue.Enqueue(_root);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.Data.CompareTo(value) == 0)
            {
                return current;
            }

            if (current.Left != null) queue.Enqueue(current.Left);
            if (current.Right != null) queue.Enqueue(current.Right);
        }

        return null;
    }

    public virtual TreeNode<T> DepthFirstSearch(T value)
    {
        return Find(value); // For BST, regular find is DFS
    }

    public virtual TreeNode<T> GetLowestCommonAncestor(T value1, T value2)
    {
        if (_root == null) return null;

        var node1 = Find(value1);
        var node2 = Find(value2);

        if (node1 == null || node2 == null) return null;

        return GetLowestCommonAncestorRecursive(_root, node1, node2);
    }

    private TreeNode<T> GetLowestCommonAncestorRecursive(TreeNode<T> root, TreeNode<T> node1, TreeNode<T> node2)
    {
        if (root == null) return null;

        if (root.Data.CompareTo(node1.Data) > 0 && root.Data.CompareTo(node2.Data) > 0)
        {
            return GetLowestCommonAncestorRecursive(root.Left, node1, node2);
        }

        if (root.Data.CompareTo(node1.Data) < 0 && root.Data.CompareTo(node2.Data) < 0)
        {
            return GetLowestCommonAncestorRecursive(root.Right, node1, node2);
        }

        return root;
    }

    public virtual bool IsValidBST()
    {
        return IsValidBSTRecursive(_root, default(T), default(T), false, false);
    }

    private bool IsValidBSTRecursive(TreeNode<T> node, T min, T max, bool hasMin, bool hasMax)
    {
        if (node == null) return true;

        if (hasMin && node.Data.CompareTo(min) <= 0) return false;
        if (hasMax && node.Data.CompareTo(max) >= 0) return false;

        return IsValidBSTRecursive(node.Left, min, node.Data, hasMin, true) &&
               IsValidBSTRecursive(node.Right, node.Data, max, true, hasMax);
    }

    public virtual T GetKthSmallest(int k)
    {
        if (k <= 0 || k > Count) throw new ArgumentOutOfRangeException(nameof(k));

        var result = new List<T>();
        InOrderTraversalKthSmallest(_root, k, result);
        return result[k - 1];
    }

    private void InOrderTraversalKthSmallest(TreeNode<T> node, int k, List<T> result)
    {
        if (node != null && result.Count < k)
        {
            InOrderTraversalKthSmallest(node.Left, k, result);
            if (result.Count < k)
            {
                result.Add(node.Data);
            }
            InOrderTraversalKthSmallest(node.Right, k, result);
        }
    }

    public virtual T GetKthLargest(int k)
    {
        if (k <= 0 || k > Count) throw new ArgumentOutOfRangeException(nameof(k));

        var result = new List<T>();
        ReverseInOrderTraversalKthLargest(_root, k, result);
        return result[k - 1];
    }

    private void ReverseInOrderTraversalKthLargest(TreeNode<T> node, int k, List<T> result)
    {
        if (node != null && result.Count < k)
        {
            ReverseInOrderTraversalKthLargest(node.Right, k, result);
            if (result.Count < k)
            {
                result.Add(node.Data);
            }
            ReverseInOrderTraversalKthLargest(node.Left, k, result);
        }
    }

    protected int GetHeight(TreeNode<T> node)
    {
        if (node == null) return 0;
        return 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
    }

    public virtual IEnumerator<T> GetEnumerator()
    {
        return InOrderTraversal().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
