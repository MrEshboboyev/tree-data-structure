using System.Collections;
using System.Text;

namespace TreeDataStructure;

/// <summary>
/// Represents a node in a B-Tree
/// </summary>
/// <typeparam name="T">The type of data stored in the node</typeparam>
public class BTreeNode<T> where T : IComparable<T>
{
    public List<T> Keys { get; set; }
    public List<BTreeNode<T>> Children { get; set; }
    public bool IsLeaf { get; set; }
    public int KeyCount { get; set; }

    public BTreeNode(bool isLeaf, int degree)
    {
        Keys = [.. new T[2 * degree - 1]];
        Children = [.. new BTreeNode<T>[2 * degree]];
        IsLeaf = isLeaf;
        KeyCount = 0;
        for (int i = 0; i < Keys.Capacity; i++) Keys[i] = default!;
        for (int i = 0; i < Children.Capacity; i++) Children[i] = null!;
    }
}

/// <summary>
/// A B-Tree implementation for efficient disk-based storage operations
/// </summary>
/// <typeparam name="T">The type of data stored in the tree, must implement IComparable</typeparam>
public class BTree<T> : ITree<T> where T : IComparable<T>
{
    private BTreeNode<T> _root;
    private readonly int _degree; // Minimum degree
    private int _count;

    public int Count => _count;
    public bool IsEmpty => _root == null;
    public int Height => GetHeight(_root);

    public BTree(int degree = 3)
    {
        if (degree < 2) throw new ArgumentException("Degree must be at least 2", nameof(degree));
        _root = null;
        _degree = degree;
        _count = 0;
    }

    public TreeNode<T> Root => throw new NotImplementedException("BTree doesn't use TreeNode<T> structure");

    private int GetHeight(BTreeNode<T> node)
    {
        if (node == null) return 0;
        if (node.IsLeaf) return 1;

        return 1 + GetHeight(node.Children[0]);
    }

    public void Insert(T value)
    {
        if (_root == null)
        {
            _root = new BTreeNode<T>(true, _degree);
            _root.Keys[0] = value;
            _root.KeyCount = 1;
            _count++;
        }
        else
        {
            if (IsFull(_root))
            {
                // Create a new root
                var newRoot = new BTreeNode<T>(false, _degree);
                newRoot.Children[0] = _root;
                SplitChild(newRoot, 0);
                _root = newRoot;
            }
            InsertNonFull(_root, value);
            _count++;
        }
    }

    private bool IsFull(BTreeNode<T> node)
    {
        return node.KeyCount == 2 * _degree - 1;
    }

    private void InsertNonFull(BTreeNode<T> x, T k)
    {
        int i = x.KeyCount - 1;

        if (x.IsLeaf)
        {
            // Find the location of the new key and move all greater keys one position ahead
            while (i >= 0 && x.Keys[i].CompareTo(k) > 0)
            {
                x.Keys[i + 1] = x.Keys[i];
                i--;
            }
            x.Keys[i + 1] = k;
            x.KeyCount++;
        }
        else
        {
            // Find the child which is going to have the new key
            while (i >= 0 && x.Keys[i].CompareTo(k) > 0)
                i--;

            i++;
            
            // If the found child is full, split it
            if (IsFull(x.Children[i]))
            {
                SplitChild(x, i);

                // After split, the middle key of x.Children[i] goes up to x and
                // x.Children[i] is split into two.  See which of the two is going
                // to have the new key
                if (x.Keys[i].CompareTo(k) < 0)
                    i++;
            }
            InsertNonFull(x.Children[i], k);
        }
    }

