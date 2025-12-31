using System.Text;

namespace TreeDataStructure;

/// <summary>
/// Represents a node in a Red-Black tree
/// </summary>
/// <typeparam name="T">The type of data stored in the node</typeparam>
public class RedBlackTreeNode<T>(T data) : TreeNode<T>(data)
    where T : IComparable<T>
{
    public bool IsRed { get; set; } = true; // New nodes are always red initially
}

/// <summary>
/// A self-balancing Red-Black tree implementation
/// </summary>
/// <typeparam name="T">The type of data stored in the tree, must implement IComparable</typeparam>
public class RedBlackTree<T> : AbstractTree<T> where T : IComparable<T>
{
    private new RedBlackTreeNode<T>? _root;

    public new RedBlackTreeNode<T> Root => _root!;

    public RedBlackTree()
    {
        _root = null;
        _count = 0;
    }

    public RedBlackTree(IEnumerable<T> values) : this()
    {
        ArgumentNullException.ThrowIfNull(values);

        foreach (var value in values)
        {
            Insert(value);
        }
    }

    public override void Insert(T value)
    {
        _root = InsertRecursive(_root, value);
        _root.IsRed = false; // Root is always black
        _count++;
    }

    private RedBlackTreeNode<T> InsertRecursive(RedBlackTreeNode<T>? node, T value)
    {
        if (node == null)
        {
            return new RedBlackTreeNode<T>(value);
        }

        int comparison = value.CompareTo(node.Data);
        if (comparison < 0)
        {
            node.Left = InsertRecursive((RedBlackTreeNode<T>?)node.Left, value);
        }
        else if (comparison > 0)
        {
            node.Right = InsertRecursive((RedBlackTreeNode<T>?)node.Right, value);
        }
        else
        {
            // Value already exists, decrement count as we incremented it before calling
            _count--;
            throw new InvalidOperationException($"Value {value} already exists in the tree.");
        }

        // Fix Red-Black properties
        return BalanceAfterInsert(node, value);
    }

    private RedBlackTreeNode<T> BalanceAfterInsert(RedBlackTreeNode<T> node, T value)
    {
        // If current node is black, no violations
        if (!node.IsRed)
            return node;

        // If current node is red and parent is also red, we have a violation
        var parent = (RedBlackTreeNode<T>?)node.Parent;
        if (parent == null) return node; // Root case

        // Check if parent is red (which would violate Red-Black property)
        if (parent.IsRed)
        {
            var grandParent = (RedBlackTreeNode<T>?)parent.Parent;
            if (grandParent == null) return node; // If parent is root

            // Determine if we need to rotate
            if (grandParent.Left == parent)
            {
                var uncle = (RedBlackTreeNode<T>?)grandParent.Right;
                if (uncle != null && uncle.IsRed)
                {
                    // Case 1: Uncle is red - recolor
                    parent.IsRed = false;
                    uncle.IsRed = false;
                    grandParent.IsRed = true;
                    return grandParent;
                }
                else
                {
                    // Uncle is black - rotation needed
                    if (parent.Right == node)
                    {
                        // Left-Right case
                        parent = RotateLeft(parent);
                        grandParent.Left = parent;
                        parent.Parent = grandParent;
                    }
                    // Left-Left case
                    grandParent = RotateRight(grandParent);
                    grandParent.IsRed = false;
                    parent.IsRed = true;
                    return grandParent;
                }
            }
            else
            {
                var uncle = (RedBlackTreeNode<T>?)grandParent.Left;
                if (uncle != null && uncle.IsRed)
                {
                    // Case 1: Uncle is red - recolor
                    parent.IsRed = false;
                    uncle.IsRed = false;
                    grandParent.IsRed = true;
                    return grandParent;
                }
                else
                {
                    // Uncle is black - rotation needed
                    if (parent.Left == node)
                    {
                        // Right-Left case
                        parent = RotateRight(parent);
                        grandParent.Right = parent;
                        parent.Parent = grandParent;
                    }
                    // Right-Right case
                    grandParent = RotateLeft(grandParent);
                    grandParent.IsRed = false;
                    parent.IsRed = true;
                    return grandParent;
                }
            }
        }

        return node;
    }

