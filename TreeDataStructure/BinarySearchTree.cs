namespace TreeDataStructure;

public class BinarySearchTree
{
    public Node Root { get; private set; }

    public void Insert(int value)
    {
        Root = InsertRecursive(Root, value);
    }

    public static void InOrderTraversal(Node node)
    {
        if (node != null)
        {
            InOrderTraversal(node.Left);

            Console.Write(node.Value + " ");

            InOrderTraversal(node.Right);
        }
    }

    private static Node InsertRecursive(Node current, int value)
    {
        if (current == null)
        {
            return new Node(value);
        }

        if (value < current.Value)
        {
            current.Left = InsertRecursive(current.Left, value);
        }
        else if (value > current.Value)
        {
            current.Right = InsertRecursive(current.Right, value);
        }

        return current;
    }
}
