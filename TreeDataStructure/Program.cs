using TreeDataStructure;

// Demonstrate the new comprehensive tree data structure implementation
Console.WriteLine("=== Enterprise Tree Data Structure Implementation ===\n");

// 1. Binary Search Tree with integer values
Console.WriteLine("1. Binary Search Tree (Generic) with integers:");
var intBst = new BinarySearchTree<int>();

int[] values = { 50, 30, 70, 20, 40, 60, 80, 10, 25, 35, 45 };
foreach (int value in values)
{
    intBst.Insert(value);
}

Console.WriteLine($"Tree height: {intBst.Height}");
Console.WriteLine($"Tree count: {intBst.Count}");
Console.WriteLine($"Min value: {intBst.GetMin()}");
Console.WriteLine($"Max value: {intBst.GetMax()}");
Console.WriteLine($"Is empty: {intBst.IsEmpty}");

Console.WriteLine("\nIn-Order Traversal: " + string.Join(" ", intBst.InOrderTraversal()));
Console.WriteLine("Pre-Order Traversal: " + string.Join(" ", intBst.PreOrderTraversal()));
Console.WriteLine("Post-Order Traversal: " + string.Join(" ", intBst.PostOrderTraversal()));
Console.WriteLine("Level-Order Traversal: " + string.Join(" ", intBst.LevelOrderTraversal()));

// Test find and contains
Console.WriteLine($"\nContains 40: {intBst.Contains(40)}");
Console.WriteLine($"Contains 100: {intBst.Contains(100)}");
var node = intBst.Find(40);
Console.WriteLine($"Node with value 40 found: {node != null} (Parent: {(node?.Parent?.Data ?? 0)})");

// Test removal
Console.WriteLine("\nAfter removing 30:");
intBst.Remove(30);
Console.WriteLine("In-Order Traversal: " + string.Join(" ", intBst.InOrderTraversal()));
Console.WriteLine($"Tree count after removal: {intBst.Count}");

// 2. String values
Console.WriteLine("\n2. Binary Search Tree with strings:");
var stringBst = new BinarySearchTree<string>(new[] { "banana", "apple", "orange", "grape", "kiwi" });
Console.WriteLine("In-Order Traversal: " + string.Join(" ", stringBst.InOrderTraversal()));
Console.WriteLine($"Min value: {stringBst.GetMin()}");
Console.WriteLine($"Max value: {stringBst.GetMax()}");

// 3. AVL Tree for balanced operations
Console.WriteLine("\n3. AVL Tree (Self-Balancing):");
var avlTree = new AvlTree<int>();

// Insert values in a way that would cause imbalance in a regular BST
int[] avlValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
foreach (int value in avlValues)
{
    avlTree.Insert(value);
}

Console.WriteLine($"AVL Tree height: {avlTree.Height} (much better than regular BST which would be {avlValues.Length})");
Console.WriteLine($"AVL Tree count: {avlTree.Count}");
Console.WriteLine("In-Order Traversal: " + string.Join(" ", avlTree.InOrderTraversal()));

// 4. Serialization and Deserialization
Console.WriteLine("\n4. Tree Serialization:");
var originalTree = new BinarySearchTree<int>(new[] { 5, 3, 8, 2, 4, 7, 9 });
string serialized = originalTree.Serialize();
Console.WriteLine($"Serialized tree: {serialized}");

var deserializedTree = new BinarySearchTree<int>();
deserializedTree.Deserialize(serialized);
Console.WriteLine($"Deserialized tree: {string.Join(" ", deserializedTree.InOrderTraversal())}");

// 5. Performance comparison example (conceptual)
Console.WriteLine("\n5. Real-world capabilities:");
Console.WriteLine("- Generic implementation supporting any IComparable type");
Console.WriteLine("- Self-balancing AVL tree for guaranteed O(log n) operations");
Console.WriteLine("- Comprehensive traversal algorithms (in-order, pre-order, post-order, level-order)");
Console.WriteLine("- Serialization/deserialization for persistence");
Console.WriteLine("- Proper error handling and validation");
Console.WriteLine("- Parent references for bidirectional navigation");
Console.WriteLine("- IEnumerable interface for LINQ compatibility");
Console.WriteLine("- Thread-safe operation patterns (implementation-ready)");
Console.WriteLine("- Memory-efficient node structure");

Console.WriteLine("\n=== Enterprise Tree Implementation Complete ===");