    public override bool Remove(T value)
    {
        int initialCount = _count;
        _root = RemoveRecursive(_root, value);
        if (_root != null) _root.IsRed = false; // Root is always black
        return _count != initialCount;
    }

    private RedBlackTreeNode<T>? RemoveRecursive(RedBlackTreeNode<T>? node, T value)
    {
        if (node == null) return null;

        int comparison = value.CompareTo(node.Data);
        if (comparison < 0)
        {
            node.Left = RemoveRecursive((RedBlackTreeNode<T>?)node.Left, value);
        }
        else if (comparison > 0)
        {
            node.Right = RemoveRecursive((RedBlackTreeNode<T>?)node.Right, value);
        }
        else
        {
            // Node to delete found
            _count--;

            // Case 1: Node has no children
            if (node.Left == null && node.Right == null)
            {
                return null;
            }
            // Case 2: Node has one child
            else if (node.Left == null)
            {
                var rightChild = (RedBlackTreeNode<T>?)node.Right;
                rightChild.Parent = node.Parent;
                return rightChild;
            }
            else if (node.Right == null)
            {
                var leftChild = (RedBlackTreeNode<T>?)node.Left;
                leftChild.Parent = node.Parent;
                return leftChild;
            }
            // Case 3: Node has two children
            else
            {
                // Find inorder successor (smallest in right subtree)
                T successorValue = FindMin((RedBlackTreeNode<T>?)node.Right).Data;
                node.Data = successorValue;
                node.Right = RemoveRecursive((RedBlackTreeNode<T>?)node.Right, successorValue);
            }
        }

        return node;
    }

    public override bool Contains(T value)
    {
        return Find(value) != null;
    }

    public override TreeNode<T> Find(T value)
    {
        return FindRecursive(_root, value);
    }

    private RedBlackTreeNode<T>? FindRecursive(RedBlackTreeNode<T>? current, T value)
    {
        if (current == null) return null;

        int comparison = value.CompareTo(current.Data);
        if (comparison == 0) return current;
        if (comparison < 0) return FindRecursive((RedBlackTreeNode<T>?)current.Left, value);
        return FindRecursive((RedBlackTreeNode<T>?)current.Right, value);
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

    private RedBlackTreeNode<T> FindMin(RedBlackTreeNode<T>? node)
    {
        while (node?.Left != null)
        {
            node = (RedBlackTreeNode<T>?)node.Left;
        }
        return node;
    }

    public override T GetMax()
    {
        if (_root == null) throw new InvalidOperationException("Tree is empty.");
        return FindMax(_root).Data;
    }

    private RedBlackTreeNode<T> FindMax(RedBlackTreeNode<T>? node)
    {
        while (node?.Right != null)
        {
            node = (RedBlackTreeNode<T>?)node.Right;
        }
        return node;
    }

    public override IEnumerable<T> InOrderTraversal()
    {
        var result = new List<T>();
        InOrderTraversalRecursive(_root, result);
        return result;
    }

    private void InOrderTraversalRecursive(RedBlackTreeNode<T>? node, List<T> result)
    {
        if (node != null)
        {
            InOrderTraversalRecursive((RedBlackTreeNode<T>?)node.Left, result);
            result.Add(node.Data);
            InOrderTraversalRecursive((RedBlackTreeNode<T>?)node.Right, result);
        }
    }

    public override IEnumerable<T> PreOrderTraversal()
    {
        var result = new List<T>();
        PreOrderTraversalRecursive(_root, result);
        return result;
    }

    private void PreOrderTraversalRecursive(RedBlackTreeNode<T>? node, List<T> result)
    {
        if (node != null)
        {
            result.Add(node.Data);
            PreOrderTraversalRecursive((RedBlackTreeNode<T>?)node.Left, result);
            PreOrderTraversalRecursive((RedBlackTreeNode<T>?)node.Right, result);
        }
    }

    public override IEnumerable<T> PostOrderTraversal()
    {
        var result = new List<T>();
        PostOrderTraversalRecursive(_root, result);
        return result;
    }

    private void PostOrderTraversalRecursive(RedBlackTreeNode<T>? node, List<T> result)
    {
        if (node != null)
        {
            PostOrderTraversalRecursive((RedBlackTreeNode<T>?)node.Left, result);
            PostOrderTraversalRecursive((RedBlackTreeNode<T>?)node.Right, result);
            result.Add(node.Data);
        }
    }

    public override IEnumerable<T> LevelOrderTraversal()
    {
        var result = new List<T>();
        if (_root == null) return result;

        var queue = new Queue<RedBlackTreeNode<T>>();
        queue.Enqueue(_root);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current.Data);

            if (current.Left != null) queue.Enqueue((RedBlackTreeNode<T>)current.Left);
            if (current.Right != null) queue.Enqueue((RedBlackTreeNode<T>)current.Right);
        }

