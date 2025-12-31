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
    /// Serializes the tree to a string representation
    /// </summary>
    /// <returns>String representation of the tree</returns>
    string Serialize();
    
    /// <summary>
    /// Deserializes a tree from a string representation
    /// </summary>
    /// <param name="data">The string representation to deserialize from</param>
    void Deserialize(string data);
}
