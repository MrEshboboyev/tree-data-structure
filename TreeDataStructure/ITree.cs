namespace TreeDataStructure;

/// <summary>
/// Interface for a generic tree data structure with comprehensive operations
/// </summary>
/// <typeparam name="T">The type of data stored in the tree</typeparam>
public interface ITree<T> : IEnumerable<T>
{
    /// <summary>
    /// Gets the number of elements in the tree
    /// </summary>
    int Count { get; }
    
    /// <summary>
    /// Gets the root node of the tree
    /// </summary>
    TreeNode<T> Root { get; }
    
    /// <summary>
    /// Gets whether the tree is empty
    /// </summary>
    bool IsEmpty { get; }
    
    /// <summary>
    /// Gets the height of the tree
    /// </summary>
    int Height { get; }
    
    /// <summary>
    /// Inserts a value into the tree
    /// </summary>
    /// <param name="value">The value to insert</param>
    void Insert(T value);
    
    /// <summary>
    /// Inserts multiple values into the tree
    /// </summary>
    /// <param name="values">The values to insert</param>
    void InsertRange(IEnumerable<T> values);
    
    /// <summary>
    /// Removes a value from the tree
    /// </summary>
    /// <param name="value">The value to remove</param>
    /// <returns>True if the value was found and removed, false otherwise</returns>
    bool Remove(T value);
    
    /// <summary>
    /// Checks if a value exists in the tree
    /// </summary>
    /// <param name="value">The value to search for</param>
    /// <returns>True if the value exists, false otherwise</returns>
    bool Contains(T value);
    
    /// <summary>
    /// Finds a node with the specified value
    /// </summary>
    /// <param name="value">The value to find</param>
    /// <returns>The node containing the value, or null if not found</returns>
    TreeNode<T> Find(T value);
    
    /// <summary>
    /// Clears all elements from the tree
    /// </summary>
    void Clear();
    
    /// <summary>
    /// Performs in-order traversal and returns the results
    /// </summary>
    /// <returns>Enumeration of values in in-order sequence</returns>
    IEnumerable<T> InOrderTraversal();
    
    /// <summary>
    /// Performs pre-order traversal and returns the results
    /// </summary>
    /// <returns>Enumeration of values in pre-order sequence</returns>
    IEnumerable<T> PreOrderTraversal();
    
    /// <summary>
    /// Performs post-order traversal and returns the results
    /// </summary>
    /// <returns>Enumeration of values in post-order sequence</returns>
    IEnumerable<T> PostOrderTraversal();
    
    /// <summary>
    /// Performs level-order traversal and returns the results
    /// </summary>
    /// <returns>Enumeration of values in level-order sequence</returns>
    IEnumerable<T> LevelOrderTraversal();
    
    /// <summary>
    /// Performs reverse in-order traversal (right-root-left)
    /// </summary>
    /// <returns>Enumeration of values in reverse in-order sequence</returns>
    IEnumerable<T> ReverseInOrderTraversal();
    
    /// <summary>
    /// Gets the minimum value in the tree
    /// </summary>
    /// <returns>The minimum value, or default(T) if tree is empty</returns>
    T GetMin();
    
    /// <summary>
    /// Gets the maximum value in the tree
    /// </summary>
    /// <returns>The maximum value, or default(T) if tree is empty</returns>
    T GetMax();
    
    /// <summary>
    /// Gets the depth of a specific value in the tree
    /// </summary>
    /// <param name="value">The value to find the depth for</param>
    /// <returns>The depth of the value, or -1 if not found</returns>
    int GetDepth(T value);
    
    /// <summary>
    /// Gets all values at a specific level in the tree
    /// </summary>
    /// <param name="level">The level to retrieve values from</param>
    /// <returns>Enumeration of values at the specified level</returns>
    IEnumerable<T> GetValuesAtLevel(int level);
    
    /// <summary>
    /// Serializes the tree to a string representation
    /// </summary>
    /// <returns>String representation of the tree</returns>
    string Serialize();
    
    /// <summary>
    /// Deserializes a tree from a string representation
    /// </summary>
    /// <param name="data">The string representation to deserialize from</param>
    void Deserialize(string data);
    
    /// <summary>
    /// Clones the tree structure
    /// </summary>
    /// <returns>A deep copy of the tree</returns>
    ITree<T> Clone();
    
    /// <summary>
    /// Checks if the tree is balanced
    /// </summary>
    /// <returns>True if the tree is balanced, false otherwise</returns>
    bool IsBalanced();
    
    /// <summary>
    /// Gets the path from root to a specific value
    /// </summary>
    /// <param name="value">The value to find the path for</param>
    /// <returns>Enumeration of values representing the path from root to the target value</returns>
    IEnumerable<T> GetPathToValue(T value);
    
    /// <summary>
    /// Performs a breadth-first search for a value
    /// </summary>
    /// <param name="value">The value to search for</param>
    /// <returns>The node containing the value, or null if not found</returns>
    TreeNode<T> BreadthFirstSearch(T value);
    
    /// <summary>
    /// Performs a depth-first search for a value
    /// </summary>
    /// <param name="value">The value to search for</param>
    /// <returns>The node containing the value, or null if not found</returns>
    TreeNode<T> DepthFirstSearch(T value);
    
    /// <summary>
    /// Gets the lowest common ancestor of two values
    /// </summary>
    /// <param name="value1">First value</param>
    /// <param name="value2">Second value</param>
    /// <returns>The lowest common ancestor node, or null if either value is not found</returns>
    TreeNode<T> GetLowestCommonAncestor(T value1, T value2);
    
    /// <summary>
    /// Checks if the tree is a valid binary search tree
    /// </summary>
    /// <returns>True if the tree is a valid BST, false otherwise</returns>
    bool IsValidBST();
    
    /// <summary>
    /// Gets the kth smallest element in the tree
    /// </summary>
    /// <param name="k">The index of the element to find (1-indexed)</param>
    /// <returns>The kth smallest element</returns>
    T GetKthSmallest(int k);
    
    /// <summary>
    /// Gets the kth largest element in the tree
    /// </summary>
    /// <param name="k">The index of the element to find (1-indexed)</param>
    /// <returns>The kth largest element</returns>
    T GetKthLargest(int k);
}