    private void SplitChild(BTreeNode<T> x, int i)
    {
        var y = x.Children[i];
        var z = new BTreeNode<T>(y.IsLeaf, _degree);
        z.KeyCount = _degree - 1;

        // Copy the last (degree-1) keys of y to z
        for (int j = 0; j < _degree - 1; j++)
            z.Keys[j] = y.Keys[j + _degree];

        // Copy the last degree children of y to z
        if (!y.IsLeaf)
        {
            for (int j = 0; j < _degree; j++)
                z.Children[j] = y.Children[j + _degree];
        }

        // Reduce the number of keys in y
        y.KeyCount = _degree - 1;

        // Since this node is going to have a new child, 
        // create space for new child
        for (int j = x.KeyCount; j >= i + 1; j--)
            x.Children[j + 1] = x.Children[j];

        // Link the new child to this node
        x.Children[i + 1] = z;

        // A key of y will move to this node. Find the location of
        // new key and move all greater keys one position ahead
        for (int j = x.KeyCount - 1; j >= i; j--)
            x.Keys[j + 1] = x.Keys[j];

        // Copy the middle key of y to this node
        x.Keys[i] = y.Keys[_degree - 1];

        // Increment count of keys in this node
        x.KeyCount = x.KeyCount + 1;
    }

    public bool Remove(T value)
    {
        if (_root == null) return false;

        bool removed = RemoveRecursive(_root, value);

        if (_root.KeyCount == 0)
        {
            // If root has 0 keys, make its first child as the new root if it exists
            if (!_root.IsLeaf)
                _root = _root.Children[0];
            else
                _root = null;
        }

        if (removed) _count--;
        return removed;
    }

    private bool RemoveRecursive(BTreeNode<T> x, T k)
    {
        int idx = FindKeyIndex(x, k);

        if (idx < x.KeyCount && x.Keys[idx].Equals(k))
        {
            // The key to be removed is present in this node
            if (x.IsLeaf)
            {
                // Case 1: The key is in a leaf node
                RemoveFromLeaf(x, idx);
                return true;
            }
            else
            {
                // Case 2: The key is in an internal node
                return RemoveFromNonLeaf(x, idx);
            }
        }
        else
        {
            // The key is not present in this node
            if (x.IsLeaf)
            {
                // Key is not present in the tree
                return false;
            }

            // Determine whether the child we descend to has at least degree keys
            bool flag = (idx == x.KeyCount) ? true : false;

            // If the child that we descend to has less than degree keys, 
            // make sure that it has at least degree keys
            if (x.Children[idx].KeyCount < _degree)
                Fill(x, idx);

            // If the last child has been merged, it must have merged with the previous
            // child and so we recurse on the (idx-1)th child. Else, we recurse on the
            // (idx)th child, which now has at least degree keys
            if (flag && idx > x.KeyCount)
                return RemoveRecursive(x.Children[idx - 1], k);
            else
                return RemoveRecursive(x.Children[idx], k);
        }
    }

    private int FindKeyIndex(BTreeNode<T> x, T k)
    {
        int idx = 0;
        while (idx < x.KeyCount && x.Keys[idx].CompareTo(k) < 0)
            ++idx;
        return idx;
    }

    private void RemoveFromLeaf(BTreeNode<T> x, int idx)
    {
        // Move all keys after idx one position back
        for (int i = idx + 1; i < x.KeyCount; ++i)
            x.Keys[i - 1] = x.Keys[i];

        x.KeyCount--;
    }

    private bool RemoveFromNonLeaf(BTreeNode<T> x, int idx)
    {
        T k = x.Keys[idx];

        // If the child that precedes k (x.Children[idx]) has at least degree keys,
        // find the predecessor 'pred' of k in the subtree rooted at x.Children[idx].
        // Replace k by pred and recursively delete pred in x.Children[idx]
        if (x.Children[idx].KeyCount >= _degree)
        {
            T pred = GetPredecessor(x, idx);
            x.Keys[idx] = pred;
            return RemoveRecursive(x.Children[idx], pred);
        }

        // If the child x.Children[idx] has less that degree keys, examine x.Children[idx+1].
        // If x.Children[idx+1] has at least degree keys, find the successor 'succ' of k in
        // the subtree rooted at x.Children[idx+1], replace k by succ and
        // recursively delete succ in x.Children[idx+1]
        else if (x.Children[idx + 1].KeyCount >= _degree)
        {
            T succ = GetSuccessor(x, idx);
            x.Keys[idx] = succ;
            return RemoveRecursive(x.Children[idx + 1], succ);
        }

        // If both x.Children[idx] and x.Children[idx+1] have less than degree keys,
        // merge k and all of x.Children[idx+1] into x.Children[idx].
        // Now x.Children[idx] contains 2*degree-1 keys
        // Free x.Children[idx+1] and recursively delete k from x.Children[idx]
        else
        {
            Merge(x, idx);
            return RemoveRecursive(x.Children[idx], k);
        }
    }

