namespace TreeDataStructure;

/// <summary>
/// Enum representing different types of tree implementations
/// </summary>
public enum TreeType
{
    BinarySearchTree,
    AvlTree,
    RedBlackTree,
    BTree
}

/// <summary>
/// Factory class for creating different types of tree implementations
/// </summary>
public static class TreeFactory
{
    /// <summary>
    /// Creates a new tree instance based on the specified type
    /// </summary>
    /// <typeparam name="T">The type of data stored in the tree</typeparam>
    /// <param name="treeType">The type of tree to create</param>
    /// <param name="parameters">Optional parameters for tree creation (e.g., degree for B-Tree)</param>
    /// <returns>A new instance of the specified tree type</returns>
    public static ITree<T> CreateTree<T>(TreeType treeType, params object[] parameters) where T : IComparable<T>
    {
        return treeType switch
        {
            TreeType.BinarySearchTree => new BinarySearchTree<T>(),
            TreeType.AvlTree => new AvlTree<T>(),
            TreeType.RedBlackTree => new RedBlackTree<T>(),
            TreeType.BTree => parameters.Length > 0 ? 
                new BTree<T>(parameters[0] is int degree ? degree : 3) : 
                new BTree<T>(3), // Default degree of 3
            _ => throw new ArgumentException($"Unsupported tree type: {treeType}", nameof(treeType))
        };
    }

    /// <summary>
    /// Creates a new tree instance with initial values based on the specified type
    /// </summary>
    /// <typeparam name="T">The type of data stored in the tree</typeparam>
    /// <param name="treeType">The type of tree to create</param>
    /// <param name="values">Initial values to insert into the tree</param>
    /// <param name="parameters">Optional parameters for tree creation (e.g., degree for B-Tree)</param>
    /// <returns>A new instance of the specified tree type with initial values</returns>
    public static ITree<T> CreateTree<T>(TreeType treeType, IEnumerable<T> values, params object[] parameters) where T : IComparable<T>
    {
        var tree = CreateTree<T>(treeType, parameters);
        tree.InsertRange(values);
        return tree;
    }
}