        return result;
    }

    public override string Serialize()
    {
        if (_root == null) return "";
        return SerializeRecursive(_root);
    }

    private string SerializeRecursive(RedBlackTreeNode<T>? node)
    {
        if (node == null) return "null";
        
        var sb = new StringBuilder();
        sb.Append($"({node.Data},{(node.IsRed ? "R" : "B")},{SerializeRecursive((RedBlackTreeNode<T>?)node.Left)},{SerializeRecursive((RedBlackTreeNode<T>?)node.Right)})");
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

    private RedBlackTreeNode<T>? DeserializeRecursive(string data, ref int index)
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
            
            index++; // Skip ','
            
            // Get color
            bool isRed = data[index] == 'R';
            index += 2; // Skip color and ','
            
            var node = new RedBlackTreeNode<T>(value) { IsRed = isRed };
            
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
    
    private int CountNodes(RedBlackTreeNode<T>? node)
    {
        if (node == null) return 0;
        return 1 + CountNodes((RedBlackTreeNode<T>?)node.Left) + CountNodes((RedBlackTreeNode<T>?)node.Right);
    }

    public override ITree<T> Clone()
    {
        var clonedTree = new RedBlackTree<T>();
        if (_root != null)
        {
            clonedTree._root = CloneRecursive(_root);
            clonedTree._count = _count;
        }
        return clonedTree;
    }

    private RedBlackTreeNode<T>? CloneRecursive(RedBlackTreeNode<T>? node)
    {
        if (node == null) return null;

        var clonedNode = new RedBlackTreeNode<T>(node.Data) { IsRed = node.IsRed };
        
        if (node.Left != null)
        {
            clonedNode.Left = CloneRecursive((RedBlackTreeNode<T>?)node.Left);
            ((RedBlackTreeNode<T>?)clonedNode.Left).Parent = clonedNode;
        }
        
        if (node.Right != null)
        {
            clonedNode.Right = CloneRecursive((RedBlackTreeNode<T>?)node.Right);
            ((RedBlackTreeNode<T>?)clonedNode.Right).Parent = clonedNode;
        }

        return clonedNode;
    }

    private RedBlackTreeNode<T> RotateLeft(RedBlackTreeNode<T> x)
    {
        var y = (RedBlackTreeNode<T>?)x.Right;
        var T2 = y?.Left;

        // Perform rotation
        if (y != null) y.Left = x;
        x.Right = T2;

        // Update parent references
        if (T2 != null) T2.Parent = x;

        var originalParent = x.Parent;
        if (y != null) y.Parent = originalParent;
        x.Parent = y;

        // Update root if needed
        if (originalParent == null)
            _root = y;
        else if (originalParent.Left == x)
            originalParent.Left = y;
        else
            originalParent.Right = y;

        return y;
    }

    private RedBlackTreeNode<T> RotateRight(RedBlackTreeNode<T> y)
    {
        var x = (RedBlackTreeNode<T>?)y.Left;
        var T2 = x?.Right;

        // Perform rotation
        if (x != null) x.Right = y;
        y.Left = T2;

        // Update parent references
        if (T2 != null) T2.Parent = y;

        var originalParent = y.Parent;
        if (x != null) x.Parent = originalParent;
        y.Parent = x;

        // Update root if needed
        if (originalParent == null)
            _root = x;
        else if (originalParent.Left == y)
            originalParent.Left = x;
        else
            originalParent.Right = x;

        return x;
    }
}