    private T GetPredecessor(BTreeNode<T> x, int idx)
    {
        var cur = x.Children[idx];
        while (!cur.IsLeaf)
            cur = cur.Children[cur.KeyCount];
        return cur.Keys[cur.KeyCount - 1];
    }

    private T GetSuccessor(BTreeNode<T> x, int idx)
    {
        var cur = x.Children[idx + 1];
        while (!cur.IsLeaf)
            cur = cur.Children[0];
        return cur.Keys[0];
    }

    private void Fill(BTreeNode<T> x, int idx)
    {
        // If the previous child has more than degree-1 keys, borrow from that child
        if (idx != 0 && x.Children[idx - 1].KeyCount >= _degree)
            BorrowFromPrev(x, idx);

        // If the next child has more than degree-1 keys, borrow from that child
        else if (idx != x.KeyCount && x.Children[idx + 1].KeyCount >= _degree)
            BorrowFromNext(x, idx);

        // Merge x.Children[idx] with its sibling
        // If x.Children[idx] is the last child, merge it with its previous sibling.
        // Otherwise merge it with its next sibling
        else
        {
            if (idx != x.KeyCount)
                Merge(x, idx);
            else
                Merge(x, idx - 1);
        }
    }

    private void BorrowFromPrev(BTreeNode<T> x, int idx)
    {
        var child = x.Children[idx];
        var sibling = x.Children[idx - 1];

        // The last key from x becomes the first key in child
        for (int i = child.KeyCount - 1; i >= 0; --i)
            child.Keys[i + 1] = child.Keys[i];

        // Copy the key from x to child
        child.Keys[0] = x.Keys[idx - 1];

        // If child is an internal node, move all its child pointers one position ahead
        if (!child.IsLeaf)
        {
            for (int i = child.KeyCount; i >= 0; --i)
                child.Children[i + 1] = child.Children[i];
        }

        // Copy the last child of sibling as the first child of child
        if (!sibling.IsLeaf)
            child.Children[0] = sibling.Children[sibling.KeyCount];

        // Move the key from sibling to x
        x.Keys[idx - 1] = sibling.Keys[sibling.KeyCount - 1];

        child.KeyCount += 1;
        sibling.KeyCount -= 1;
    }

    private void BorrowFromNext(BTreeNode<T> x, int idx)
    {
        var child = x.Children[idx];
        var sibling = x.Children[idx + 1];

        // child gets the first key from sibling
        child.Keys[child.KeyCount] = x.Keys[idx];

        // If sibling is not a leaf, first child of sibling becomes the last child of child
        if (!child.IsLeaf)
            child.Children[child.KeyCount + 1] = sibling.Children[0];

        // First key of sibling is inserted as the last key in child
        x.Keys[idx] = sibling.Keys[0];

        // Shift all keys in sibling to one position back
        for (int i = 1; i < sibling.KeyCount; ++i)
            sibling.Keys[i - 1] = sibling.Keys[i];

        // Shift all child pointers in sibling to one position back
        if (!sibling.IsLeaf)
        {
            for (int i = 1; i <= sibling.KeyCount; ++i)
                sibling.Children[i - 1] = sibling.Children[i];
        }

        child.KeyCount += 1;
        sibling.KeyCount -= 1;
    }

