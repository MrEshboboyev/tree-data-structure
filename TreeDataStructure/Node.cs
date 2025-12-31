namespace TreeDataStructure;

/// <summary>
/// Represents a generic node in a tree data structure
/// </summary>
/// <typeparam name="T">The type of data stored in the node</typeparam>
public class TreeNode<T>(T data)
{
    public T Data { get; set; } = data;
    public TreeNode<T>? Left { get; set; } = null;
    public TreeNode<T>? Right { get; set; } = null;
    public TreeNode<T>? Parent { get; set; } = null;
}
