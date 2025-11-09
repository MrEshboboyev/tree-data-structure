namespace TreeDataStructure;

public class Node(int value)
{
    public int Value { get; set; } = value;

    public Node Left { get; set; } = null!;

    public Node Right { get; set; } = null!;
}