    private void Merge(BTreeNode<T> x, int idx)
    {
        var child = x.Children[idx];
        var sibling = x.Children[idx + 1];

        // Pull the key from x and insert it into child
        child.Keys[_degree - 1] = x.Keys[idx];

        // Copy keys from sibling to child
        for (int i = 0; i < sibling.KeyCount; ++i)
            child.Keys[i + _degree] = sibling.Keys[i];

        // Copy child pointers from sibling to child
        if (!sibling.IsLeaf)
        {
            for (int i = 0; i <= sibling.KeyCount; ++i)
                child.Children[i + _degree] = sibling.Children[i];
        }

        // Move all keys after idx in x one position back
        for (int i = idx + 1; i < x.KeyCount; ++i)
            x.Keys[i - 1] = x.Keys[i];

        // Move all child pointers after (idx+1) in x one position back
        for (int i = idx + 2; i <= x.KeyCount; ++i)
            x.Children[i - 1] = x.Children[i];

        child.KeyCount += sibling.KeyCount + 1;
        x.KeyCount--;

        // Free sibling
    }

    public bool Contains(T value)
    {
        return Search(_root, value) != null;
    }

    private BTreeNode<T> Search(BTreeNode<T> x, T k)
    {
        int i = 0;
        while (i < x.KeyCount && k.CompareTo(x.Keys[i]) > 0)
            i++;

        if (i < x.KeyCount && x.Keys[i].Equals(k))
            return x;

        if (x.IsLeaf)
            return null;

        return Search(x.Children[i], k);
    }

    public TreeNode<T> Find(T value)
    {
        // BTree doesn't use TreeNode<T>, so we return null for compatibility
        return Contains(value) ? new TreeNode<T>(value) : null;
    }

    public void Clear()
    {
        _root = null;
        _count = 0;
    }

    public IEnumerable<T> InOrderTraversal()
    {
        var result = new List<T>();
        InOrderTraversalRecursive(_root, result);
        return result;
    }

    private void InOrderTraversalRecursive(BTreeNode<T> node, List<T> result)
    {
        if (node != null)
        {
            int i;
            for (i = 0; i < node.KeyCount; i++)
            {
                if (!node.IsLeaf)
                    InOrderTraversalRecursive(node.Children[i], result);
                result.Add(node.Keys[i]);
            }

            if (!node.IsLeaf)
                InOrderTraversalRecursive(node.Children[i], result);
        }
    }

    public IEnumerable<T> PreOrderTraversal()
    {
        var result = new List<T>();
        PreOrderTraversalRecursive(_root, result);
        return result;
    }

    private void PreOrderTraversalRecursive(BTreeNode<T> node, List<T> result)
    {
        if (node != null)
        {
            for (int i = 0; i < node.KeyCount; i++)
            {
                result.Add(node.Keys[i]);
                if (!node.IsLeaf)
                    PreOrderTraversalRecursive(node.Children[i], result);
            }
            
            if (!node.IsLeaf)
                PreOrderTraversalRecursive(node.Children[node.KeyCount], result);
        }
    }

    public IEnumerable<T> PostOrderTraversal()
    {
        var result = new List<T>();
        PostOrderTraversalRecursive(_root, result);
        return result;
    }

    private void PostOrderTraversalRecursive(BTreeNode<T> node, List<T> result)
    {
        if (node != null)
        {
            if (!node.IsLeaf)
                PostOrderTraversalRecursive(node.Children[0], result);
            
            for (int i = 0; i < node.KeyCount - 1; i++)
            {
                if (!node.IsLeaf)
                    PostOrderTraversalRecursive(node.Children[i + 1], result);
                result.Add(node.Keys[i]);
            }
            
            if (node.KeyCount > 0)
                result.Add(node.Keys[node.KeyCount - 1]);
                
            if (!node.IsLeaf)
                PostOrderTraversalRecursive(node.Children[node.KeyCount], result);
        }
    }

    public IEnumerable<T> LevelOrderTraversal()
    {
        var result = new List<T>();
        if (_root == null) return result;

        var queue = new Queue<BTreeNode<T>>();
        queue.Enqueue(_root);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            
            for (int i = 0; i < current.KeyCount; i++)
                result.Add(current.Keys[i]);

            if (!current.IsLeaf)
            {
                for (int i = 0; i <= current.KeyCount; i++)
                {
                    if (current.Children[i] != null)
                        queue.Enqueue(current.Children[i]);
                }
            }
        }

        return result;
    }

    public T GetMin()
    {
        if (_root == null) throw new InvalidOperationException("Tree is empty.");
        var current = _root;
        while (!current.IsLeaf)
            current = current.Children[0];
        return current.Keys[0];
    }

    public T GetMax()
    {
        if (_root == null) throw new InvalidOperationException("Tree is empty.");
        var current = _root;
        while (!current.IsLeaf)
            current = current.Children[current.KeyCount];
        return current.Keys[current.KeyCount - 1];
    }

    public string Serialize()
    {
        if (_root == null) return "";
        return SerializeRecursive(_root);
    }

    private string SerializeRecursive(BTreeNode<T> node)
    {
        if (node == null) return "null";
        
        var sb = new StringBuilder();
        sb.Append($"({string.Join(",", node.Keys.Take(node.KeyCount))},{node.IsLeaf},{string.Join(";", node.Children.Take(node.KeyCount + 1).Select(child => SerializeRecursive(child)))})");
        return sb.ToString();
    }

    public void Deserialize(string data)
    {
        throw new NotImplementedException("BTree deserialization is complex and requires a more specialized implementation");
    }

    public ITree<T> Clone()
    {
        throw new NotImplementedException("BTree cloning is complex and requires a more specialized implementation");
    }

    public bool IsBalanced()
    {
        // B-trees are always balanced by definition
        return true;
    }

    public IEnumerable<T> GetPathToValue(T value)
    {
        throw new NotImplementedException();
    }

    public TreeNode<T> BreadthFirstSearch(T value)
    {
        throw new NotImplementedException();
    }

    public TreeNode<T> DepthFirstSearch(T value)
    {
        throw new NotImplementedException();
    }

    public TreeNode<T> GetLowestCommonAncestor(T value1, T value2)
    {
        throw new NotImplementedException();
    }

    public bool IsValidBST()
    {
        throw new NotImplementedException();
    }

    public T GetKthSmallest(int k)
    {
        if (k <= 0 || k > Count) throw new ArgumentOutOfRangeException(nameof(k));
        var sortedList = InOrderTraversal().ToList();
        return sortedList[k - 1];
    }

    public T GetKthLargest(int k)
    {
        if (k <= 0 || k > Count) throw new ArgumentOutOfRangeException(nameof(k));
        var sortedList = InOrderTraversal().ToList();
        return sortedList[sortedList.Count - k];
    }

    public void InsertRange(IEnumerable<T> values)
    {
        foreach (var value in values)
        {
            Insert(value);
        }
    }

    public IEnumerable<T> ReverseInOrderTraversal()
    {
        var result = new List<T>();
        ReverseInOrderTraversalRecursive(_root, result);
        return result;
    }

    private void ReverseInOrderTraversalRecursive(BTreeNode<T> node, List<T> result)
    {
        if (node != null)
        {
            int i;
            for (i = node.KeyCount - 1; i >= 0; i--)
            {
                if (!node.IsLeaf)
                    ReverseInOrderTraversalRecursive(node.Children[i + 1], result);
                result.Add(node.Keys[i]);
            }

            if (!node.IsLeaf)
                ReverseInOrderTraversalRecursive(node.Children[0], result);
        }
    }

    public int GetDepth(T value)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> GetValuesAtLevel(int level)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return InOrderTraversal().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
